using System;
using UnityEngine;
using Voxon.EyeTracker;

namespace Voxon.EyeTracker
{
    /// <summary>
    /// Abstract base class for different eye tracker implementations
    /// </summary>
    public abstract class EyeTrackerProvider : MonoBehaviour
    {
        public event Action<GazeData> OnGazeDataUpdated;

        protected bool isInitialized = false;
        protected bool isConnected = false;

        /// <summary>
        /// Initialize the eye tracker provider
        /// </summary>
        public abstract bool Initialize();

        /// <summary>
        /// Connect to the eye tracker hardware
        /// </summary>
        public abstract bool Connect();

        /// <summary>
        /// Disconnect from the eye tracker hardware
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// Get the current gaze data
        /// </summary>
        public abstract GazeData GetGazeData();

        /// <summary>
        /// Check if the eye tracker is connected and ready
        /// </summary>
        public bool IsConnected => isConnected;

        /// <summary>
        /// Check if the eye tracker is initialized
        /// </summary>
        public bool IsInitialized => isInitialized;

        protected virtual void Update()
        {
            if (isConnected)
            {
                GazeData gazeData = GetGazeData();
                if (gazeData != null && gazeData.isValid)
                {
                    OnGazeDataUpdated?.Invoke(gazeData);
                }
            }
        }

        protected virtual void OnDestroy()
        {
            Disconnect();
        }
    }
}

