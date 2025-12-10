using System;
using UnityEngine;
using Voxon.EyeTracker;

namespace Voxon.EyeTracker
{
    /// <summary>
    /// Singleton manager that initializes and manages eye tracker connection
    /// </summary>
    public class EyeTrackerManager : MonoBehaviour
    {
        private static EyeTrackerManager _instance;
        public static EyeTrackerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("EyeTrackerManager");
                    _instance = go.AddComponent<EyeTrackerManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        [Header("Eye Tracker Settings")]
        [SerializeField] private EyeTrackerProvider eyeTrackerProvider;
        [SerializeField] private bool autoInitialize = true;

        public event Action<GazeData> OnGazeDataUpdated;
        public event Action<bool> OnConnectionStateChanged;

        private bool isInitialized = false;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);

            if (eyeTrackerProvider == null)
            {
                Debug.LogWarning("EyeTrackerProvider not assigned. Creating GenericEyeTrackerProvider.");
                eyeTrackerProvider = gameObject.AddComponent<GenericEyeTrackerProvider>();
            }
        }

        private void Start()
        {
            if (autoInitialize)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Initialize the eye tracker system
        /// </summary>
        public bool Initialize()
        {
            if (isInitialized)
            {
                Debug.LogWarning("Eye tracker already initialized.");
                return true;
            }

            if (eyeTrackerProvider == null)
            {
                Debug.LogError("EyeTrackerProvider is null. Cannot initialize.");
                return false;
            }

            if (eyeTrackerProvider.Initialize())
            {
                isInitialized = true;
                Debug.Log("Eye tracker initialized successfully.");

                if (eyeTrackerProvider.Connect())
                {
                    eyeTrackerProvider.OnGazeDataUpdated += HandleGazeDataUpdated;
                    OnConnectionStateChanged?.Invoke(true);
                    Debug.Log("Eye tracker connected successfully.");
                    return true;
                }
                else
                {
                    Debug.LogError("Failed to connect to eye tracker.");
                    return false;
                }
            }
            else
            {
                Debug.LogError("Failed to initialize eye tracker.");
                return false;
            }
        }

        /// <summary>
        /// Get the current gaze data
        /// </summary>
        public GazeData GetGazeData()
        {
            if (eyeTrackerProvider != null && eyeTrackerProvider.IsConnected)
            {
                return eyeTrackerProvider.GetGazeData();
            }
            return new GazeData();
        }

        /// <summary>
        /// Check if eye tracker is connected
        /// </summary>
        public bool IsConnected()
        {
            return eyeTrackerProvider != null && eyeTrackerProvider.IsConnected;
        }

        private void HandleGazeDataUpdated(GazeData gazeData)
        {
            OnGazeDataUpdated?.Invoke(gazeData);
        }

        private void OnDestroy()
        {
            if (eyeTrackerProvider != null)
            {
                eyeTrackerProvider.OnGazeDataUpdated -= HandleGazeDataUpdated;
                eyeTrackerProvider.Disconnect();
            }
        }
    }
}

