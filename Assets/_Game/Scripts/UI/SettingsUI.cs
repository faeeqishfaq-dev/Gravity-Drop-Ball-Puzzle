using UnityEngine;
using UnityEngine.UI;
using PhysicsDraw.Save;

namespace PhysicsDraw.UI
{
    public class SettingsUI : MonoBehaviour
    {
        public Toggle soundToggle;
        public Toggle musicToggle;
        public Toggle hapticsToggle;

        private void OnEnable()
        {
            var data = SaveManager.Instance.CurrentData;
            if (soundToggle != null) soundToggle.isOn = data.soundEnabled;
            if (musicToggle != null) musicToggle.isOn = data.musicEnabled;
            if (hapticsToggle != null) hapticsToggle.isOn = data.hapticsEnabled;
        }

        public void SaveSettings()
        {
            var data = SaveManager.Instance.CurrentData;
            if (soundToggle != null) data.soundEnabled = soundToggle.isOn;
            if (musicToggle != null) data.musicEnabled = musicToggle.isOn;
            if (hapticsToggle != null) data.hapticsEnabled = hapticsToggle.isOn;
            SaveManager.Instance.SaveGame();
            
            gameObject.SetActive(false);
        }
    }
}
