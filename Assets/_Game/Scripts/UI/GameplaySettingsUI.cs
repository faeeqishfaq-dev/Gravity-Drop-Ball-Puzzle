using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PhysicsDraw.Core;
using PhysicsDraw.Level;
using System.Collections;

namespace PhysicsDraw.UI
{
    public class GameplaySettingsUI : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject popupPanel;
        public Button openSettingsBtn;
        public Button closeBtn;
        public Button replayBtn;
        public Button mainMenuBtn;
        public Image darkOverlay;
        
        [Header("Animation")]
        public RectTransform popupRect;

        private GameState previousState;

        private void Start()
        {
            if (popupPanel == null) popupPanel = gameObject;
            popupPanel.SetActive(false);

            if (openSettingsBtn == null)
            {
                var topHud = GameObject.Find("TopHUD");
                if (topHud != null)
                {
                    var btnTransform = topHud.transform.Find("SettingsButton");
                    if (btnTransform != null)
                    {
                        openSettingsBtn = btnTransform.GetComponent<Button>();
                        if (openSettingsBtn == null)
                        {
                            openSettingsBtn = btnTransform.gameObject.AddComponent<Button>();
                        }
                    }
                }
            }

            if (openSettingsBtn != null) openSettingsBtn.onClick.AddListener(OpenSettings);

            var window = popupPanel.transform.Find("Window");
            if (window != null)
            {
                if (popupRect == null) popupRect = window.GetComponent<RectTransform>();
                
                if (closeBtn == null)
                {
                    var c = window.Find("CloseButton");
                    if (c != null) closeBtn = c.GetComponent<Button>();
                }
                if (replayBtn == null)
                {
                    var r = window.Find("ReplayButton");
                    if (r != null) replayBtn = r.GetComponent<Button>();
                }
                if (mainMenuBtn == null)
                {
                    var m = window.Find("MainMenuButton");
                    if (m != null) mainMenuBtn = m.GetComponent<Button>();
                }
                
                if (darkOverlay == null)
                {
                    var o = popupPanel.transform.Find("DarkOverlay");
                    if (o != null) darkOverlay = o.GetComponent<Image>();
                }
            }

            if (closeBtn != null) closeBtn.onClick.AddListener(CloseSettings);
            if (replayBtn != null) replayBtn.onClick.AddListener(OnReplayClicked);
            if (mainMenuBtn != null) mainMenuBtn.onClick.AddListener(OnMainMenuClicked);
        }

        public void OpenSettings()
        {
            if (GameManager.Instance != null)
            {
                previousState = GameManager.Instance.CurrentState;
                GameManager.Instance.ChangeState(GameState.Paused);
            }
            
            popupPanel.SetActive(true);
            Time.timeScale = 0f; // Pause physics
            
            StopAllCoroutines();
            StartCoroutine(AnimatePopup(true));
        }

        public void CloseSettings()
        {
            Time.timeScale = 1f; // Resume physics
            
            if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameState.Paused)
            {
                GameManager.Instance.ChangeState(previousState);
            }
            
            StopAllCoroutines();
            StartCoroutine(AnimatePopup(false));
        }

        private IEnumerator AnimatePopup(bool opening)
        {
            float duration = 0.2f;
            float elapsed = 0f;

            if (popupRect != null)
            {
                Vector3 startScale = opening ? new Vector3(0.5f, 0.5f, 0.5f) : Vector3.one;
                Vector3 endScale = opening ? Vector3.one : new Vector3(0.5f, 0.5f, 0.5f);
                
                Color startColor = opening ? new Color(0, 0, 0, 0) : new Color(0, 0, 0, 0.7f);
                Color endColor = opening ? new Color(0, 0, 0, 0.7f) : new Color(0, 0, 0, 0);

                while (elapsed < duration)
                {
                    elapsed += Time.unscaledDeltaTime;
                    float t = elapsed / duration;
                    
                    // Easing out/in
                    float easeT = opening ? Mathf.Sin(t * Mathf.PI * 0.5f) : t * t;
                    
                    popupRect.localScale = Vector3.Lerp(startScale, endScale, easeT);
                    if (darkOverlay != null) darkOverlay.color = Color.Lerp(startColor, endColor, easeT);
                    
                    yield return null;
                }

                popupRect.localScale = endScale;
                if (darkOverlay != null) darkOverlay.color = endColor;
            }

            if (!opening)
            {
                popupPanel.SetActive(false);
            }
        }

        private void OnReplayClicked()
        {
            Time.timeScale = 1f;
            if (popupPanel != null) popupPanel.SetActive(false);
            
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.ResetLevel();
            }
        }

        private void OnMainMenuClicked()
        {
            Time.timeScale = 1f;
            if (Save.SaveManager.Instance != null)
            {
                Save.SaveManager.Instance.SaveGame();
            }
            SceneManager.LoadScene("MainMenu");
        }
    }
}
