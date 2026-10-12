using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using PhysicsDraw.UI;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase13_AddSettingsUI
    {
        static Phase13_AddSettingsUI()
        {
            if (!SessionState.GetBool("Phase13_Done", false))
            {
                SessionState.SetBool("Phase13_Done", true);
                EditorApplication.delayCall += BuildSettingsUI;
            }
        }

        [MenuItem("PhysicsDraw/Phase 13 - Build Settings UI")]
        public static void BuildSettingsUI()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.LogWarning("[Phase13] Cannot run setup while in Play Mode. Please exit Play Mode and run from menu.");
                return;
            }

            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/Gameplay.unity", OpenSceneMode.Single);
            
            GameObject canvasObj = null;
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.name == "GameplayCanvas")
                {
                    canvasObj = go;
                    break;
                }
            }

            if (canvasObj == null)
            {
                Debug.LogError("[Phase13] GameplayCanvas not found in root objects!");
                return;
            }

            // Remove old settings UI if it exists to avoid duplicates
            var oldUI = canvasObj.transform.Find("GameplaySettingsPanel");
            if (oldUI != null) GameObject.DestroyImmediate(oldUI.gameObject);
            var oldBtn = canvasObj.transform.Find("OpenSettingsButton");
            if (oldBtn != null) GameObject.DestroyImmediate(oldBtn.gameObject);

            // 1. Settings Open Button (Top Right)
            GameObject openBtnObj = new GameObject("OpenSettingsButton");
            openBtnObj.transform.SetParent(canvasObj.transform, false);
            var openImg = openBtnObj.AddComponent<Image>();
            openImg.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            var openBtn = openBtnObj.AddComponent<Button>();
            
            var btnRt = openBtnObj.GetComponent<RectTransform>();
            btnRt.anchorMin = new Vector2(1, 1);
            btnRt.anchorMax = new Vector2(1, 1);
            btnRt.pivot = new Vector2(1, 1);
            btnRt.anchoredPosition = new Vector2(-50, -50);
            btnRt.sizeDelta = new Vector2(120, 120);

            GameObject gearTxtObj = new GameObject("Icon");
            gearTxtObj.transform.SetParent(openBtnObj.transform, false);
            var gearTxt = gearTxtObj.AddComponent<Text>();
            gearTxt.text = "⚙"; // Gear symbol
            gearTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            gearTxt.alignment = TextAnchor.MiddleCenter;
            gearTxt.fontSize = 80;
            gearTxt.color = Color.white;
            var gearRt = gearTxtObj.GetComponent<RectTransform>();
            gearRt.anchorMin = Vector2.zero; gearRt.anchorMax = Vector2.one; 
            gearRt.offsetMin = Vector2.zero; gearRt.offsetMax = Vector2.zero;

            // Level Text Indicator
            var oldLvlTxt = canvasObj.transform.Find("LevelText");
            if (oldLvlTxt != null) GameObject.DestroyImmediate(oldLvlTxt.gameObject);
            GameObject levelTxtObj = new GameObject("LevelText");
            levelTxtObj.transform.SetParent(canvasObj.transform, false);
            var levelTxt = levelTxtObj.AddComponent<Text>();
            levelTxt.text = "LEVEL 1";
            levelTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            levelTxt.alignment = TextAnchor.UpperCenter;
            levelTxt.fontSize = 60;
            levelTxt.color = Color.black;
            var lvlRt = levelTxtObj.GetComponent<RectTransform>();
            lvlRt.anchorMin = new Vector2(0.5f, 1); lvlRt.anchorMax = new Vector2(0.5f, 1);
            lvlRt.pivot = new Vector2(0.5f, 1);
            lvlRt.anchoredPosition = new Vector2(0, -50);
            lvlRt.sizeDelta = new Vector2(400, 100);

            // 2. Settings Popup Panel
            GameObject popupRoot = new GameObject("GameplaySettingsPanel");
            popupRoot.transform.SetParent(canvasObj.transform, false);
            var popupRt = popupRoot.AddComponent<RectTransform>();
            popupRt.anchorMin = Vector2.zero; popupRt.anchorMax = Vector2.one; 
            popupRt.offsetMin = Vector2.zero; popupRt.offsetMax = Vector2.zero;

            GameObject overlayObj = new GameObject("DarkOverlay");
            overlayObj.transform.SetParent(popupRoot.transform, false);
            var overlayImg = overlayObj.AddComponent<Image>();
            overlayImg.color = new Color(0, 0, 0, 0.7f);
            var ovRt = overlayObj.GetComponent<RectTransform>();
            ovRt.anchorMin = Vector2.zero; ovRt.anchorMax = Vector2.one; 
            ovRt.offsetMin = Vector2.zero; ovRt.offsetMax = Vector2.zero;

            GameObject windowObj = new GameObject("Window");
            windowObj.transform.SetParent(popupRoot.transform, false);
            var winImg = windowObj.AddComponent<Image>();
            winImg.color = new Color(0.95f, 0.95f, 0.95f, 1f);
            var winRt = windowObj.GetComponent<RectTransform>();
            winRt.anchoredPosition = Vector2.zero;
            winRt.sizeDelta = new Vector2(800, 600);

            // Title
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(windowObj.transform, false);
            var titleTxt = titleObj.AddComponent<Text>();
            titleTxt.text = "PAUSED";
            titleTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            titleTxt.alignment = TextAnchor.UpperCenter;
            titleTxt.fontSize = 80;
            titleTxt.color = Color.black;
            var tRt = titleObj.GetComponent<RectTransform>();
            tRt.anchorMin = new Vector2(0, 1); tRt.anchorMax = new Vector2(1, 1);
            tRt.pivot = new Vector2(0.5f, 1);
            tRt.anchoredPosition = new Vector2(0, -50);
            tRt.sizeDelta = new Vector2(0, 100);

            // Replay Button
            GameObject replayBtnObj = new GameObject("ReplayButton");
            replayBtnObj.transform.SetParent(windowObj.transform, false);
            var repImg = replayBtnObj.AddComponent<Image>();
            repImg.color = new Color(1f, 0.6f, 0f); // Orange
            var replayBtn = replayBtnObj.AddComponent<Button>();
            var repRt = replayBtnObj.GetComponent<RectTransform>();
            repRt.anchoredPosition = new Vector2(0, 50);
            repRt.sizeDelta = new Vector2(500, 150);
            GameObject repTxtObj = new GameObject("Text");
            repTxtObj.transform.SetParent(replayBtnObj.transform, false);
            var repTxt = repTxtObj.AddComponent<Text>();
            repTxt.text = "REPLAY ↺";
            repTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            repTxt.alignment = TextAnchor.MiddleCenter;
            repTxt.fontSize = 60;
            repTxt.color = Color.white;
            var rTxtRt = repTxtObj.GetComponent<RectTransform>();
            rTxtRt.anchorMin = Vector2.zero; rTxtRt.anchorMax = Vector2.one; 
            rTxtRt.offsetMin = Vector2.zero; rTxtRt.offsetMax = Vector2.zero;

            // Main Menu Button
            GameObject menuBtnObj = new GameObject("MainMenuButton");
            menuBtnObj.transform.SetParent(windowObj.transform, false);
            var menImg = menuBtnObj.AddComponent<Image>();
            menImg.color = new Color(0.2f, 0.6f, 1f); // Blue
            var menuBtn = menuBtnObj.AddComponent<Button>();
            var menRt = menuBtnObj.GetComponent<RectTransform>();
            menRt.anchoredPosition = new Vector2(0, -150);
            menRt.sizeDelta = new Vector2(500, 150);
            GameObject menTxtObj = new GameObject("Text");
            menTxtObj.transform.SetParent(menuBtnObj.transform, false);
            var menTxt = menTxtObj.AddComponent<Text>();
            menTxt.text = "MAIN MENU ⌂";
            menTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            menTxt.alignment = TextAnchor.MiddleCenter;
            menTxt.fontSize = 60;
            menTxt.color = Color.white;
            var mTxtRt = menTxtObj.GetComponent<RectTransform>();
            mTxtRt.anchorMin = Vector2.zero; mTxtRt.anchorMax = Vector2.one; 
            mTxtRt.offsetMin = Vector2.zero; mTxtRt.offsetMax = Vector2.zero;

            // Close Button
            GameObject closeBtnObj = new GameObject("CloseButton");
            closeBtnObj.transform.SetParent(windowObj.transform, false);
            var closeImg = closeBtnObj.AddComponent<Image>();
            closeImg.color = Color.red;
            var closeBtn = closeBtnObj.AddComponent<Button>();
            var cloRt = closeBtnObj.GetComponent<RectTransform>();
            cloRt.anchorMin = new Vector2(1, 1); cloRt.anchorMax = new Vector2(1, 1);
            cloRt.pivot = new Vector2(1, 1);
            cloRt.anchoredPosition = new Vector2(-20, -20);
            cloRt.sizeDelta = new Vector2(80, 80);
            GameObject cloTxtObj = new GameObject("Text");
            cloTxtObj.transform.SetParent(closeBtnObj.transform, false);
            var cloTxt = cloTxtObj.AddComponent<Text>();
            cloTxt.text = "X";
            cloTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            cloTxt.alignment = TextAnchor.MiddleCenter;
            cloTxt.fontSize = 60;
            cloTxt.color = Color.white;
            var cTxtRt = cloTxtObj.GetComponent<RectTransform>();
            cTxtRt.anchorMin = Vector2.zero; cTxtRt.anchorMax = Vector2.one; 
            cTxtRt.offsetMin = Vector2.zero; cTxtRt.offsetMax = Vector2.zero;

            // Wire up script
            var settingsUI = popupRoot.AddComponent<GameplaySettingsUI>();
            settingsUI.popupPanel = popupRoot;
            settingsUI.openSettingsBtn = openBtn;
            settingsUI.closeBtn = closeBtn;
            settingsUI.replayBtn = replayBtn;
            settingsUI.mainMenuBtn = menuBtn;
            settingsUI.darkOverlay = overlayImg;
            settingsUI.popupRect = winRt;

            // Force inactive initially
            popupRoot.SetActive(false);

            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/Gameplay.unity");
            Debug.Log("[Phase13] Added Gameplay Settings UI successfully.");
        }
    }
}
