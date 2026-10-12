using System.Collections.Generic;
using UnityEngine;

namespace PhysicsDraw.Save
{
    [System.Serializable]
    public class SaveData
    {
        public int highestUnlockedLevel = 1;
        public Dictionary<int, int> levelStars = new Dictionary<int, int>();
        public Dictionary<int, float> levelBestInk = new Dictionary<int, float>();
        public bool soundEnabled = true;
        public bool musicEnabled = true;
        public bool hapticsEnabled = true;
    }

    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        public SaveData CurrentData { get; private set; }

        private const string SAVE_KEY = "PhysicsDraw_SaveData";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadGame();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadGame()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                string json = PlayerPrefs.GetString(SAVE_KEY);
                CurrentData = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
            }
            else
            {
                CurrentData = new SaveData();
            }
            Debug.Log("[SaveManager] Game Loaded.");
        }

        public void SaveGame()
        {
            if (CurrentData != null)
            {
                string json = JsonUtility.ToJson(CurrentData);
                PlayerPrefs.SetString(SAVE_KEY, json);
                PlayerPrefs.Save();
                Debug.Log("[SaveManager] Game Saved.");
            }
        }
    }
}
