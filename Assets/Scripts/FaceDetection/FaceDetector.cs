using UnityEngine;
using Voxon.FaceDetection;

namespace Voxon.FaceDetection
{
    /// <summary>
    /// Detects human faces in camera/webcam feed
    /// </summary>
    public class FaceDetector : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField] private FaceDetectionProvider detectionProvider;
        [SerializeField] private bool autoStart = true;
        [SerializeField] private int targetFPS = 30;

        private float lastDetectionTime = 0f;
        private float detectionInterval;

        private void Start()
        {
            detectionInterval = 1f / targetFPS;

            if (detectionProvider == null)
            {
                Debug.LogWarning("FaceDetectionProvider not assigned. Creating GenericFaceDetectionProvider.");
                detectionProvider = gameObject.AddComponent<GenericFaceDetectionProvider>();
            }

            if (autoStart)
            {
                InitializeAndStart();
            }
        }

        /// <summary>
        /// Initialize and start face detection
        /// </summary>
        public void InitializeAndStart()
        {
            if (detectionProvider != null && detectionProvider.Initialize())
            {
                detectionProvider.OnExpressionDetected += HandleExpressionDetected;
                detectionProvider.StartDetection();
            }
        }

        /// <summary>
        /// Stop face detection
        /// </summary>
        public void StopDetection()
        {
            if (detectionProvider != null)
            {
                detectionProvider.OnExpressionDetected -= HandleExpressionDetected;
                detectionProvider.StopDetection();
            }
        }

        /// <summary>
        /// Get the current detected expression
        /// </summary>
        public HumanExpressionData GetCurrentExpression()
        {
            if (detectionProvider != null && detectionProvider.IsRunning)
            {
                return detectionProvider.GetCurrentExpression();
            }
            return new HumanExpressionData();
        }

        private void HandleExpressionDetected(HumanExpressionData expressionData)
        {
            // Expression detected, can be used by other systems
        }

        private void OnDestroy()
        {
            StopDetection();
        }
    }
}

