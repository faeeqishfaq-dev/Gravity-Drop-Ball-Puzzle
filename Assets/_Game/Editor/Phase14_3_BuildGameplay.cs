using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using PhysicsDraw.UI;

namespace PhysicsDraw.EditorScripts
{
    public static class Phase14_3_BuildGameplay
    {
        [MenuItem("PhysicsDraw/Phase 14.3 - Build Gameplay Scene")]
        public static void BuildGameplay()
        {
            if (EditorApplication.isPlaying) return;

            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/Gameplay.unity", OpenSceneMode.Single);

            // Clean up old canvas
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.GetComponent<Canvas>() != null || go.name == "EventSystem" || go.name == "Background")
                    GameObject.DestroyImmediate(go);
            }

            // Background
            GameObject bgObj = new GameObject("Background");
            var sr = bgObj.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/NotebookGrid.png");
            sr.drawMode = SpriteDrawMode.Tiled;
            sr.size = new Vector2(100, 100);
            sr.sortingOrder = -100;
            bgObj.transform.position = Vector3.zero;

            // Canvas
            GameObject canvasObj = new GameObject("GameplayCanvas");
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

            // Gameplay HUD Script
            var hudUI = canvasObj.AddComponent<GameplayUI>();

            // SafeArea Container
            GameObject saObj = new GameObject("SafeArea");
            saObj.transform.SetParent(canvasObj.transform, false);
            var saRt = saObj.AddComponent<RectTransform>();
            saRt.anchorMin = Vector2.zero; saRt.anchorMax = Vector2.one;
            saRt.sizeDelta = Vector2.zero;

            // Top HUD
            GameObject topObj = new GameObject("TopHUD");
            topObj.transform.SetParent(saObj.transform, false);
            var topRt = topObj.AddComponent<RectTransform>();
            topRt.anchorMin = new Vector2(0, 1); topRt.anchorMax = new Vector2(1, 1);
            topRt.pivot = new Vector2(0.5f, 1);
            topRt.anchoredPosition = Vector2.zero;
            topRt.sizeDelta = new Vector2(0, 150);

            // Level Text
            GameObject lvlObj = new GameObject("LevelText");
            lvlObj.transform.SetParent(topObj.transform, false);
            var lvlTxt = lvlObj.AddComponent<Text>();
            lvlTxt.text = "LEVEL 1";
            lvlTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            lvlTxt.fontSize = 80;
            lvlTxt.alignment = TextAnchor.MiddleCenter;
            lvlTxt.color = Color.black;
            var lRt = lvlObj.GetComponent<RectTransform>();
            lRt.anchorMin = Vector2.zero; lRt.anchorMax = Vector2.one;
            lRt.sizeDelta = Vector2.zero;

            // Settings Button
            GameObject setObj = new GameObject("SettingsButton");
            setObj.transform.SetParent(topObj.transform, false);
            var setImg = setObj.AddComponent<Image>();
            setImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/Circle.png");
            setImg.color = new Color(0.2f, 0.2f, 0.2f);
            var setBtn = setObj.AddComponent<Button>();
            var sRt = setObj.GetComponent<RectTransform>();
            sRt.anchorMin = new Vector2(1, 0.5f); sRt.anchorMax = new Vector2(1, 0.5f);
            sRt.pivot = new Vector2(1, 0.5f);
            sRt.anchoredPosition = new Vector2(-40, 0);
            sRt.sizeDelta = new Vector2(100, 100);

            GameObject gearTxtObj = new GameObject("Icon");
            gearTxtObj.transform.SetParent(setObj.transform, false);
            var gearTxt = gearTxtObj.AddComponent<Text>();
            gearTxt.text = "⚙";
            gearTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            gearTxt.alignment = TextAnchor.MiddleCenter;
            gearTxt.fontSize = 70;
            gearTxt.color = Color.white;
            var gearRt = gearTxtObj.GetComponent<RectTransform>();
            gearRt.anchorMin = Vector2.zero; gearRt.anchorMax = Vector2.one; 
            gearRt.sizeDelta = Vector2.zero;

            // Bottom HUD
            GameObject botObj = new GameObject("BottomHUD");
            botObj.transform.SetParent(saObj.transform, false);
            var botRt = botObj.AddComponent<RectTransform>();
            botRt.anchorMin = new Vector2(0, 0); botRt.anchorMax = new Vector2(1, 0);
            botRt.pivot = new Vector2(0.5f, 0);
            botRt.anchoredPosition = Vector2.zero;
            botRt.sizeDelta = new Vector2(0, 200);

