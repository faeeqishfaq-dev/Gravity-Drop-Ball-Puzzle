using System.Collections.Generic;
using UnityEngine;
using PhysicsDraw.InputSystem;
using PhysicsDraw.Core;
using PhysicsDraw.Level;

namespace PhysicsDraw.Drawing
{
    public class DrawingController : MonoBehaviour, IResettable
    {
        [Header("References")]
        [SerializeField] private Material drawingMaterial;
        [SerializeField] private PhysicsMaterial2D drawingPhysicsMaterial;

        private StrokeBuilder currentBuilder;
        private DrawnPhysicsObject currentDrawingPreview;
        private Camera mainCam;

        private List<GameObject> activeDrawings = new List<GameObject>();
        private Core.PhysicsSettings settings;

        private void Start()
        {
            mainCam = Camera.main;
            settings = Resources.Load<Core.PhysicsSettings>("Settings/PhysicsSettings");

            if (settings == null)
            {
                Debug.LogError("[DrawingController] Missing PhysicsSettings in Resources!");
            }

            if (InputProvider.Instance != null)
            {
                InputProvider.Instance.OnPointerDown += HandlePointerDown;
                InputProvider.Instance.OnPointerMove += HandlePointerMove;
                InputProvider.Instance.OnPointerUp += HandlePointerUp;
            }

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.OnLevelLoaded += ClearAllDrawings;
                LevelManager.Instance.OnLevelReset += ClearAllDrawings;
            }
        }

        private void OnDestroy()
        {
            if (InputProvider.Instance != null)
            {
                InputProvider.Instance.OnPointerDown -= HandlePointerDown;
                InputProvider.Instance.OnPointerMove -= HandlePointerMove;
                InputProvider.Instance.OnPointerUp -= HandlePointerUp;
            }

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.OnLevelLoaded -= ClearAllDrawings;
                LevelManager.Instance.OnLevelReset -= ClearAllDrawings;
            }
        }

        private void HandlePointerDown(Vector2 screenPos)
        {
            var state = GameManager.Instance.CurrentState;
            if (state != GameState.Playing && state != GameState.Ready) return;

            if (settings == null) return;

            if (mainCam == null) mainCam = Camera.main;
            if (mainCam == null) return;

            Vector2 worldPos = mainCam.ScreenToWorldPoint(screenPos);
            
            currentBuilder = new StrokeBuilder(settings.minPointDistance);
            currentBuilder.StartStroke(worldPos);

            CreatePreviewObject();
            var mat = drawingPhysicsMaterial != null ? drawingPhysicsMaterial : Resources.Load<PhysicsMaterial2D>("PhysicsMaterials/Drawing");
            currentDrawingPreview.InitializePreview(currentBuilder.Points, settings.drawingWidth, mat);
        }

        private void HandlePointerMove(Vector2 screenPos)
        {
            var state = GameManager.Instance.CurrentState;
            if (state != GameState.Playing && state != GameState.Ready)
            {
                if (currentDrawingPreview != null) Destroy(currentDrawingPreview.gameObject);
                currentBuilder = null;
                currentDrawingPreview = null;
                return;
            }

            if (currentBuilder == null || currentDrawingPreview == null) return;

            if (mainCam == null) mainCam = Camera.main;
            if (mainCam == null) return;

            Vector2 worldPos = mainCam.ScreenToWorldPoint(screenPos);
            
            if (currentBuilder.AddPoint(worldPos))
            {
                currentDrawingPreview.UpdatePreview(currentBuilder.Points);
            }
        }

        private void HandlePointerUp(Vector2 screenPos)
        {
            var state = GameManager.Instance.CurrentState;
            if (state != GameState.Playing && state != GameState.Ready)
            {
                if (currentDrawingPreview != null) Destroy(currentDrawingPreview.gameObject);
                currentBuilder = null;
                currentDrawingPreview = null;
                return;
            }

            if (currentBuilder == null || currentDrawingPreview == null) return;

            if (mainCam == null) mainCam = Camera.main;
            if (mainCam == null) return;

            Vector2 worldPos = mainCam.ScreenToWorldPoint(screenPos);
            currentBuilder.AddPoint(worldPos); // Add final point
            
            var simplifiedPoints = currentBuilder.SimplifyStroke(settings.simplificationTolerance);

            if (simplifiedPoints.Count >= 2)
            {
                currentDrawingPreview.FinalizePhysics(
                    simplifiedPoints, 
                    settings.massPerUnitLength, 
                    settings.minimumDrawingMass, 
                    settings.maximumDrawingMass, 
                    settings.drawingGravityScale
                );

                activeDrawings.Add(currentDrawingPreview.gameObject);
                
                if (GameManager.Instance.CurrentState == GameState.Ready)
                {
                    GameManager.Instance.ChangeState(GameState.Playing);
                }
            }
            else
            {
                Destroy(currentDrawingPreview.gameObject);
            }

            currentBuilder = null;
            currentDrawingPreview = null;
        }

        private void CreatePreviewObject()
        {
            // Requirement: "All player-created drawings should be parented under a dedicated runtime container"
            GameObject drawingRoot = GameObject.Find("DrawingRoot");
            if (drawingRoot == null)
            {
                drawingRoot = new GameObject("DrawingRoot");
            }

            GameObject obj = new GameObject("PlayerDrawing");
            obj.transform.SetParent(drawingRoot.transform);
            obj.layer = LayerMask.NameToLayer("Drawing");
            
            var lr = obj.AddComponent<LineRenderer>();
            
            var mat = Resources.Load<Material>("Materials/DrawingMaterial");
            if (mat == null)
            {
                mat = new Material(Shader.Find("Sprites/Default"));
            }
            lr.material = mat;
            lr.startColor = Color.blue;
            lr.endColor = Color.blue;
            lr.numCapVertices = 5;
            lr.numCornerVertices = 5;
            lr.useWorldSpace = false; // MUST be false so line moves with Rigidbody!
            lr.sortingOrder = 30; // 30 is Drawing layer

            currentDrawingPreview = obj.AddComponent<DrawnPhysicsObject>();
        }

        public void UndoLastDrawing()
        {
            if (activeDrawings.Count > 0)
            {
                GameObject lastObj = activeDrawings[activeDrawings.Count - 1];
                activeDrawings.RemoveAt(activeDrawings.Count - 1);
                if (lastObj != null)
                {
                    Destroy(lastObj);
                }
            }
        }

        public void ResetObject()
        {
            ClearAllDrawings();
        }

        public void ClearAllDrawings()
        {
            if (currentDrawingPreview != null)
            {
                Destroy(currentDrawingPreview.gameObject);
                currentDrawingPreview = null;
            }
            currentBuilder = null;

            foreach (var drawing in activeDrawings)
            {
                if (drawing != null)
                {
                    drawing.SetActive(false);
                    Destroy(drawing);
                }
            }
            activeDrawings.Clear();
            
            GameObject drawingRoot = GameObject.Find("DrawingRoot");
            if (drawingRoot != null)
            {
                for (int i = drawingRoot.transform.childCount - 1; i >= 0; i--)
                {
                    var child = drawingRoot.transform.GetChild(i).gameObject;
                    child.SetActive(false);
                    Destroy(child);
                }
            }
        }
    }
}
