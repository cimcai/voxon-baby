using UnityEngine;
using Voxon.FaceDetection;
using System.Collections;

namespace Voxon.FaceDetection
{
    /// <summary>
    /// Generic face detection provider for testing (simulated)
    /// </summary>
    public class GenericFaceDetectionProvider : FaceDetectionProvider
    {
        [Header("Simulation Settings")]
        [SerializeField] private float updateInterval = 0.1f;
        [SerializeField] private bool simulateExpressions = true;

        private HumanExpressionData currentExpression;
        private Coroutine detectionCoroutine;

        public override bool Initialize()
        {
            isInitialized = true;
            currentExpression = new HumanExpressionData();
            Debug.Log("GenericFaceDetectionProvider initialized (simulated).");
            return true;
        }

        public override void StartDetection()
        {
            if (!isInitialized)
            {
                Debug.LogError("Provider not initialized. Call Initialize() first.");
                return;
            }

            isRunning = true;
            detectionCoroutine = StartCoroutine(DetectionLoop());
            Debug.Log("GenericFaceDetectionProvider started.");
        }

        public override void StopDetection()
        {
            isRunning = false;
            if (detectionCoroutine != null)
            {
                StopCoroutine(detectionCoroutine);
                detectionCoroutine = null;
            }
            Debug.Log("GenericFaceDetectionProvider stopped.");
        }

        public override HumanExpressionData GetCurrentExpression()
        {
            return currentExpression;
        }

        private IEnumerator DetectionLoop()
        {
            while (isRunning)
            {
                if (simulateExpressions)
                {
                    // Simulate random expressions for testing
                    ExpressionType randomExpression = (ExpressionType)Random.Range(0, System.Enum.GetValues(typeof(ExpressionType)).Length);
                    float confidence = Random.Range(0.6f, 1f);
                    float intensity = Random.Range(0.3f, 1f);

                    currentExpression = new HumanExpressionData(randomExpression, confidence, intensity);
                    InvokeExpressionDetected(currentExpression);
                }

                yield return new WaitForSeconds(updateInterval);
            }
        }
    }
}