            // Undo Button
            var undoBtn = CreateRoundButton(botObj.transform, "UndoButton", "↶", new Vector2(-150, 100), Color.blue);
            var retryBtn = CreateRoundButton(botObj.transform, "RetryButton", "↺", new Vector2(150, 100), new Color(1f, 0.6f, 0f));

            // Level Complete Panel
            BuildLevelCompletePanel(canvasObj.transform);

            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/Gameplay.unity");
            Debug.Log("[Phase 14.3] Gameplay Scene UI Built.");

            // Add the Settings UI from Phase 13 into the new Canvas
            Phase13_AddSettingsUI.BuildSettingsUI();
        }

        private static GameObject CreateRoundButton(Transform parent, string name, string text, Vector2 pos, Color color)
        {
            GameObject btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent, false);
            var img = btnObj.AddComponent<Image>();
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/Circle.png");
            img.color = color;
            btnObj.AddComponent<Button>();

            var rt = btnObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0); rt.anchorMax = new Vector2(0.5f, 0);
            rt.pivot = new Vector2(0.5f, 0);
            rt.anchoredPosition = pos;
            rt.sizeDelta = new Vector2(120, 120);

            GameObject txtObj = new GameObject("Icon");
            txtObj.transform.SetParent(btnObj.transform, false);
            var txt = txtObj.AddComponent<Text>();
            txt.text = text;
            txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txt.fontSize = 80;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;
            var trt = txtObj.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
            trt.sizeDelta = Vector2.zero;

            return btnObj;
        }

        private static void BuildLevelCompletePanel(Transform canvas)
        {
            GameObject panelObj = new GameObject("LevelCompletePanel");
            panelObj.transform.SetParent(canvas, false);
            var pRt = panelObj.AddComponent<RectTransform>();
            pRt.anchorMin = Vector2.zero; pRt.anchorMax = Vector2.one;
            pRt.sizeDelta = Vector2.zero;
            var ui = panelObj.AddComponent<LevelCompleteUI>();

            // Overlay
            GameObject overObj = new GameObject("DarkOverlay");
            overObj.transform.SetParent(panelObj.transform, false);
            var overImg = overObj.AddComponent<Image>();
            overImg.color = new Color(0, 0, 0, 0.8f);
            var oRt = overObj.GetComponent<RectTransform>();
            oRt.anchorMin = Vector2.zero; oRt.anchorMax = Vector2.one;
            oRt.sizeDelta = Vector2.zero;

            // Window
            GameObject winObj = new GameObject("Window");
            winObj.transform.SetParent(panelObj.transform, false);
            var winImg = winObj.AddComponent<Image>();
            winImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/RoundedRect.png");
            winImg.type = Image.Type.Sliced;
            var wRt = winObj.GetComponent<RectTransform>();
            wRt.anchoredPosition = Vector2.zero;
            wRt.sizeDelta = new Vector2(800, 800);

            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(winObj.transform, false);
            var tTxt = titleObj.AddComponent<Text>();
            tTxt.text = "LEVEL COMPLETE!";
            tTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            tTxt.fontSize = 70;
            tTxt.fontStyle = FontStyle.Bold;
            tTxt.alignment = TextAnchor.MiddleCenter;
            tTxt.color = new Color(0.1f, 0.6f, 0.1f);
            var tRt = titleObj.GetComponent<RectTransform>();
            tRt.anchorMin = new Vector2(0, 1); tRt.anchorMax = new Vector2(1, 1);
            tRt.pivot = new Vector2(0.5f, 1);
            tRt.anchoredPosition = new Vector2(0, -100);
            tRt.sizeDelta = new Vector2(0, 150);

            // Next Level Button
            var nextBtn = CreateButton(winObj.transform, "NextButton", "NEXT LEVEL", new Vector2(0, -50), new Vector2(600, 150), new Color(0.2f, 0.8f, 0.2f));
            var menuBtn = CreateButton(winObj.transform, "MenuButton", "MAIN MENU", new Vector2(0, -250), new Vector2(600, 150), new Color(0.2f, 0.6f, 1f));

            panelObj.SetActive(false);
        }

        private static GameObject CreateButton(Transform parent, string name, string text, Vector2 pos, Vector2 size, Color color)
        {
            GameObject btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent, false);
            var img = btnObj.AddComponent<Image>();
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/RoundedRect.png");
            img.type = Image.Type.Sliced;
            img.color = color;
            var btn = btnObj.AddComponent<Button>();

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
