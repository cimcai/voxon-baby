using UnityEngine;
using Voxon.EyeTracker;

namespace Voxon.GazeDetection
{
    /// <summary>
    /// Performs raycast from camera using gaze data
    /// </summary>
    public class GazeRaycast : MonoBehaviour
    {
        [Header("Raycast Settings")]
        [SerializeField] private float maxRaycastDistance = 100f;
        [SerializeField] private LayerMask raycastLayerMask = -1;
        [SerializeField] private UnityEngine.Camera targetCamera;

        private EyeTrackerManager eyeTrackerManager;

        private void Start()
        {
            eyeTrackerManager = EyeTrackerManager.Instance;
            
            if (targetCamera == null)
            {
                targetCamera = UnityEngine.Camera.main;
            }
        }

        /// <summary>
        /// Perform raycast using current gaze data
        /// </summary>
        public bool PerformRaycast(out RaycastHit hitInfo)
        {
            hitInfo = new RaycastHit();

            if (eyeTrackerManager == null || !eyeTrackerManager.IsConnected())
            {
                return false;
            }

            GazeData gazeData = eyeTrackerManager.GetGazeData();
            if (!gazeData.isValid)
            {
                return false;
            }

            Ray ray = new Ray(gazeData.gazeOrigin, gazeData.gazeDirection);
            return Physics.Raycast(ray, out hitInfo, maxRaycastDistance, raycastLayerMask);
        }

        /// <summary>
        /// Perform raycast using provided gaze data
        /// </summary>
        public bool PerformRaycast(GazeData gazeData, out RaycastHit hitInfo)
        {
            hitInfo = new RaycastHit();

            if (!gazeData.isValid)
            {
                return false;
            }

            Ray ray = new Ray(gazeData.gazeOrigin, gazeData.gazeDirection);
            return Physics.Raycast(ray, out hitInfo, maxRaycastDistance, raycastLayerMask);
        }

        /// <summary>
        /// Get the current gaze ray
        /// </summary>
        public bool GetGazeRay(out Ray ray)
        {
            ray = new Ray();

            if (eyeTrackerManager == null || !eyeTrackerManager.IsConnected())
            {
                return false;
            }

            GazeData gazeData = eyeTrackerManager.GetGazeData();
            if (!gazeData.isValid)
            {
                return false;
            }

            ray = new Ray(gazeData.gazeOrigin, gazeData.gazeDirection);
            return true;
        }
    }
}

