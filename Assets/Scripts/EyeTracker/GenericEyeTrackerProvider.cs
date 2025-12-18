using UnityEngine;
using Voxon.EyeTracker;

namespace Voxon.EyeTracker
{
    /// <summary>
    /// Generic eye tracker provider that simulates eye tracking using mouse position
    /// Useful for testing without hardware
    /// </summary>
    public class GenericEyeTrackerProvider : EyeTrackerProvider
    {
        [Header("Simulation Settings")]
        [SerializeField] private UnityEngine.Camera targetCamera;
        [SerializeField] private float maxRaycastDistance = 100f;
        [SerializeField] private LayerMask raycastLayerMask = -1;

        private GazeData currentGazeData;

        public override bool Initialize()
        {
            if (targetCamera == null)
            {
                targetCamera = UnityEngine.Camera.main;
                if (targetCamera == null)
                {
                    Debug.LogError("No camera found. Cannot initialize GenericEyeTrackerProvider.");
                    return false;
                }
            }

            isInitialized = true;
            Debug.Log("GenericEyeTrackerProvider initialized (using mouse simulation).");
            return true;
        }

        public override bool Connect()
        {
            if (!isInitialized)
            {
                Debug.LogError("Provider not initialized. Call Initialize() first.");
                return false;
            }

            isConnected = true;
            currentGazeData = new GazeData();
            Debug.Log("GenericEyeTrackerProvider connected.");
            return true;
        }

        public override void Disconnect()
        {
            isConnected = false;
            Debug.Log("GenericEyeTrackerProvider disconnected.");
        }

        public override GazeData GetGazeData()
        {
            if (!isConnected || targetCamera == null)
            {
                return new GazeData(Vector3.zero, Vector3.forward, false);
            }

            // Convert mouse position to world space ray
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = targetCamera.ScreenPointToRay(mousePosition);

            currentGazeData.gazeOrigin = ray.origin;
            currentGazeData.gazeDirection = ray.direction.normalized;
            currentGazeData.timestamp = Time.time;
            currentGazeData.isValid = true;

            return currentGazeData;
        }

        /// <summary>
        /// Get gaze data as screen coordinates (0-1 normalized)
        /// </summary>
        public Vector2 GetGazeScreenPosition()
        {
            if (!isConnected || targetCamera == null)
            {
                return Vector2.zero;
            }

            Vector3 mousePosition = Input.mousePosition;
            return new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
        }
    }
}

