using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace PhysicsDraw.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("[GameBootstrap] Initializing core systems...");
            GameManager.Instance.ChangeState(GameState.Boot);
            StartCoroutine(LoadingSequence());
        }

        private IEnumerator LoadingSequence()
        {
            GameManager.Instance.ChangeState(GameState.Loading);

            // Find Loading Bar
            Image fill = null;
            var canvas = GameObject.Find("BootstrapCanvas");
            if (canvas != null)
            {
                var barBg = canvas.transform.Find("LoadingBarBg");
                if (barBg != null)
                {
                    var barFill = barBg.Find("LoadingBarFill");
                    if (barFill != null) fill = barFill.GetComponent<Image>();
                }
            }

            float duration = 1.5f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                if (fill != null)
                {
                    var rt = fill.rectTransform;
                    float pct = elapsed / duration;
                    // width is up to 580 (inside 600 bg)
                    rt.sizeDelta = new Vector2(580f * pct, 0); 
                }
                yield return null;
            }
            CheckCpAd.instance.CallAd();
            // Load Main Menu
            SceneManager.LoadScene("MainMenu"); 
        }
    }
}
