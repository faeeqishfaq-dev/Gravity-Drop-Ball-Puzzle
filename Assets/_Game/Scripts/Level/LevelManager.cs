using System;
using System.Collections.Generic;
using UnityEngine;
using PhysicsDraw.Core;

namespace PhysicsDraw.Level
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        public event Action OnLevelLoaded;
        public event Action OnLevelReset;

        public LevelData CurrentLevelData { get; private set; }
        private GameObject currentLevelInstance;
        private int currentLevelId = 1;

        private List<IResettable> resettableObjects = new List<IResettable>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            if (scene.name == "Gameplay")
            {
                int levelToLoad = PlayerPrefs.GetInt("LoadLevelID", 1);
                LoadLevel(levelToLoad);
            }
            else if (scene.name == "MainMenu")
            {
                // Explicitly completely reset runtime session state as requested
                Time.timeScale = 1f;
                if (GameManager.Instance != null) GameManager.Instance.ChangeState(GameState.Ready);
                
                var dc = FindObjectOfType<PhysicsDraw.Drawing.DrawingController>();
                if (dc != null)
                {
                    dc.ClearAllDrawings();
                }

                // Clean up any lingering level instance when returning to menu
                if (currentLevelInstance != null)
                {
                    Destroy(currentLevelInstance);
                    currentLevelInstance = null;
                }
                resettableObjects.Clear();
            }
        }

        private void Start()
        {
            // Initial load handled by sceneLoaded if Gameplay is active, 
            // but just in case we started directly in Gameplay:
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Gameplay")
            {
                int levelToLoad = PlayerPrefs.GetInt("LoadLevelID", 1);
                LoadLevel(levelToLoad);
            }
        }

        public void LoadLevel(int levelId)
        {
            var db = Resources.Load<LevelDatabase>("LevelDatabase");
            if (db == null)
            {
                Debug.LogError("[LevelManager] LevelDatabase not found in Resources!");
                return;
            }

            var data = db.GetLevel(levelId);
            if (data == null)
            {
                Debug.LogError($"[LevelManager] Level {levelId} not found in database.");
                return;
            }

            LoadLevel(data);
        }

        public void LoadLevel(LevelData data)
        {
            // Clean up previous level drawings/state
            var dc = FindObjectOfType<PhysicsDraw.Drawing.DrawingController>();
            if (dc != null)
            {
                dc.ClearAllDrawings();
            }

            foreach (var r in resettableObjects)
            {
                if (r is MonoBehaviour mb && mb != null)
                {
                    r.ResetObject();
                }
            }
            resettableObjects.Clear();

            if (currentLevelInstance != null)
            {
                Destroy(currentLevelInstance);
            }

            CurrentLevelData = data;
            currentLevelId = data.levelID;
            
            // Fix: ensure the game knows which level is active if scene reloads
            PlayerPrefs.SetInt("LoadLevelID", currentLevelId);
            PlayerPrefs.Save();

            if (data.levelPrefab != null)
            {
                currentLevelInstance = Instantiate(data.levelPrefab);
            }
            else
            {
                Debug.LogWarning($"[LevelManager] LevelData {data.levelID} has no prefab assigned.");
            }

            ApplyLevelSettings(data);
            FindResettables();

            if (data.ballStartMode == BallStartMode.OnFirstDraw)
            {
                GameManager.Instance.ChangeState(GameState.Ready);
            }
            else
            {
                GameManager.Instance.ChangeState(GameState.Playing);
            }
            
            OnLevelLoaded?.Invoke();
        }

        private void ApplyLevelSettings(LevelData data)
        {
            if (GravityManager.Instance != null)
            {
                GravityManager.Instance.SetDefaultGravity(data.gravityDirection * data.gravityStrength);
            }
            else
            {
                Physics2D.gravity = data.gravityDirection * data.gravityStrength;
            }

            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                var camController = mainCam.GetComponent<CameraController>();
                if (camController == null) camController = mainCam.gameObject.AddComponent<CameraController>();

                if (currentLevelInstance != null)
                {
                    var bounds = currentLevelInstance.GetComponentInChildren<LevelBounds>();
                    if (bounds != null)
                    {
                        camController.FrameLevel(bounds);
                    }
                    else
                    {
                        // Fallback
                        mainCam.transform.position = data.cameraPosition;
                        mainCam.orthographicSize = data.orthographicSize;
                    }
                }
            }
            
            // Connect Goal event ONLY from the new level instance
            if (currentLevelInstance != null)
            {
                var goal = currentLevelInstance.GetComponentInChildren<Goal>();
                if (goal != null)
                {
                    goal.OnGoalAchieved += HandleGoalAchieved;
                }
            }
        }

        private void FindResettables()
        {
            resettableObjects.Clear();
            if (currentLevelInstance != null)
            {
                var allResettables = currentLevelInstance.GetComponentsInChildren<MonoBehaviour>();
                foreach (var r in allResettables)
                {
                    if (r is IResettable resettable)
                    {
                        resettableObjects.Add(resettable);
                    }
                }
            }
        }

        public void ResetLevel()
        {
            Debug.Log("[LevelManager] Hard Resetting Level Prefab...");
            if (CurrentLevelData != null)
            {
                LoadLevel(CurrentLevelData);
            }
            OnLevelReset?.Invoke();
        }

        private void HandleGoalAchieved()
        {
            GameManager.Instance.ChangeState(GameState.Success);

            // Unlock next level
            Save.SaveManager.Instance.CurrentData.highestUnlockedLevel = Mathf.Max(
                Save.SaveManager.Instance.CurrentData.highestUnlockedLevel, 
                currentLevelId + 1
            );
            Save.SaveManager.Instance.SaveGame();
        }

        public void LoadNextLevel()
        {
            Debug.Log($"[LevelManager] LoadNextLevel called. Current ID: {currentLevelId}");
            
            // Force timeScale to 1 just in case it was paused
            Time.timeScale = 1f;

            var db = Resources.Load<LevelDatabase>("LevelDatabase");
            if (db != null)
            {
                var data = db.GetLevel(currentLevelId + 1);
                if (data != null)
                {
                    Debug.Log($"[LevelManager] Found next level: {data.levelID}. Loading...");
                    LoadLevel(data);
                }
                else
                {
                    Debug.Log("[LevelManager] Final level reached. Returning to Main Menu.");
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                }
            }
            else
            {
                Debug.LogError("[LevelManager] LevelDatabase is NULL!");
                LoadLevel(currentLevelId + 1);
            }
        }
    }
}
