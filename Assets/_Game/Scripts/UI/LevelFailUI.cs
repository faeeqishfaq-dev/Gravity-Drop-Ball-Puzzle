using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PhysicsDraw.Core;
using PhysicsDraw.Level;

namespace PhysicsDraw.UI
{
    public class LevelFailUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        private bool isAnimating = false;

        private void Start()
        {
            if (panel == null) panel = gameObject;
            panel.SetActive(false);
            
            var window = panel.transform.Find("Window");
            if (window != null)
            {
                var retryBtn = window.Find("RetryButton");
                if (retryBtn != null) retryBtn.GetComponent<Button>().onClick.AddListener(RetryLevel);

                var menuBtn = window.Find("MenuButton");
                if (menuBtn != null) menuBtn.GetComponent<Button>().onClick.AddListener(MainMenu);
            }
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameState.Failed)
            {
                if (panel != null && !panel.activeSelf && !isAnimating)
                {
                    StartCoroutine(AnimateFail());
                }
            }
        }

        private IEnumerator AnimateFail()
        {
            isAnimating = true;
            panel.SetActive(true);

            Image overlay = null;
            if (overlay != null) overlay.color = new Color(0.2f, 0, 0, 0f);

            CanvasGroup titleGroup = null;
            CanvasGroup btnGroup1 = null;
            CanvasGroup btnGroup2 = null;
            RectTransform window = null;

            var ov = panel.transform.Find("DarkOverlay");
            if (ov != null) overlay = ov.GetComponent<Image>();

            var win = panel.transform.Find("Window");
            if (win != null)
            {
                window = win.GetComponent<RectTransform>();
                
                var t = win.Find("Title");
                if (t != null)
                {
                    titleGroup = t.GetComponent<CanvasGroup>();
                    if (titleGroup == null) titleGroup = t.gameObject.AddComponent<CanvasGroup>();
                }

                var rb = win.Find("RetryButton");
                if (rb != null)
                {
                    btnGroup1 = rb.GetComponent<CanvasGroup>();
                    if (btnGroup1 == null) btnGroup1 = rb.gameObject.AddComponent<CanvasGroup>();
                }

                var mb = win.Find("MenuButton");
                if (mb != null)
                {
                    btnGroup2 = mb.GetComponent<CanvasGroup>();
                    if (btnGroup2 == null) btnGroup2 = mb.gameObject.AddComponent<CanvasGroup>();
                }
            }

            // Initialization
            if (window != null)
            {
                window.localScale = Vector3.zero;
                window.anchoredPosition = new Vector2(0, -500f);
            }
            if (titleGroup != null) titleGroup.alpha = 0f;
            if (btnGroup1 != null) btnGroup1.alpha = 0f;
            if (btnGroup2 != null) btnGroup2.alpha = 0f;

            float elapsed = 0f;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;

                if (overlay != null)
                {
                    overlay.color = new Color(0.2f, 0, 0, Mathf.Lerp(0f, 0.8f, t));
                }

                if (window != null)
                {
                    window.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Clamp01(t * 1.5f));
                    window.anchoredPosition = Vector2.Lerp(new Vector2(0, -500f), Vector2.zero, t);
                }

                if (t > 0.5f)
                {
                    float alphaT = (t - 0.5f) * 2f;
                    if (titleGroup != null) titleGroup.alpha = Mathf.Lerp(0f, 1f, alphaT);
                    if (btnGroup1 != null) btnGroup1.alpha = Mathf.Lerp(0f, 1f, alphaT);
                    if (btnGroup2 != null) btnGroup2.alpha = Mathf.Lerp(0f, 1f, alphaT);
                }

                yield return null;
            }

            // Ensure final values
            if (overlay != null) overlay.color = new Color(0.2f, 0, 0, 0.8f);
            if (window != null) { window.localScale = Vector3.one; window.anchoredPosition = Vector2.zero; }
            if (titleGroup != null) titleGroup.alpha = 1f;
            if (btnGroup1 != null) btnGroup1.alpha = 1f;
            if (btnGroup2 != null) btnGroup2.alpha = 1f;
            
            isAnimating = false;
        }

        public void RetryLevel()
        {
            if (panel != null)
            {
                if (!panel.activeSelf) return; // Prevent double-execution
                panel.SetActive(false);
            }

            if (LevelManager.Instance != null)
            {
                CheckCpAd.instance.CallAd();
                LevelManager.Instance.ResetLevel();
            }
        }

        public void MainMenu()
        {
            CheckCpAd.instance.CallAd();
            Time.timeScale = 1f;
            if (GameManager.Instance != null) GameManager.Instance.ChangeState(GameState.Ready);

            if (panel != null)
            {
                if (!panel.activeSelf) return;
                panel.SetActive(false);
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}
