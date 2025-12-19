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
            // Try new Input System first
            try
            {
                // Enable mouse device if needed
                var mouse = UnityEngine.InputSystem.Mouse.current;
                if (mouse == null)
                {
                    // Try to add/enable mouse device
                    try
                    {
                        UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Mouse>();
                        mouse = UnityEngine.InputSystem.Mouse.current;
                    }
                    catch
                    {
                        // Can't add device, continue to legacy
                    }
                }
                
                if (mouse != null)
                {
                    // Try to read mouse position
                    try
                    {
                        if (mouse.position.enabled)
                        {
                            return mouse.position.ReadValue();
                        }
                        else
                        {
                            // Try to enable it
                            mouse.position.Enable();
                            return mouse.position.ReadValue();
                        }
                    }
                    catch
                    {
                        // Position not readable, try value property
                        try
                        {
                            return mouse.position.value;
                        }
                        catch
                        {
                            // Still failed, fall through
                        }
                    }
                }
            }
            catch
            {
                // New Input System failed, fall through to legacy
            }
#endif
            
            // Try legacy Input System
            try
            {
                return Input.mousePosition;
            }
            catch (System.InvalidOperationException)
            {
                // New Input System is blocking legacy Input
                // This means we're in "Input System Package (New)" mode
                // We need to use new Input System, but mouse might not be initialized
                
                // Log warning only once with helpful message
                if (!hasLoggedInputWarning)
                {
                    Debug.LogWarning($"Input System: New Input System is active but mouse device not accessible. " +
                        $"Gaze simulation will use screen center. To fix: Edit → Project Settings → Player → " +
                        $"Active Input Handling → Set to 'Both' or 'Input Manager (Old)'");
                    hasLoggedInputWarning = true;
                }
                
                // Return screen center as fallback
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

