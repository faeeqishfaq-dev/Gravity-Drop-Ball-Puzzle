using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PhysicsDraw.Save;
using PhysicsDraw.Level;

namespace PhysicsDraw.UI
{
    public class LevelSelectUI : MonoBehaviour
    {
        public GameObject buttonPrefab;
        public Transform contentPanel;
        
        private void Start()
        {
            PopulateLevels();
        }

        private void PopulateLevels()
        {
            var db = Resources.Load<LevelDatabase>("LevelDatabase");
            if (db == null) return;

            int highestUnlocked = SaveManager.Instance.CurrentData.highestUnlockedLevel;

            foreach (var level in db.allLevels)
            {
                GameObject btnObj = Instantiate(buttonPrefab, contentPanel);
                var btn = btnObj.GetComponent<Button>();
                var text = btnObj.transform.Find("NumberText").GetComponent<Text>();
                var lockIcon = btnObj.transform.Find("LockIcon").gameObject;
                
                text.text = level.levelID.ToString();

                if (level.levelID <= highestUnlocked)
                {
                    btn.interactable = true;
                    lockIcon.SetActive(false);
                    int id = level.levelID;
                    btn.onClick.AddListener(() => LoadLevel(id));
                }
                else
                {
                    btn.interactable = false;
                    btn.image.color = new Color(0.8f, 0.8f, 0.8f);
                    text.gameObject.SetActive(false); // Hide number if locked
                    lockIcon.SetActive(true);
                }
            }
        }

        private void LoadLevel(int id)
        {
            PlayerPrefs.SetInt("LoadLevelID", id);
            SceneManager.LoadScene("Gameplay");
        }
    }
}
