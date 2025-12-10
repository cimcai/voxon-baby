using UnityEngine;
using Voxon.EyeTracker;

namespace Voxon.Camera
{
    /// <summary>
    /// Configures main camera for eye tracking
    /// </summary>
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class EyeTrackerCamera : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private bool autoSetup = true;
        [SerializeField] private float fieldOfView = 60f;
        [SerializeField] private float nearClipPlane = 0.1f;
        [SerializeField] private float farClipPlane = 1000f;

        [Header("Calibration")]
        [SerializeField] private bool showCalibrationUI = false;
        [SerializeField] private GameObject calibrationUIPrefab;

        private UnityEngine.Camera cam;
        private EyeTrackerManager eyeTrackerManager;

        private void Awake()
        {
            cam = GetComponent<UnityEngine.Camera>();
            eyeTrackerManager = EyeTrackerManager.Instance;
        }

        private void Start()
        {
            if (autoSetup)
            {
                SetupCamera();
            }
        }

        /// <summary>
        /// Setup camera for eye tracking
        /// </summary>
        public void SetupCamera()
        {
            if (cam == null)
            {
                cam = GetComponent<UnityEngine.Camera>();
            }

            if (cam != null)
            {
                cam.fieldOfView = fieldOfView;
                cam.nearClipPlane = nearClipPlane;
                cam.farClipPlane = farClipPlane;
                cam.stereoTargetEye = StereoTargetEyeMask.None; // Mono for eye tracking
            }

            // Ensure camera is tagged as MainCamera
            if (tag != "MainCamera")
            {
                tag = "MainCamera";
            }
        }

        /// <summary>
        /// Show calibration UI if needed
        /// </summary>
        public void ShowCalibration()
        {
            if (showCalibrationUI && calibrationUIPrefab != null)
            {
                Instantiate(calibrationUIPrefab);
            }
        }

        /// <summary>
        /// Get the camera component
        /// </summary>
        public UnityEngine.Camera GetCamera()
        {
            return cam;
        }
    }
}

