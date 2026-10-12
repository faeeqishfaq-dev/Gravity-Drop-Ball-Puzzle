using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using PhysicsDraw.Core;
using PhysicsDraw.Drawing;
using PhysicsDraw.InputSystem;
using PhysicsDraw.Level;
using PhysicsDraw.UI;
using PhysicsDraw.Audio;
using PhysicsDraw.Save;

namespace PhysicsDraw.EditorScripts
{
    public static class Phase11_FixAndFlowSetup
    {
        [MenuItem("PhysicsDraw/Phase 11 - Build Full Game Flow")]
        public static void BuildFullFlow()
        {
            BuildBootstrapScene();
            BuildMainMenuScene();
            BuildGameplayScene();
            FixBuildSettings();
            Debug.Log("[Phase11] Full Game Flow Built and Saved.");
        }

        private static void BuildBootstrapScene()
        {
            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/Bootstrap.unity", OpenSceneMode.Single);
            
            // Clean up
            foreach (var go in scene.GetRootGameObjects()) { if (go.name != "Main Camera") GameObject.DestroyImmediate(go); }

            // Add CoreManagers
            GameObject core = new GameObject("CoreManagers");
            core.AddComponent<GameManager>();
            core.AddComponent<SaveManager>();
            core.AddComponent<AudioManager>();
            core.AddComponent<GravityManager>();
            core.AddComponent<GameBootstrap>(); // Triggers load to MainMenu

            // UI
            GameObject canvasObj = new GameObject("BootstrapCanvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            
            GameObject textObj = new GameObject("LoadingText");
            textObj.transform.SetParent(canvasObj.transform);
            var text = textObj.AddComponent<UnityEngine.UI.Text>();
            text.text = "PHYSICS DROP\nLoading...";
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 50;
            text.color = Color.black;
            
            var rt = textObj.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(800, 200);

            Camera.main.backgroundColor = Color.white;
            Camera.main.clearFlags = CameraClearFlags.SolidColor;

            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/Bootstrap.unity");
        }

        private static void BuildMainMenuScene()
        {
            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/MainMenu.unity", OpenSceneMode.Single);
            
            foreach (var go in scene.GetRootGameObjects()) { if (go.name != "Main Camera") GameObject.DestroyImmediate(go); }

            Camera.main.backgroundColor = new Color(0.95f, 0.95f, 0.95f); // Off-white
            Camera.main.clearFlags = CameraClearFlags.SolidColor;

            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();

            GameObject canvasObj = new GameObject("MainMenuCanvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

            var mainMenuUI = canvasObj.AddComponent<MainMenuUI>();

            // Title
            GameObject titleObj = new GameObject("TitleText");
            titleObj.transform.SetParent(canvasObj.transform);
            var title = titleObj.AddComponent<UnityEngine.UI.Text>();
            title.text = "PHYSICS PUZZLE";
            title.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            title.alignment = TextAnchor.MiddleCenter;
            title.fontSize = 100;
            title.color = Color.black;
            var trt = titleObj.GetComponent<RectTransform>();
            trt.anchoredPosition = new Vector2(0, 400);
            trt.sizeDelta = new Vector2(1000, 300);

            // Play Button
            GameObject btnObj = new GameObject("PlayButton");
            btnObj.transform.SetParent(canvasObj.transform);
            var img = btnObj.AddComponent<UnityEngine.UI.Image>();
            img.color = Color.green;
            var btn = btnObj.AddComponent<UnityEngine.UI.Button>();
            
            GameObject btnTextObj = new GameObject("Text");
            btnTextObj.transform.SetParent(btnObj.transform);
            var btnText = btnTextObj.AddComponent<UnityEngine.UI.Text>();
            btnText.text = "PLAY";
            btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            btnText.alignment = TextAnchor.MiddleCenter;
            btnText.fontSize = 60;
            btnText.color = Color.white;
            var btntrt = btnTextObj.GetComponent<RectTransform>();
            btntrt.anchorMin = Vector2.zero; btntrt.anchorMax = Vector2.one; btntrt.offsetMin = Vector2.zero; btntrt.offsetMax = Vector2.zero;

            var brt = btnObj.GetComponent<RectTransform>();
            brt.anchoredPosition = new Vector2(0, 0);
            brt.sizeDelta = new Vector2(500, 150);

            // Wire button
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, mainMenuUI.PlayGame);

            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/MainMenu.unity");
        }

        private static void BuildGameplayScene()
        {
            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/Gameplay.unity", OpenSceneMode.Single);
            
            foreach (var go in scene.GetRootGameObjects()) { if (go.name != "Main Camera") GameObject.DestroyImmediate(go); }

            Camera.main.backgroundColor = new Color(0.9f, 0.9f, 0.9f);
            Camera.main.clearFlags = CameraClearFlags.SolidColor;

            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();

            GameObject gameplayManagers = new GameObject("GameplayManagers");
            gameplayManagers.AddComponent<InputProvider>();
            gameplayManagers.AddComponent<DrawingController>();
            gameplayManagers.AddComponent<LevelManager>();
            gameplayManagers.AddComponent<PhysicsDraw.Scripts.Core.GameplayBootstrapper>();

            // UI
            GameObject canvasObj = new GameObject("GameplayCanvas");
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

            var gameplayUI = canvasObj.AddComponent<GameplayUI>();

            // Undo Button
            GameObject undoObj = new GameObject("UndoButton");
            undoObj.transform.SetParent(canvasObj.transform);
            var undoImg = undoObj.AddComponent<UnityEngine.UI.Image>();
            undoImg.color = Color.gray;
            var undoBtn = undoObj.AddComponent<UnityEngine.UI.Button>();
            
            GameObject undoTextObj = new GameObject("Text");
            undoTextObj.transform.SetParent(undoObj.transform);
            var undoText = undoTextObj.AddComponent<UnityEngine.UI.Text>();
            undoText.text = "UNDO";
            undoText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            undoText.alignment = TextAnchor.MiddleCenter;
            undoText.fontSize = 40;
            undoText.color = Color.white;
            var urt = undoTextObj.GetComponent<RectTransform>();
            urt.anchorMin = Vector2.zero; urt.anchorMax = Vector2.one; urt.offsetMin = Vector2.zero; urt.offsetMax = Vector2.zero;

            var ubrt = undoObj.GetComponent<RectTransform>();
            ubrt.anchorMin = new Vector2(0, 1);
            ubrt.anchorMax = new Vector2(0, 1);
            ubrt.pivot = new Vector2(0, 1);
            ubrt.anchoredPosition = new Vector2(50, -50);
            ubrt.sizeDelta = new Vector2(200, 100);

            // Retry Button
            GameObject retryObj = new GameObject("RetryButton");
            retryObj.transform.SetParent(canvasObj.transform);
            var retryImg = retryObj.AddComponent<UnityEngine.UI.Image>();
            retryImg.color = Color.red;
            var retryBtn = retryObj.AddComponent<UnityEngine.UI.Button>();
            
            GameObject retryTextObj = new GameObject("Text");
            retryTextObj.transform.SetParent(retryObj.transform);
            var retryText = retryTextObj.AddComponent<UnityEngine.UI.Text>();
            retryText.text = "RETRY";
            retryText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            retryText.alignment = TextAnchor.MiddleCenter;
            retryText.fontSize = 40;
            retryText.color = Color.white;
            var rtrt = retryTextObj.GetComponent<RectTransform>();
            rtrt.anchorMin = Vector2.zero; rtrt.anchorMax = Vector2.one; rtrt.offsetMin = Vector2.zero; rtrt.offsetMax = Vector2.zero;

            var rbrt = retryObj.GetComponent<RectTransform>();
            rbrt.anchorMin = new Vector2(1, 1);
            rbrt.anchorMax = new Vector2(1, 1);
            rbrt.pivot = new Vector2(1, 1);
            rbrt.anchoredPosition = new Vector2(-50, -50);
            rbrt.sizeDelta = new Vector2(200, 100);

            // Next Level Panel
            GameObject successObj = new GameObject("SuccessPanel");
            successObj.transform.SetParent(canvasObj.transform);
            var successImg = successObj.AddComponent<UnityEngine.UI.Image>();
            successImg.color = new Color(0, 0, 0, 0.8f);
            var srt = successObj.GetComponent<RectTransform>();
            srt.anchorMin = Vector2.zero; srt.anchorMax = Vector2.one; srt.offsetMin = Vector2.zero; srt.offsetMax = Vector2.zero;
            
            GameObject nextBtnObj = new GameObject("NextButton");
            nextBtnObj.transform.SetParent(successObj.transform);
            var nextImg = nextBtnObj.AddComponent<UnityEngine.UI.Image>();
            nextImg.color = Color.green;
            var nextBtn = nextBtnObj.AddComponent<UnityEngine.UI.Button>();
            
            GameObject nextTextObj = new GameObject("Text");
            nextTextObj.transform.SetParent(nextBtnObj.transform);
            var nextText = nextTextObj.AddComponent<UnityEngine.UI.Text>();
            nextText.text = "NEXT LEVEL";
            nextText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            nextText.alignment = TextAnchor.MiddleCenter;
            nextText.fontSize = 60;
            nextText.color = Color.white;
            var ntrt = nextTextObj.GetComponent<RectTransform>();
            ntrt.anchorMin = Vector2.zero; ntrt.anchorMax = Vector2.one; ntrt.offsetMin = Vector2.zero; ntrt.offsetMax = Vector2.zero;

            var nbrt = nextBtnObj.GetComponent<RectTransform>();
            nbrt.anchoredPosition = new Vector2(0, 0);
            nbrt.sizeDelta = new Vector2(500, 150);

            var lcUI = successObj.AddComponent<LevelCompleteUI>();
            
            // Note: the reflection wiring of private fields via script can be complex, so we will assign via code if possible, or just use Find in LevelCompleteUI.
            
            successObj.SetActive(false);

            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/Gameplay.unity");
        }

        private static void FixBuildSettings()
        {
            var scenes = new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene("Assets/_Game/Scenes/Bootstrap.unity", true),
                new EditorBuildSettingsScene("Assets/_Game/Scenes/MainMenu.unity", true),
                new EditorBuildSettingsScene("Assets/_Game/Scenes/Gameplay.unity", true)
            };
            EditorBuildSettings.scenes = scenes;
        }
    }
}
