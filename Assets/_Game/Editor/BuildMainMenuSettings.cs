using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace PhysicsDraw.EditorScripts
{
    public class BuildMainMenuSettings : EditorWindow
    {
        [MenuItem("PhysicsDraw/Build MainMenu Settings UI")]
        public static void BuildUI()
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning("Cannot build UI in Play Mode. Please exit Play Mode first.");
                return;
            }

            Scene scene = SceneManager.GetActiveScene();
            if (scene.name != "MainMenu")
            {
                Debug.LogError("Please open MainMenu scene first!");
                return;
            }

            GameObject canvas = GameObject.Find("MainMenuCanvas");
            if (canvas == null)
            {
                Debug.LogError("MainMenuCanvas not found!");
                return;
            }

            // Check if already exists
            Transform existing = canvas.transform.Find("SettingsPopup");
            if (existing != null)
            {
                Undo.DestroyObjectImmediate(existing.gameObject);
            }

            // Font
            Font arial = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // --- Root ---
            GameObject root = new GameObject("SettingsPopup");
            root.transform.SetParent(canvas.transform, false);
            var rootRect = root.AddComponent<RectTransform>();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = Vector2.zero;
            rootRect.offsetMax = Vector2.zero;

            var uiScript = root.AddComponent<PhysicsDraw.UI.MainMenuSettingsUI>();

            // --- Dark Overlay ---
            GameObject overlay = new GameObject("DarkOverlay");
            overlay.transform.SetParent(root.transform, false);
            var overlayRect = overlay.AddComponent<RectTransform>();
            overlayRect.anchorMin = Vector2.zero;
            overlayRect.anchorMax = Vector2.one;
            overlayRect.offsetMin = Vector2.zero;
            overlayRect.offsetMax = Vector2.zero;
            var overlayImg = overlay.AddComponent<Image>();
            overlayImg.color = new Color(0, 0, 0, 0.8f);
            var overlayCG = overlay.AddComponent<CanvasGroup>();
            // Block raycasts so background buttons are not clickable
            overlayCG.blocksRaycasts = true;

            // --- Main Panel ---
            GameObject panel = new GameObject("Panel");
            panel.transform.SetParent(root.transform, false);
            var panelRect = panel.AddComponent<RectTransform>();
            panelRect.sizeDelta = new Vector2(600, 400);
            var panelImg = panel.AddComponent<Image>();
            panelImg.color = new Color(0.15f, 0.15f, 0.2f, 1f);

            // Settings Title
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(panel.transform, false);
            var titleRect = titleObj.AddComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 1f);
            titleRect.anchorMax = new Vector2(0.5f, 1f);
            titleRect.pivot = new Vector2(0.5f, 1f);
            titleRect.anchoredPosition = new Vector2(0, -20);
            titleRect.sizeDelta = new Vector2(400, 60);
            var titleText = titleObj.AddComponent<Text>();
            titleText.font = arial;
            titleText.text = "SETTINGS";
            titleText.fontSize = 40;
            titleText.fontStyle = FontStyle.Bold;
            titleText.alignment = TextAnchor.MiddleCenter;
            titleText.color = Color.white;

            // Close Button
            GameObject closeBtnObj = new GameObject("CloseButton");
            closeBtnObj.transform.SetParent(panel.transform, false);
            var closeBtnRect = closeBtnObj.AddComponent<RectTransform>();
            closeBtnRect.anchorMin = new Vector2(1f, 1f);
            closeBtnRect.anchorMax = new Vector2(1f, 1f);
            closeBtnRect.pivot = new Vector2(1f, 1f);
            closeBtnRect.anchoredPosition = new Vector2(-20, -20);
            closeBtnRect.sizeDelta = new Vector2(50, 50);
            var closeBtnImg = closeBtnObj.AddComponent<Image>();
            closeBtnImg.color = new Color(0.8f, 0.2f, 0.2f, 1f);
            var closeBtn = closeBtnObj.AddComponent<Button>();

            GameObject closeTextObj = new GameObject("Text");
            closeTextObj.transform.SetParent(closeBtnObj.transform, false);
            var closeTextRect = closeTextObj.AddComponent<RectTransform>();
            closeTextRect.anchorMin = Vector2.zero;
            closeTextRect.anchorMax = Vector2.one;
            closeTextRect.offsetMin = Vector2.zero;
            closeTextRect.offsetMax = Vector2.zero;
            var closeText = closeTextObj.AddComponent<Text>();
            closeText.font = arial;
            closeText.text = "X";
            closeText.fontSize = 30;
            closeText.fontStyle = FontStyle.Bold;
            closeText.alignment = TextAnchor.MiddleCenter;
            closeText.color = Color.white;

            // Sound Button
            GameObject soundBtnObj = new GameObject("SoundButton");
            soundBtnObj.transform.SetParent(panel.transform, false);
            var soundBtnRect = soundBtnObj.AddComponent<RectTransform>();
            soundBtnRect.anchoredPosition = new Vector2(0, 40);
            soundBtnRect.sizeDelta = new Vector2(300, 80);
            var soundBtnImg = soundBtnObj.AddComponent<Image>();
            soundBtnImg.color = new Color(0.2f, 0.6f, 0.2f, 1f);
            var soundBtn = soundBtnObj.AddComponent<Button>();

            // Sound ON Image
            GameObject soundOnObj = new GameObject("SoundOnImage");
            soundOnObj.transform.SetParent(soundBtnObj.transform, false);
            var soundOnRect = soundOnObj.AddComponent<RectTransform>();
            soundOnRect.anchorMin = Vector2.zero;
            soundOnRect.anchorMax = Vector2.one;
            soundOnRect.offsetMin = new Vector2(10, 10);
            soundOnRect.offsetMax = new Vector2(-10, -10);
            var soundOnImg = soundOnObj.AddComponent<Image>();
            soundOnImg.color = new Color(0.2f, 1f, 0.2f, 1f); // Bright green placeholder
            
            GameObject soundOnTextObj = new GameObject("Text");
            soundOnTextObj.transform.SetParent(soundOnObj.transform, false);
            var soundOnTextRect = soundOnTextObj.AddComponent<RectTransform>();
            soundOnTextRect.anchorMin = Vector2.zero;
            soundOnTextRect.anchorMax = Vector2.one;
            soundOnTextRect.offsetMin = Vector2.zero;
            soundOnTextRect.offsetMax = Vector2.zero;
            var soundOnText = soundOnTextObj.AddComponent<Text>();
            soundOnText.font = arial;
            soundOnText.text = "SOUND ON";
            soundOnText.fontSize = 25;
            soundOnText.fontStyle = FontStyle.Bold;
            soundOnText.alignment = TextAnchor.MiddleCenter;
            soundOnText.color = Color.white;

            // Sound OFF Image
            GameObject soundOffObj = new GameObject("SoundOffImage");
            soundOffObj.transform.SetParent(soundBtnObj.transform, false);
            var soundOffRect = soundOffObj.AddComponent<RectTransform>();
            soundOffRect.anchorMin = Vector2.zero;
            soundOffRect.anchorMax = Vector2.one;
            soundOffRect.offsetMin = new Vector2(10, 10);
            soundOffRect.offsetMax = new Vector2(-10, -10);
            var soundOffImg = soundOffObj.AddComponent<Image>();
            soundOffImg.color = new Color(1f, 0.2f, 0.2f, 1f); // Bright red placeholder
            
            GameObject soundOffTextObj = new GameObject("Text");
            soundOffTextObj.transform.SetParent(soundOffObj.transform, false);
            var soundOffTextRect = soundOffTextObj.AddComponent<RectTransform>();
            soundOffTextRect.anchorMin = Vector2.zero;
            soundOffTextRect.anchorMax = Vector2.one;
            soundOffTextRect.offsetMin = Vector2.zero;
            soundOffTextRect.offsetMax = Vector2.zero;
            var soundOffText = soundOffTextObj.AddComponent<Text>();
            soundOffText.font = arial;
            soundOffText.text = "SOUND OFF";
            soundOffText.fontSize = 25;
            soundOffText.fontStyle = FontStyle.Bold;
            soundOffText.alignment = TextAnchor.MiddleCenter;
            soundOffText.color = Color.white;

            // Quit Button
            GameObject quitBtnObj = new GameObject("QuitButton");
            quitBtnObj.transform.SetParent(panel.transform, false);
            var quitBtnRect = quitBtnObj.AddComponent<RectTransform>();
            quitBtnRect.anchoredPosition = new Vector2(0, -70);
            quitBtnRect.sizeDelta = new Vector2(300, 80);
            var quitBtnImg = quitBtnObj.AddComponent<Image>();
            quitBtnImg.color = new Color(0.8f, 0.3f, 0.3f, 1f);
            var quitBtn = quitBtnObj.AddComponent<Button>();

            GameObject quitTextObj = new GameObject("Text");
            quitTextObj.transform.SetParent(quitBtnObj.transform, false);
            var quitTextRect = quitTextObj.AddComponent<RectTransform>();
            quitTextRect.anchorMin = Vector2.zero;
            quitTextRect.anchorMax = Vector2.one;
            quitTextRect.offsetMin = Vector2.zero;
            quitTextRect.offsetMax = Vector2.zero;
            var quitText = quitTextObj.AddComponent<Text>();
            quitText.font = arial;
            quitText.text = "QUIT GAME";
            quitText.fontSize = 30;
            quitText.fontStyle = FontStyle.Bold;
            quitText.alignment = TextAnchor.MiddleCenter;
            quitText.color = Color.white;

            // --- Quit Confirmation Popup ---
            GameObject quitConfRoot = new GameObject("QuitConfirmation");
            quitConfRoot.transform.SetParent(canvas.transform, false);
            var quitConfRootRect = quitConfRoot.AddComponent<RectTransform>();
            quitConfRootRect.anchorMin = Vector2.zero;
            quitConfRootRect.anchorMax = Vector2.one;
            quitConfRootRect.offsetMin = Vector2.zero;
            quitConfRootRect.offsetMax = Vector2.zero;

            GameObject quitOverlay = new GameObject("QuitOverlay");
            quitOverlay.transform.SetParent(quitConfRoot.transform, false);
            var quitOverlayRect = quitOverlay.AddComponent<RectTransform>();
            quitOverlayRect.anchorMin = Vector2.zero;
            quitOverlayRect.anchorMax = Vector2.one;
            quitOverlayRect.offsetMin = Vector2.zero;
            quitOverlayRect.offsetMax = Vector2.zero;
            var quitOverlayImg = quitOverlay.AddComponent<Image>();
            quitOverlayImg.color = new Color(0, 0, 0, 0.9f);
            var quitOverlayCG = quitOverlay.AddComponent<CanvasGroup>();
            quitOverlayCG.blocksRaycasts = true;

            GameObject quitPanel = new GameObject("QuitPanel");
            quitPanel.transform.SetParent(quitConfRoot.transform, false);
            var quitPanelRect = quitPanel.AddComponent<RectTransform>();
            quitPanelRect.sizeDelta = new Vector2(500, 300);
            var quitPanelImg = quitPanel.AddComponent<Image>();
            quitPanelImg.color = new Color(0.2f, 0.2f, 0.25f, 1f);

            GameObject qTitleObj = new GameObject("Title");
            qTitleObj.transform.SetParent(quitPanel.transform, false);
            var qTitleRect = qTitleObj.AddComponent<RectTransform>();
            qTitleRect.anchoredPosition = new Vector2(0, 50);
            qTitleRect.sizeDelta = new Vector2(400, 60);
            var qTitleText = qTitleObj.AddComponent<Text>();
            qTitleText.font = arial;
            qTitleText.text = "Quit Game?";
            qTitleText.fontSize = 40;
            qTitleText.fontStyle = FontStyle.Bold;
            qTitleText.alignment = TextAnchor.MiddleCenter;
            qTitleText.color = Color.white;

            // Yes Button
            GameObject yesBtnObj = new GameObject("YesButton");
            yesBtnObj.transform.SetParent(quitPanel.transform, false);
            var yesBtnRect = yesBtnObj.AddComponent<RectTransform>();
            yesBtnRect.anchoredPosition = new Vector2(-120, -50);
            yesBtnRect.sizeDelta = new Vector2(160, 70);
            var yesBtnImg = yesBtnObj.AddComponent<Image>();
            yesBtnImg.color = new Color(0.8f, 0.2f, 0.2f, 1f);
            var yesBtn = yesBtnObj.AddComponent<Button>();

            GameObject yesTextObj = new GameObject("Text");
            yesTextObj.transform.SetParent(yesBtnObj.transform, false);
            var yesTextRect = yesTextObj.AddComponent<RectTransform>();
            yesTextRect.anchorMin = Vector2.zero;
            yesTextRect.anchorMax = Vector2.one;
            yesTextRect.offsetMin = Vector2.zero;
            yesTextRect.offsetMax = Vector2.zero;
            var yesText = yesTextObj.AddComponent<Text>();
            yesText.font = arial;
            yesText.text = "YES";
            yesText.fontSize = 30;
            yesText.fontStyle = FontStyle.Bold;
            yesText.alignment = TextAnchor.MiddleCenter;
            yesText.color = Color.white;

            // No Button
            GameObject noBtnObj = new GameObject("NoButton");
            noBtnObj.transform.SetParent(quitPanel.transform, false);
            var noBtnRect = noBtnObj.AddComponent<RectTransform>();
            noBtnRect.anchoredPosition = new Vector2(120, -50);
            noBtnRect.sizeDelta = new Vector2(160, 70);
            var noBtnImg = noBtnObj.AddComponent<Image>();
            noBtnImg.color = new Color(0.4f, 0.4f, 0.4f, 1f);
            var noBtn = noBtnObj.AddComponent<Button>();

            GameObject noTextObj = new GameObject("Text");
            noTextObj.transform.SetParent(noBtnObj.transform, false);
            var noTextRect = noTextObj.AddComponent<RectTransform>();
            noTextRect.anchorMin = Vector2.zero;
            noTextRect.anchorMax = Vector2.one;
            noTextRect.offsetMin = Vector2.zero;
            noTextRect.offsetMax = Vector2.zero;
            var noText = noTextObj.AddComponent<Text>();
            noText.font = arial;
            noText.text = "NO";
            noText.fontSize = 30;
            noText.fontStyle = FontStyle.Bold;
            noText.alignment = TextAnchor.MiddleCenter;
            noText.color = Color.white;

            // --- Wire up references via SerializedObject ---
            SerializedObject so = new SerializedObject(uiScript);
            so.FindProperty("settingsPanel").objectReferenceValue = root;
            so.FindProperty("backgroundOverlay").objectReferenceValue = overlayCG;
            so.FindProperty("panelRect").objectReferenceValue = panelRect;
            so.FindProperty("soundButton").objectReferenceValue = soundBtn;
            so.FindProperty("soundOnImage").objectReferenceValue = soundOnObj;
            so.FindProperty("soundOffImage").objectReferenceValue = soundOffObj;
            so.FindProperty("quitButton").objectReferenceValue = quitBtn;
            so.FindProperty("closeButton").objectReferenceValue = closeBtn;
            
            so.FindProperty("quitConfirmPanel").objectReferenceValue = quitConfRoot;
            so.FindProperty("quitBackgroundOverlay").objectReferenceValue = quitOverlayCG;
            so.FindProperty("quitPanelRect").objectReferenceValue = quitPanelRect;
            so.FindProperty("quitYesButton").objectReferenceValue = yesBtn;
            so.FindProperty("quitNoButton").objectReferenceValue = noBtn;
            so.ApplyModifiedProperties();

            // Set initial state
            root.SetActive(false);
            quitConfRoot.SetActive(false);

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            
            Debug.Log("MainMenu Settings UI built and saved successfully!");
        }
    }
}
