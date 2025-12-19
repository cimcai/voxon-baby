using UnityEngine;
using Voxon.EyeTracker;
using Voxon.GazeDetection;

namespace Voxon.Utilities
{
    /// <summary>
    /// Debug utility to visualize and diagnose gaze detection issues
    /// </summary>
    public class GazeDebugger : MonoBehaviour
    {
        [Header("Debug Settings")]
        [SerializeField] private bool showGazeRay = true;
        [SerializeField] private bool showGazeHit = true;
        [SerializeField] private bool logGazeData = false;
        [SerializeField] private Color rayColor = Color.green;
        [SerializeField] private Color hitColor = Color.red;
        [SerializeField] private float rayLength = 10f;

        private EyeTrackerManager eyeTrackerManager;
        private GazeRaycast gazeRaycast;
        private LineRenderer lineRenderer;
        private GameObject hitIndicator;

        private void Start()
        {
            eyeTrackerManager = EyeTrackerManager.Instance;
            gazeRaycast = FindObjectOfType<GazeRaycast>();

            if (gazeRaycast == null)
            {
                Debug.LogError("GazeDebugger: GazeRaycast not found! Please add GazeRaycast component to a GameObject.");
            }

            // Create line renderer for gaze ray visualization
            if (showGazeRay)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
                // Use Unlit shader which works better for debug lines
                Shader shader = Shader.Find("Unlit/Color");
                if (shader == null)
                {
                    shader = Shader.Find("Sprites/Default");
                }
                if (shader != null)
                {
                    lineRenderer.material = new Material(shader);
                    lineRenderer.material.color = rayColor;
                }
                lineRenderer.startColor = rayColor;
                lineRenderer.endColor = rayColor;
                lineRenderer.startWidth = 0.05f;
                lineRenderer.endWidth = 0.05f;
                lineRenderer.positionCount = 2;
                lineRenderer.useWorldSpace = true;
                // Make sure it renders in all views
                lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                lineRenderer.receiveShadows = false;
            }

            // Create hit indicator
            if (showGazeHit)
            {
                hitIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                hitIndicator.name = "GazeHitIndicator";
                hitIndicator.transform.localScale = Vector3.one * 0.1f;
                Renderer renderer = hitIndicator.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = hitColor;
                }
                // Remove collider so it doesn't interfere
                Collider col = hitIndicator.GetComponent<Collider>();
                if (col != null)
                {
                    Destroy(col);
                }
                hitIndicator.SetActive(false);
            }

            Debug.Log("GazeDebugger initialized. Check console for diagnostics.");
            PrintDiagnostics();
        }

        private void Update()
        {
            if (eyeTrackerManager == null || !eyeTrackerManager.IsConnected())
            {
                if (lineRenderer != null) lineRenderer.enabled = false;
                if (hitIndicator != null) hitIndicator.SetActive(false);
                return;
            }

            GazeData gazeData = eyeTrackerManager.GetGazeData();
            
            if (!gazeData.isValid)
            {
                if (lineRenderer != null) lineRenderer.enabled = false;
                if (hitIndicator != null) hitIndicator.SetActive(false);
                return;
            }

            // Draw gaze ray
            if (showGazeRay && lineRenderer != null)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, gazeData.gazeOrigin);
                lineRenderer.SetPosition(1, gazeData.gazeOrigin + gazeData.gazeDirection * rayLength);
            }

            // Check for hits
            if (gazeRaycast != null && gazeRaycast.PerformRaycast(out RaycastHit hitInfo))
            {
                if (showGazeHit && hitIndicator != null)
                {
                    hitIndicator.SetActive(true);
                    hitIndicator.transform.position = hitInfo.point;
                }

                if (logGazeData)
                {
                    Debug.Log($"Gaze Hit: {hitInfo.collider.name} at {hitInfo.point}");
                }
            }
            else
            {
                if (hitIndicator != null)
                {
                    hitIndicator.SetActive(false);
                }
            }
        }

        private void PrintDiagnostics()
        {
            Debug.Log("=== Gaze Detection Diagnostics ===");
            
            // Check EyeTrackerManager
            if (eyeTrackerManager == null)
            {
                Debug.LogError("❌ EyeTrackerManager.Instance is null!");
            }
            else
            {
                Debug.Log($"✓ EyeTrackerManager found");
                if (eyeTrackerManager.IsConnected())
                {
                    Debug.Log($"✓ Eye Tracker is connected");
                    GazeData gazeData = eyeTrackerManager.GetGazeData();
                    Debug.Log($"  Gaze Origin: {gazeData.gazeOrigin}");
                    Debug.Log($"  Gaze Direction: {gazeData.gazeDirection}");
                    Debug.Log($"  Is Valid: {gazeData.isValid}");
                }
                else
                {
                    Debug.LogWarning("⚠ Eye Tracker is NOT connected");
                }
            }

            // Check GazeRaycast
            if (gazeRaycast == null)
            {
                Debug.LogError("❌ GazeRaycast component not found!");
                Debug.LogWarning("  → Add GazeRaycast component to a GameObject in the scene");
            }
            else
            {
                Debug.Log($"✓ GazeRaycast found on {gazeRaycast.gameObject.name}");
            }

            // Check GazeHitDetector
            GazeHitDetector hitDetector = FindObjectOfType<GazeHitDetector>();
            if (hitDetector == null)
            {
                Debug.LogError("❌ GazeHitDetector component not found!");
                Debug.LogWarning("  → Add GazeHitDetector component to a GameObject in the scene");
            }
            else
            {
                Debug.Log($"✓ GazeHitDetector found on {hitDetector.gameObject.name}");
            }

            // Check Camera
            UnityEngine.Camera mainCam = UnityEngine.Camera.main;
            if (mainCam == null)
            {
                Debug.LogWarning("⚠ Main Camera not found!");
            }
            else
            {
                Debug.Log($"✓ Main Camera found: {mainCam.name}");
            }

            // Check for volumetric shapes
            VolumetricShapes.VolumetricShape[] shapes = FindObjectsOfType<VolumetricShapes.VolumetricShape>();
            Debug.Log($"Found {shapes.Length} volumetric shape(s) in scene");
            foreach (var shape in shapes)
            {
                Collider col = shape.GetComponent<Collider>();
                if (col == null)
                {
                    Debug.LogWarning($"  ⚠ {shape.name} has no Collider!");
                }
                else
                {
                    Debug.Log($"  ✓ {shape.name} has {col.GetType().Name}");
                }
            }

            Debug.Log("=== End Diagnostics ===");
        }

        private void OnDestroy()
        {
            if (hitIndicator != null)
            {
                Destroy(hitIndicator);
            }
        }
    }
}

