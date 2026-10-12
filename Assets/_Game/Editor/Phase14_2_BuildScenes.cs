using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using PhysicsDraw.UI; // Assuming some UI scripts

namespace PhysicsDraw.EditorScripts
{
    public static class Phase14_2_BuildScenes
    {
        [MenuItem("PhysicsDraw/Phase 14.2 - Build Bootstrap & MainMenu")]
        public static void BuildScenes()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.LogWarning("Cannot run while in Play Mode.");
                return;
            }

            BuildBootstrap();
            BuildMainMenu();
        }

        private static void BuildBootstrap()
        {
            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/Bootstrap.unity", OpenSceneMode.Single);
            
            // Clean up old canvas
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.GetComponent<Canvas>() != null || go.name == "EventSystem")
                    GameObject.DestroyImmediate(go);
            }

            // Canvas
            GameObject canvasObj = new GameObject("BootstrapCanvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObj.AddComponent<GraphicRaycaster>();

            // Event System
            GameObject esObj = new GameObject("EventSystem");
            esObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

            // Background
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(canvasObj.transform, false);
            var bgImg = bgObj.AddComponent<Image>();
            bgImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/NotebookGrid.png");
            bgImg.type = Image.Type.Tiled;
            var bgRt = bgObj.GetComponent<RectTransform>();
            bgRt.anchorMin = Vector2.zero; bgRt.anchorMax = Vector2.one;
            bgRt.sizeDelta = Vector2.zero;

            // Logo Text
            GameObject logoObj = new GameObject("Logo");
            logoObj.transform.SetParent(canvasObj.transform, false);
            var logoTxt = logoObj.AddComponent<Text>();
            logoTxt.text = "PHYSICS DROP";
            logoTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            logoTxt.fontStyle = FontStyle.Bold;
            logoTxt.fontSize = 120;
            logoTxt.alignment = TextAnchor.MiddleCenter;
            logoTxt.color = new Color(0.1f, 0.3f, 0.8f); // Blue
            var logoRt = logoObj.GetComponent<RectTransform>();
            logoRt.anchoredPosition = new Vector2(0, 300);
            logoRt.sizeDelta = new Vector2(800, 200);

            // Loading Bar bg
            GameObject barBgObj = new GameObject("LoadingBarBg");
            barBgObj.transform.SetParent(canvasObj.transform, false);
            var barBgImg = barBgObj.AddComponent<Image>();
            barBgImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/RoundedRect.png");
            barBgImg.type = Image.Type.Sliced;
            barBgImg.color = new Color(0.8f, 0.8f, 0.8f);
            var bbgRt = barBgObj.GetComponent<RectTransform>();
            bbgRt.anchoredPosition = new Vector2(0, -300);
            bbgRt.sizeDelta = new Vector2(600, 40);

            // Loading Bar fill
            GameObject barFillObj = new GameObject("LoadingBarFill");
            barFillObj.transform.SetParent(barBgObj.transform, false);
            var barFillImg = barFillObj.AddComponent<Image>();
            barFillImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/RoundedRect.png");
            barFillImg.type = Image.Type.Sliced;
            barFillImg.color = new Color(0.2f, 0.8f, 0.2f);
            var bfRt = barFillObj.GetComponent<RectTransform>();
            bfRt.anchorMin = new Vector2(0, 0); bfRt.anchorMax = new Vector2(0, 1);
            bfRt.pivot = new Vector2(0, 0.5f);
            bfRt.anchoredPosition = new Vector2(0, 0);
            bfRt.sizeDelta = new Vector2(500, 0); // 80% full visually for static view

            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/Bootstrap.unity");
            Debug.Log("[Phase 14.2] Bootstrap Scene UI Built.");
        }

        private static void BuildMainMenu()
        {
            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/MainMenu.unity", OpenSceneMode.Single);
            
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.GetComponent<Canvas>() != null || go.name == "EventSystem")
                    GameObject.DestroyImmediate(go);
            }

            GameObject canvasObj = new GameObject("MainMenuCanvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObj.AddComponent<GraphicRaycaster>();

            GameObject esObj = new GameObject("EventSystem");
            esObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

            // Background
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(canvasObj.transform, false);
            var bgImg = bgObj.AddComponent<Image>();
            bgImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/NotebookGrid.png");
            bgImg.type = Image.Type.Tiled;
            var bgRt = bgObj.GetComponent<RectTransform>();
            bgRt.anchorMin = Vector2.zero; bgRt.anchorMax = Vector2.one;
            bgRt.sizeDelta = Vector2.zero;

            // Header
            GameObject logoObj = new GameObject("HeaderTitle");
            logoObj.transform.SetParent(canvasObj.transform, false);
            var logoTxt = logoObj.AddComponent<Text>();
            logoTxt.text = "PHYSICS\nPUZZLES";
            logoTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            logoTxt.fontStyle = FontStyle.Bold;
            logoTxt.fontSize = 140;
            logoTxt.alignment = TextAnchor.MiddleCenter;
            logoTxt.color = new Color(0.1f, 0.3f, 0.8f);
            var logoRt = logoObj.GetComponent<RectTransform>();
            logoRt.anchorMin = new Vector2(0.5f, 1); logoRt.anchorMax = new Vector2(0.5f, 1);
            logoRt.pivot = new Vector2(0.5f, 1);
            logoRt.anchoredPosition = new Vector2(0, -200);
            logoRt.sizeDelta = new Vector2(1000, 300);

            // Play Button
            var playBtn = CreateButton(canvasObj.transform, "PlayButton", "PLAY", new Vector2(0, 100), new Vector2(600, 200), new Color(0.2f, 0.8f, 0.2f));
            
            // Levels Button
            var lvlBtn = CreateButton(canvasObj.transform, "LevelsButton", "LEVELS", new Vector2(0, -150), new Vector2(600, 160), new Color(1f, 0.6f, 0f));

            // Settings Button
            var setBtn = CreateButton(canvasObj.transform, "SettingsButton", "SETTINGS", new Vector2(0, -350), new Vector2(600, 160), new Color(0.4f, 0.4f, 0.4f));

            // Wire up script
            var menuUI = canvasObj.AddComponent<MainMenuUI>();
            
            // Level Select Panel (Hidden)
            GameObject lvlPanel = new GameObject("LevelSelectPanel");
            lvlPanel.transform.SetParent(canvasObj.transform, false);
            var pImg = lvlPanel.AddComponent<Image>();
            pImg.color = new Color(0.95f, 0.95f, 0.95f, 1f);
            var pRt = lvlPanel.GetComponent<RectTransform>();
            pRt.anchorMin = Vector2.zero; pRt.anchorMax = Vector2.one;
            pRt.sizeDelta = Vector2.zero;

            GameObject lvlTitle = new GameObject("Title");
            lvlTitle.transform.SetParent(lvlPanel.transform, false);
            var lTxt = lvlTitle.AddComponent<Text>();
            lTxt.text = "SELECT LEVEL";
            lTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            lTxt.fontSize = 80;
            lTxt.alignment = TextAnchor.MiddleCenter;
            lTxt.color = Color.black;
            var ltRt = lvlTitle.GetComponent<RectTransform>();
            ltRt.anchorMin = new Vector2(0, 1); ltRt.anchorMax = new Vector2(1, 1);
            ltRt.pivot = new Vector2(0.5f, 1);
            ltRt.anchoredPosition = new Vector2(0, -100);
            ltRt.sizeDelta = new Vector2(0, 150);

            var backBtn = CreateButton(lvlPanel.transform, "BackButton", "BACK", new Vector2(0, -200), new Vector2(400, 120), Color.gray);
            var bRt = backBtn.GetComponent<RectTransform>();
            bRt.anchorMin = new Vector2(0.5f, 0); bRt.anchorMax = new Vector2(0.5f, 0);
            bRt.pivot = new Vector2(0.5f, 0);
            bRt.anchoredPosition = new Vector2(0, 100);

            // Re-map scripts (Assuming LevelSelectUI exists on lvlPanel)
            var lsUI = lvlPanel.AddComponent<LevelSelectUI>();

            lvlPanel.SetActive(false);

            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/MainMenu.unity");
            Debug.Log("[Phase 14.2] MainMenu Scene UI Built.");
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
