using UnityEngine;
using PhysicsDraw.Level;
using PhysicsDraw.Core;

namespace PhysicsDraw.Scripts.Core
{
    public class GameplayBootstrapper : MonoBehaviour
    {
        private void Start()
        {
            if (LevelManager.Instance == null)
            {
                Debug.LogError("[GameplayBootstrapper] LevelManager is missing. Start from Bootstrap scene!");
                return;
            }

            int levelToLoad = PlayerPrefs.GetInt("LoadLevelID", 1);
            LevelManager.Instance.LoadLevel(levelToLoad);
        }
    }
}
