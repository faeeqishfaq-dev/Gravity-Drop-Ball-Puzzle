using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PhysicsDraw.UI
{
    public class MainMenuSettingsUI : MonoBehaviour
    {
        [Header("Settings Panel")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private CanvasGroup backgroundOverlay;
        [SerializeField] private RectTransform panelRect;
        
        [Header("Buttons")]
        [SerializeField] private Button soundButton;
        [SerializeField] private GameObject soundOnImage;
        [SerializeField] private GameObject soundOffImage;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button closeButton;

        [Header("Quit Confirmation")]
        [SerializeField] private GameObject quitConfirmPanel;
        [SerializeField] private CanvasGroup quitBackgroundOverlay;
        [SerializeField] private RectTransform quitPanelRect;
        [SerializeField] private Button quitYesButton;
        [SerializeField] private Button quitNoButton;

        private const string SOUND_PREF = "SoundEnabled";
        private bool isSoundEnabled = true;
        private bool isAnimating = false;

        private void Awake()
        {
            if (soundButton != null) soundButton.onClick.AddListener(ToggleSound);
            if (quitButton != null) quitButton.onClick.AddListener(OpenQuitConfirm);
            if (closeButton != null) closeButton.onClick.AddListener(CloseSettings);

            if (quitYesButton != null) quitYesButton.onClick.AddListener(QuitGame);
            if (quitNoButton != null) quitNoButton.onClick.AddListener(CloseQuitConfirm);

            // Load Sound Pref
            isSoundEnabled = PlayerPrefs.GetInt(SOUND_PREF, 1) == 1;
            UpdateSoundButtonUI();
            AudioListener.volume = isSoundEnabled ? 1f : 0f;
        }

        public void OpenSettings()
        {
            if (isAnimating || (settingsPanel != null && settingsPanel.activeSelf)) return;
            settingsPanel.SetActive(true);
            quitConfirmPanel.SetActive(false); // Ensure quit is closed
            StartCoroutine(AnimateOpen(backgroundOverlay, panelRect));
        }

        public void CloseSettings()
        {
            if (isAnimating || settingsPanel == null || !settingsPanel.activeSelf) return;
            StartCoroutine(AnimateClose(backgroundOverlay, panelRect, settingsPanel));
        }

        private void ToggleSound()
        {
            isSoundEnabled = !isSoundEnabled;
            PlayerPrefs.SetInt(SOUND_PREF, isSoundEnabled ? 1 : 0);
            PlayerPrefs.Save();
            UpdateSoundButtonUI();
            
            // Audio listener toggle
            AudioListener.volume = isSoundEnabled ? 1f : 0f;
        }

        private void UpdateSoundButtonUI()
        {
            if (soundOnImage != null) soundOnImage.SetActive(isSoundEnabled);
            if (soundOffImage != null) soundOffImage.SetActive(!isSoundEnabled);
        }

        private void OpenQuitConfirm()
        {
            if (isAnimating || (quitConfirmPanel != null && quitConfirmPanel.activeSelf)) return;
            quitConfirmPanel.SetActive(true);
            StartCoroutine(AnimateOpen(quitBackgroundOverlay, quitPanelRect));
        }

        private void CloseQuitConfirm()
        {
            if (isAnimating || quitConfirmPanel == null || !quitConfirmPanel.activeSelf) return;
            StartCoroutine(AnimateClose(quitBackgroundOverlay, quitPanelRect, quitConfirmPanel));
        }

        private void QuitGame()
        {
            Debug.Log("[MainMenuSettings] Quitting Game...");
#if UNITY_EDITOR
            // UnityEditor.EditorApplication.isPlaying = false; // Safe behavior/log use
#else
            Application.Quit();
#endif
        }

        private IEnumerator AnimateOpen(CanvasGroup overlay, RectTransform panel)
        {
            isAnimating = true;

            if (overlay != null) overlay.alpha = 0f;
            if (panel != null) panel.localScale = new Vector3(0.5f, 0.5f, 1f);

            float duration = 0.3f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                
                // Ease out back for panel (overshoot)
                float easeT = Mathf.Clamp01(t);
                float overshoot = 1.2f;
                float scale = 0.5f + 0.5f * (1f - Mathf.Pow(1f - easeT, 3f)) * overshoot;
                if (easeT > 0.7f)
                {
                     // Settle back to 1
                     scale = Mathf.Lerp(scale, 1f, (easeT - 0.7f) / 0.3f);
                }

                if (overlay != null) overlay.alpha = Mathf.Lerp(0f, 1f, easeT * 1.5f); // Fade in faster
                if (panel != null) panel.localScale = new Vector3(scale, scale, 1f);
                
                yield return null;
            }

            if (overlay != null) overlay.alpha = 1f;
            if (panel != null) panel.localScale = Vector3.one;

            isAnimating = false;
        }

        private IEnumerator AnimateClose(CanvasGroup overlay, RectTransform panel, GameObject rootToHide)
        {
            isAnimating = true;

            float duration = 0.2f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                
                if (overlay != null) overlay.alpha = Mathf.Lerp(1f, 0f, t);
                if (panel != null) panel.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.5f, 0.5f, 1f), t);
                
                yield return null;
            }

            if (overlay != null) overlay.alpha = 0f;
            if (rootToHide != null) rootToHide.SetActive(false);

            isAnimating = false;
        }
    }
}
