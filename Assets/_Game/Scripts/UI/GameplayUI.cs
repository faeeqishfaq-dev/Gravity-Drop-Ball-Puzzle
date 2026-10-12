using UnityEngine;
using UnityEngine.UI;
using PhysicsDraw.Core;
using PhysicsDraw.Level;
using PhysicsDraw.Drawing;

namespace PhysicsDraw.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private Button btnUndo;
        [SerializeField] private Button btnRestart;

        private void Start()
        {
            if (btnUndo == null)
            {
                var t = transform.Find("UndoButton");
                if (t != null) btnUndo = t.GetComponent<Button>();
            }
            if (btnRestart == null)
            {
                var t = transform.Find("RetryButton");
                if (t != null) btnRestart = t.GetComponent<Button>();
            }

            if (btnUndo != null)
                btnUndo.onClick.AddListener(OnUndoClicked);
            
            if (btnRestart != null)
                btnRestart.onClick.AddListener(OnRestartClicked);
                
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.OnLevelLoaded += UpdateLevelText;
            }
            
            // Manual update in case OnLevelLoaded fired before Start
            UpdateLevelText();
        }

        private void OnUndoClicked()
        {
            var drawingController = FindObjectOfType<DrawingController>();
            if (drawingController != null)
            {
                drawingController.UndoLastDrawing();
            }
        }

        private void OnRestartClicked()
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.ResetLevel();
            }
        }

        private void OnDestroy()
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.OnLevelLoaded -= UpdateLevelText;
            }
        }

        private void UpdateLevelText()
        {
            var db = Resources.Load<LevelDatabase>("LevelDatabase");
            if (db == null) return;
            
            if (LevelManager.Instance != null && LevelManager.Instance.CurrentLevelData != null)
            {
                var t = transform.Find("LevelText");
                if (t != null)
                {
                    var textComp = t.GetComponent<Text>();
                    if (textComp != null)
                    {
                        textComp.text = "LEVEL " + LevelManager.Instance.CurrentLevelData.levelID;
                    }
                }
            }
        }
    }
}
