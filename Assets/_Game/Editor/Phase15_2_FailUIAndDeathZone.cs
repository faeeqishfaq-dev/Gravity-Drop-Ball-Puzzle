using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using PhysicsDraw.UI;
using PhysicsDraw.Level;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase15_2_FailUIAndDeathZone
    {
        static Phase15_2_FailUIAndDeathZone()
        {
            if (!SessionState.GetBool("Phase15_2_Done", false))
            {
                SessionState.SetBool("Phase15_2_Done", true);
                EditorApplication.delayCall += BuildFailUI;
            }
        }

        [MenuItem("PhysicsDraw/Phase 15.2 - Add Level Fail UI & DeathZone")]
        public static void BuildFailUI()
        {
            if (EditorApplication.isPlaying) return;

            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/Gameplay.unity", OpenSceneMode.Single);
            
            Transform canvasTrans = null;
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.name == "GameplayCanvas") canvasTrans = go.transform;
            }

            if (canvasTrans != null)
            {
                // Remove old if exists
                var old = canvasTrans.Find("LevelFailPanel");
                if (old != null) GameObject.DestroyImmediate(old.gameObject);

                GameObject panelObj = new GameObject("LevelFailPanel");
                panelObj.transform.SetParent(canvasTrans, false);
                var pRt = panelObj.AddComponent<RectTransform>();
                pRt.anchorMin = Vector2.zero; pRt.anchorMax = Vector2.one;
                pRt.sizeDelta = Vector2.zero;
                panelObj.AddComponent<LevelFailUI>();

                GameObject overObj = new GameObject("DarkOverlay");
                overObj.transform.SetParent(panelObj.transform, false);
                var overImg = overObj.AddComponent<Image>();
                overImg.color = new Color(0.2f, 0, 0, 0.8f);
                var oRt = overObj.GetComponent<RectTransform>();
                oRt.anchorMin = Vector2.zero; oRt.anchorMax = Vector2.one;
                oRt.sizeDelta = Vector2.zero;

                GameObject winObj = new GameObject("Window");
                winObj.transform.SetParent(panelObj.transform, false);
                var winImg = winObj.AddComponent<Image>();
                winImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/RoundedRect.png");
                winImg.type = Image.Type.Sliced;
                var wRt = winObj.GetComponent<RectTransform>();
                wRt.anchoredPosition = Vector2.zero;
                wRt.sizeDelta = new Vector2(800, 600);

                GameObject titleObj = new GameObject("Title");
                titleObj.transform.SetParent(winObj.transform, false);
                var tTxt = titleObj.AddComponent<Text>();
                tTxt.text = "LEVEL FAILED";
                tTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                tTxt.fontSize = 70;
                tTxt.fontStyle = FontStyle.Bold;
                tTxt.alignment = TextAnchor.MiddleCenter;
                tTxt.color = new Color(0.8f, 0.1f, 0.1f);
                var tRt = titleObj.GetComponent<RectTransform>();
                tRt.anchorMin = new Vector2(0, 1); tRt.anchorMax = new Vector2(1, 1);
                tRt.pivot = new Vector2(0.5f, 1);
                tRt.anchoredPosition = new Vector2(0, -100);
                tRt.sizeDelta = new Vector2(0, 150);

                var retryBtn = CreateButton(winObj.transform, "RetryButton", "RETRY", new Vector2(0, -50), new Vector2(600, 150), new Color(1f, 0.6f, 0f));
                var menuBtn = CreateButton(winObj.transform, "MenuButton", "MAIN MENU", new Vector2(0, -250), new Vector2(600, 150), new Color(0.2f, 0.6f, 1f));

                panelObj.SetActive(false);
            }

            // 2. Add DeathZone to scene
            GameObject dzObj = GameObject.Find("GlobalDeathZone");
            if (dzObj == null)
            {
                dzObj = new GameObject("GlobalDeathZone");
            }
            dzObj.transform.position = new Vector3(0, -15, 0);
            
            var bc = dzObj.GetComponent<BoxCollider2D>();
            if (bc == null) bc = dzObj.AddComponent<BoxCollider2D>();
            bc.isTrigger = true;
            bc.size = new Vector2(100, 2);

            if (dzObj.GetComponent<DeathZone>() == null) dzObj.AddComponent<DeathZone>();

            // Assign proper layer so only ball triggers it if desired, but code filters by tag/name.

            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/Gameplay.unity");
            Debug.Log("[Phase 15.2] Level Fail UI and DeathZone Added.");
        }

        private static GameObject CreateButton(Transform parent, string name, string text, Vector2 pos, Vector2 size, Color color)
        {
            GameObject btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent, false);
            var img = btnObj.AddComponent<Image>();
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/RoundedRect.png");
            img.type = Image.Type.Sliced;
            img.color = color;
            btnObj.AddComponent<Button>();

            var rt = btnObj.GetComponent<RectTransform>();
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;

            GameObject txtObj = new GameObject("Text");
            txtObj.transform.SetParent(btnObj.transform, false);
            var txt = txtObj.AddComponent<Text>();
            txt.text = text;
            txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txt.fontSize = (int)(size.y * 0.4f);
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;
            var trt = txtObj.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
            trt.sizeDelta = Vector2.zero;

            return btnObj;
        }
    }
}
