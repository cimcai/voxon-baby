using UnityEngine;
using Voxon.EyeTracker;
using Voxon.GazeDetection;

namespace Voxon.Utilities
{
    /// <summary>
    /// Visualizes gaze ray in the scene for debugging
    /// </summary>
    public class GazeVisualizer : MonoBehaviour
    {
        [Header("Visualization Settings")]
        [SerializeField] private bool showGazeRay = true;
        [SerializeField] private Color gazeRayColor = Color.red;
        [SerializeField] private float rayLength = 10f;
        [SerializeField] private float rayWidth = 0.02f;
        [SerializeField] private bool showHitPoint = true;
        [SerializeField] private float hitPointSize = 0.1f;

        private GazeRaycast gazeRaycast;
        private EyeTrackerManager eyeTrackerManager;
        private LineRenderer lineRenderer;
        private GameObject hitPointIndicator;

        private void Start()
        {
            gazeRaycast = FindObjectOfType<GazeRaycast>();
            eyeTrackerManager = EyeTrackerManager.Instance;

            if (showGazeRay)
            {
                SetupLineRenderer();
            }

            if (showHitPoint)
            {
                SetupHitPointIndicator();
            }
        }

        private void SetupLineRenderer()
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.color = gazeRayColor;
            lineRenderer.startWidth = rayWidth;
            lineRenderer.endWidth = rayWidth;
            lineRenderer.useWorldSpace = true;
            lineRenderer.positionCount = 2;
        }

        private void SetupHitPointIndicator()
        {
            hitPointIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hitPointIndicator.name = "GazeHitPoint";
            hitPointIndicator.transform.localScale = Vector3.one * hitPointSize;
            
            Renderer renderer = hitPointIndicator.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = gazeRayColor;
            }

            // Remove collider as it's just a visual indicator
            Collider collider = hitPointIndicator.GetComponent<Collider>();
            if (collider != null)
            {
                Destroy(collider);
            }

            hitPointIndicator.SetActive(false);
        }

        private void Update()
        {
            if (gazeRaycast == null || eyeTrackerManager == null || !eyeTrackerManager.IsConnected())
            {
                if (lineRenderer != null)
                {
                    lineRenderer.enabled = false;
                }
                if (hitPointIndicator != null)
                {
                    hitPointIndicator.SetActive(false);
                }
                return;
            }

            if (gazeRaycast.PerformRaycast(out RaycastHit hitInfo))
            {
                // Show hit point
                if (hitPointIndicator != null)
                {
                    hitPointIndicator.SetActive(true);
                    hitPointIndicator.transform.position = hitInfo.point;
                }

                // Draw ray to hit point
                if (lineRenderer != null && showGazeRay)
                {
                    GazeData gazeData = eyeTrackerManager.GetGazeData();
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, gazeData.gazeOrigin);
                    lineRenderer.SetPosition(1, hitInfo.point);
                }
            }
            else
            {
                // Draw ray without hit
                if (lineRenderer != null && showGazeRay)
                {
                    GazeData gazeData = eyeTrackerManager.GetGazeData();
                    if (gazeData.isValid)
                    {
                        lineRenderer.enabled = true;
                        Vector3 endPoint = gazeData.gazeOrigin + gazeData.gazeDirection * rayLength;
                        lineRenderer.SetPosition(0, gazeData.gazeOrigin);
                        lineRenderer.SetPosition(1, endPoint);
                    }
                    else
                    {
                        lineRenderer.enabled = false;
                    }
                }

                if (hitPointIndicator != null)
                {
                    hitPointIndicator.SetActive(false);
                }
            }
        }

        private void OnDestroy()
        {
            if (hitPointIndicator != null)
            {
                Destroy(hitPointIndicator);
            }
        }
    }
}

