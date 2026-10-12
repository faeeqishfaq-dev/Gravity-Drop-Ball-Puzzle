using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace PhysicsDraw.EditorScripts
{
    public static class Phase14_6_BuildLevelSelect
    {
        public static void BuildLevelSelect()
        {
            // 1. Create LevelButton Prefab
            GameObject btnObj = new GameObject("LevelButton");
            var btnRt = btnObj.AddComponent<RectTransform>();
            btnRt.sizeDelta = new Vector2(200, 200);

            var img = btnObj.AddComponent<Image>();
            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/RoundedRect.png");
            img.type = Image.Type.Sliced;
            img.color = new Color(0.2f, 0.6f, 1f); // Blue
            btnObj.AddComponent<Button>();

            // Number Text
            GameObject txtObj = new GameObject("NumberText");
            txtObj.transform.SetParent(btnObj.transform, false);
            var txt = txtObj.AddComponent<Text>();
            txt.text = "1";
            txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txt.fontSize = 80;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;
            var trt = txtObj.GetComponent<RectTransform>();
            trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
            trt.sizeDelta = Vector2.zero;

            // Lock Icon
            GameObject lockObj = new GameObject("LockIcon");
            lockObj.transform.SetParent(btnObj.transform, false);
            var lTxt = lockObj.AddComponent<Text>();
            lTxt.text = "🔒";
            lTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            lTxt.fontSize = 100;
            lTxt.alignment = TextAnchor.MiddleCenter;
            lTxt.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            var lrt = lockObj.GetComponent<RectTransform>();
            lrt.anchorMin = Vector2.zero; lrt.anchorMax = Vector2.one;
            lrt.sizeDelta = Vector2.zero;

            // Save Prefab
            if (!AssetDatabase.IsValidFolder("Assets/_Game/Prefabs/UI"))
            {
                AssetDatabase.CreateFolder("Assets/_Game/Prefabs", "UI");
            }
            string prefabPath = "Assets/_Game/Prefabs/UI/LevelButton.prefab";
            PrefabUtility.SaveAsPrefabAsset(btnObj, prefabPath);
            GameObject.DestroyImmediate(btnObj);

            // 2. Add ScrollView to MainMenu -> LevelSelectPanel
            var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/_Game/Scenes/MainMenu.unity", UnityEditor.SceneManagement.OpenSceneMode.Single);
            
            GameObject lvlPanel = null;
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.name == "MainMenuCanvas")
                {
                    var t = go.transform.Find("LevelSelectPanel");
                    if (t != null) lvlPanel = t.gameObject;
                    break;
                }
            }

            if (lvlPanel != null)
            {
                // Remove existing scroll view if any
                var existingScroll = lvlPanel.transform.Find("Scroll View");
                if (existingScroll != null) GameObject.DestroyImmediate(existingScroll.gameObject);

                // Create Scroll View
                GameObject scrollObj = new GameObject("Scroll View");
                scrollObj.transform.SetParent(lvlPanel.transform, false);
                var scrollRt = scrollObj.AddComponent<RectTransform>();
                scrollRt.anchorMin = new Vector2(0, 0); scrollRt.anchorMax = new Vector2(1, 1);
                scrollRt.offsetMin = new Vector2(50, 300); // Pad bottom for back button
                scrollRt.offsetMax = new Vector2(-50, -300); // Pad top for title
                
                var scrollRect = scrollObj.AddComponent<ScrollRect>();
                scrollRect.horizontal = false;
                scrollRect.vertical = true;

                GameObject viewport = new GameObject("Viewport");
                viewport.transform.SetParent(scrollObj.transform, false);
                var vpRt = viewport.AddComponent<RectTransform>();
                vpRt.anchorMin = Vector2.zero; vpRt.anchorMax = Vector2.one;
                vpRt.sizeDelta = Vector2.zero;
                viewport.AddComponent<RectMask2D>();
                scrollRect.viewport = vpRt;

                GameObject content = new GameObject("Content");
                content.transform.SetParent(viewport.transform, false);
                var contentRt = content.AddComponent<RectTransform>();
                contentRt.anchorMin = new Vector2(0, 1); contentRt.anchorMax = new Vector2(1, 1);
                contentRt.pivot = new Vector2(0.5f, 1);
                contentRt.sizeDelta = new Vector2(0, 2000); // Enough for many levels
                scrollRect.content = contentRt;

                var grid = content.AddComponent<GridLayoutGroup>();
                grid.cellSize = new Vector2(250, 250);
                grid.spacing = new Vector2(50, 50);
                grid.padding = new RectOffset(50, 50, 50, 50);
                grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
                grid.startAxis = GridLayoutGroup.Axis.Horizontal;
                grid.childAlignment = TextAnchor.UpperCenter;
                grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                grid.constraintCount = 3;

                // Content Size Fitter
                var csf = content.AddComponent<ContentSizeFitter>();
                csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                csf.verticalFit = ContentSizeFitter.FitMode.MinSize;

                // Re-wire UI script assuming LevelSelectUI is on lvlPanel
                var lsUI = lvlPanel.GetComponent<PhysicsDraw.UI.LevelSelectUI>();
                if (lsUI != null)
                {
                    lsUI.buttonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/UI/LevelButton.prefab");
                    lsUI.contentPanel = content.transform;
                }

                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/MainMenu.unity");
                Debug.Log("[Phase 14.6] Level Selection Grid Built.");
            }
        }
    }
}
