using System;
using UnityEngine;
using Voxon.FaceDetection;

namespace Voxon.FaceDetection
{
    /// <summary>
    /// Abstract base class for different face detection implementations
    /// </summary>
    public abstract class FaceDetectionProvider : MonoBehaviour
    {
        public event Action<HumanExpressionData> OnExpressionDetected;

        protected bool isInitialized = false;
        protected bool isRunning = false;

        /// <summary>
        /// Initialize the face detection provider
        /// </summary>
        public abstract bool Initialize();

        /// <summary>
        /// Start face detection
        /// </summary>
        public abstract void StartDetection();

        /// <summary>
        /// Stop face detection
        /// </summary>
        public abstract void StopDetection();

        /// <summary>
        /// Get the current detected expression
        /// </summary>
        public abstract HumanExpressionData GetCurrentExpression();

        /// <summary>
        /// Check if detection is running
        /// </summary>
        public bool IsRunning => isRunning;

        /// <summary>
        /// Check if provider is initialized
        /// </summary>
        public bool IsInitialized => isInitialized;

        /// <summary>
        /// Invoke expression detected event (protected so derived classes can call it)
        /// </summary>
        protected void InvokeExpressionDetected(HumanExpressionData expressionData)
        {
            OnExpressionDetected?.Invoke(expressionData);
        }

        protected virtual void OnDestroy()
        {
            StopDetection();
        }
    }
}

