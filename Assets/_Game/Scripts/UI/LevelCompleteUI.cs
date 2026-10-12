using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PhysicsDraw.Core;
using PhysicsDraw.Level;

namespace PhysicsDraw.UI
{
    public class LevelCompleteUI : MonoBehaviour
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
                var nextBtn = window.Find("NextButton");
                if (nextBtn != null) nextBtn.GetComponent<Button>().onClick.AddListener(NextLevel);

                var menuBtn = window.Find("MenuButton");
                if (menuBtn != null) menuBtn.GetComponent<Button>().onClick.AddListener(MainMenu);
            }
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameState.Success)
            {
                if (panel != null && !panel.activeSelf && !isAnimating)
                {
                    StartCoroutine(AnimateSuccess());
                }
            }
        }

        private IEnumerator AnimateSuccess()
        {
            isTransitioning = false;
            isAnimating = true;
            panel.SetActive(true);

            Image overlay = null;
            RectTransform window = null;
            CanvasGroup titleGroup = null;
            CanvasGroup btnGroup1 = null;
            CanvasGroup btnGroup2 = null;
            RectTransform btn1Rect = null;
            RectTransform btn2Rect = null;

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

                var nb = win.Find("NextButton");
                if (nb != null)
                {
                    btnGroup1 = nb.GetComponent<CanvasGroup>();
                    if (btnGroup1 == null) btnGroup1 = nb.gameObject.AddComponent<CanvasGroup>();
                    btn1Rect = nb.GetComponent<RectTransform>();
                }

                var mb = win.Find("MenuButton");
                if (mb != null)
                {
                    btnGroup2 = mb.GetComponent<CanvasGroup>();
                    if (btnGroup2 == null) btnGroup2 = mb.gameObject.AddComponent<CanvasGroup>();
                    btn2Rect = mb.GetComponent<RectTransform>();
                }
            }

            // Initialization
            if (overlay != null) overlay.color = new Color(0, 0, 0, 0);
            if (window != null) window.localScale = new Vector3(0.3f, 0.3f, 1f);
            if (titleGroup != null) titleGroup.alpha = 0f;
            
            float b1StartY = btn1Rect != null ? btn1Rect.anchoredPosition.y - 50f : 0;
            float b1TargetY = btn1Rect != null ? btn1Rect.anchoredPosition.y : 0;
            if (btnGroup1 != null) { btnGroup1.alpha = 0f; btn1Rect.anchoredPosition = new Vector2(btn1Rect.anchoredPosition.x, b1StartY); }
            
            float b2StartY = btn2Rect != null ? btn2Rect.anchoredPosition.y - 50f : 0;
            float b2TargetY = btn2Rect != null ? btn2Rect.anchoredPosition.y : 0;
            if (btnGroup2 != null) { btnGroup2.alpha = 0f; btn2Rect.anchoredPosition = new Vector2(btn2Rect.anchoredPosition.x, b2StartY); }

            float elapsed = 0f;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;

                // 1. Overlay fade (0 to 0.7)
                if (overlay != null) overlay.color = new Color(0, 0, 0, Mathf.Lerp(0f, 0.8f, t * 1.5f));

                // 2. Window pop with overshoot
                if (window != null)
                {
                    // Overshoot formula: sin(t * pi * 0.5) with a little extra scale
                    float scaleT = t < 0.6f ? Mathf.Lerp(0.3f, 1.1f, t / 0.6f) : Mathf.Lerp(1.1f, 1f, (t - 0.6f) / 0.4f);
                    window.localScale = new Vector3(scaleT, scaleT, 1f);
                }

                // 3. Title fade in
                if (titleGroup != null && t > 0.4f)
                {
                    titleGroup.alpha = Mathf.Lerp(0f, 1f, (t - 0.4f) / 0.3f);
                }

                // 4. Buttons fade and slide
                if (t > 0.6f)
                {
                    float btnT = (t - 0.6f) / 0.4f;
                    if (btnGroup1 != null)
                    {
                        btnGroup1.alpha = Mathf.Lerp(0f, 1f, btnT);
                        btn1Rect.anchoredPosition = new Vector2(btn1Rect.anchoredPosition.x, Mathf.Lerp(b1StartY, b1TargetY, btnT * btnT)); // fast slide
                    }
                    if (btnGroup2 != null)
                    {
                        btnGroup2.alpha = Mathf.Lerp(0f, 1f, btnT);
                        btn2Rect.anchoredPosition = new Vector2(btn2Rect.anchoredPosition.x, Mathf.Lerp(b2StartY, b2TargetY, btnT * btnT));
                    }
                }

                yield return null;
            }

            // Ensure final values
            if (overlay != null) overlay.color = new Color(0, 0, 0, 0.8f);
            if (window != null) { window.localScale = Vector3.one; window.anchoredPosition = Vector2.zero; }
            if (titleGroup != null) { titleGroup.alpha = 1f; titleGroup.blocksRaycasts = false; }
            if (btnGroup1 != null) { btnGroup1.alpha = 1f; btn1Rect.anchoredPosition = new Vector2(btn1Rect.anchoredPosition.x, b1TargetY); btnGroup1.blocksRaycasts = true; btnGroup1.interactable = true; }
            if (btnGroup2 != null) { btnGroup2.alpha = 1f; btn2Rect.anchoredPosition = new Vector2(btn2Rect.anchoredPosition.x, b2TargetY); btnGroup2.blocksRaycasts = true; btnGroup2.interactable = true; }

            // Double check physical buttons
            var nBtn = panel.transform.Find("Window/NextButton");
            if (nBtn != null)
            {
                var b = nBtn.GetComponent<Button>();
                if (b != null) b.interactable = true;
                nBtn.SetAsLastSibling(); // Force it to the front so NOTHING covers it
            }
            var mBtn = panel.transform.Find("Window/MenuButton");
            if (mBtn != null)
            {
                mBtn.SetAsLastSibling(); // Force it to the front so NOTHING covers it
            }
            
            isAnimating = false;
        }

        private bool isTransitioning = false;

        public void NextLevel()
        {
            CheckCpAd.instance.CallAd();
            Debug.Log("[LevelCompleteUI] NextLevel clicked!");
            if (isTransitioning) return;
            isTransitioning = true;
            
            if (panel != null)
            {
                panel.SetActive(false);
            }

            var lm = LevelManager.Instance;
            if (lm == null) lm = FindObjectOfType<LevelManager>();

            if (lm != null)
            {
                Debug.Log("[LevelCompleteUI] Calling LevelManager.LoadNextLevel()");
                lm.LoadNextLevel();
            }
            else
            {
                Debug.LogError("[LevelCompleteUI] LevelManager is NULL!");
            }
        }

        public void MainMenu()
        {
            CheckCpAd.instance.CallAd();
            if (isTransitioning) return;
            isTransitioning = true;
            
            Time.timeScale = 1f;
            if (GameManager.Instance != null) GameManager.Instance.ChangeState(GameState.Ready);

            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        public void Retry()
        {
            if (isTransitioning) return;
            isTransitioning = true;
            
            if (panel != null) panel.SetActive(false);
            if (LevelManager.Instance != null)
            {
                CheckCpAd.instance.CallAd();
                LevelManager.Instance.ResetLevel();
            }
        }
    }
}
