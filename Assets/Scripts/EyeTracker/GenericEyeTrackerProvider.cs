using UnityEngine;
using Voxon.EyeTracker;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

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
        private static bool hasLoggedInputWarning = false;
        
        /// <summary>
        /// Get mouse position compatible with both Input Systems
        /// </summary>
        private Vector3 GetMousePosition()
        {
#if ENABLE_INPUT_SYSTEM
            // Check if new Input System is actually available and mouse is present
            try
            {
                // Check if Input System is enabled and mouse device exists
                if (UnityEngine.InputSystem.InputSystem.devices != null)
                {
                    var mouse = UnityEngine.InputSystem.Mouse.current;
                    if (mouse != null && mouse.position.isReadable)
                    {
                        return mouse.position.ReadValue();
                    }
                }
            }
            catch
            {
                // Fall through to legacy
            }
#endif
            
            // Try legacy Input System
            try
            {
                return Input.mousePosition;
            }
            catch (System.InvalidOperationException)
            {
                // New Input System is active but we can't access mouse
                // Log warning only once to avoid spam
                if (!hasLoggedInputWarning)
                {
                    Debug.LogWarning("Input System: New Input System is active but mouse access failed. Using screen center for gaze simulation. This is normal if Input System package is installed but not fully configured.");
                    hasLoggedInputWarning = true;
                }
                return new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            }
        }

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
            Vector3 mousePosition = GetMousePosition();
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

            Vector3 mousePosition = GetMousePosition();
            return new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
        }
    }
}

