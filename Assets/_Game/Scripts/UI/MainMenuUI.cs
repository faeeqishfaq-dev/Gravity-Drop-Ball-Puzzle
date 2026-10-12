using UnityEngine;
using UnityEngine.SceneManagement;
using PhysicsDraw.Save;

namespace PhysicsDraw.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        public GameObject levelSelectPanel;

        private void Start()
        {
            var play = transform.Find("PlayButton");
            if (play != null) play.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PlayGame);

            var lvl = transform.Find("LevelsButton");
            if (lvl != null) lvl.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenLevelSelect);

            var set = transform.Find("SettingsButton");
            if (set != null) set.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenSettings);

            if (levelSelectPanel == null)
            {
                var p = transform.Find("LevelSelectPanel");
                if (p != null) levelSelectPanel = p.gameObject;
            }

            if (levelSelectPanel != null)
            {
                var back = levelSelectPanel.transform.Find("BackButton");
                if (back != null) back.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CloseLevelSelect);
            }
        }

        public void PlayGame()
        {
            int highest = SaveManager.Instance.CurrentData.highestUnlockedLevel;
            PlayerPrefs.SetInt("LoadLevelID", highest);
            CheckCpAd.instance.CallAd();
            SceneManager.LoadScene("Gameplay");
        }

        public void OpenSettings()
        {
            Debug.Log("[MainMenu] Settings opened.");
            var settings = FindObjectOfType<MainMenuSettingsUI>(true);
            if (settings != null)
            {
                settings.OpenSettings();
            }
        }

        public void OpenLevelSelect()
        {
            if (levelSelectPanel != null) levelSelectPanel.SetActive(true);
        }

        public void CloseLevelSelect()
        {
            if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        }
    }
}
