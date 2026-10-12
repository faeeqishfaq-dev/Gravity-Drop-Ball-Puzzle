using UnityEngine;
using PhysicsDraw.Core;
using PhysicsDraw.Level;

namespace PhysicsDraw.EditorScripts
{
    public class TestSceneController : MonoBehaviour
    {
        private void Start()
        {
            if (PlayerPrefs.HasKey("EditorTestLevelID"))
            {
                int levelId = PlayerPrefs.GetInt("EditorTestLevelID");
                PlayerPrefs.DeleteKey("EditorTestLevelID"); // Clear it so it doesn't affect normal play
                
                if (LevelManager.Instance != null)
                {
                    LevelManager.Instance.LoadLevel(levelId);
                }
            }
            else
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ChangeState(GameState.Playing);
                }
            }
        }
    }
}
