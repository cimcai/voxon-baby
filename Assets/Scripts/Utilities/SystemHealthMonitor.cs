using UnityEngine;
using System.Collections.Generic;
using Voxon.EyeTracker;
using Voxon.FaceDetection;

namespace Voxon.Utilities
{
    /// <summary>
    /// Monitors system health and performance metrics
    /// </summary>
    public class SystemHealthMonitor : MonoBehaviour
    {
        [Header("Monitoring Settings")]
        [SerializeField] private bool monitorEyeTracker = true;
        [SerializeField] private bool monitorFaceDetection = true;
        [SerializeField] private bool monitorPerformance = true;
        [SerializeField] private float healthCheckInterval = 5f;

        [Header("Thresholds")]
        [SerializeField] private float maxFrameTime = 0.033f; // 30 FPS
        [SerializeField] private int maxMemoryMB = 1024;

        private float lastHealthCheck = 0f;
        private Dictionary<string, float> healthMetrics = new Dictionary<string, float>();

        public SystemHealthStatus GetSystemHealth()
        {
            SystemHealthStatus status = new SystemHealthStatus
            {
                overallHealth = 1f,
                eyeTrackerHealth = 1f,
                faceDetectionHealth = 1f,
                performanceHealth = 1f,
                issues = new List<string>()
            };

            if (monitorEyeTracker)
            {
                EyeTrackerManager eyeTracker = EyeTrackerManager.Instance;
                if (eyeTracker == null || !eyeTracker.IsConnected())
                {
                    status.eyeTrackerHealth = 0f;
                    status.issues.Add("Eye tracker not connected");
                }
                else
                {
                    status.eyeTrackerHealth = 1f;
                }
            }

            if (monitorFaceDetection)
            {
                FaceDetector faceDetector = FindObjectOfType<FaceDetector>();
                if (faceDetector == null)
                {
                    status.faceDetectionHealth = 0.5f;
                    status.issues.Add("Face detector not found");
                }
                else
                {
                    status.faceDetectionHealth = 1f;
                }
            }

            if (monitorPerformance)
            {
                float frameTime = Time.deltaTime;
                if (frameTime > maxFrameTime)
                {
                    status.performanceHealth = Mathf.Clamp01(1f - (frameTime - maxFrameTime) / maxFrameTime);
                    status.issues.Add($"High frame time: {frameTime * 1000f:F1}ms");
                }

                long memoryMB = System.GC.GetTotalMemory(false) / 1024 / 1024;
                if (memoryMB > maxMemoryMB)
                {
                    status.performanceHealth *= 0.5f;
                    status.issues.Add($"High memory usage: {memoryMB}MB");
                }
            }

            status.overallHealth = (status.eyeTrackerHealth + status.faceDetectionHealth + status.performanceHealth) / 3f;

            return status;
        }

        private void Update()
        {
            if (Time.time - lastHealthCheck > healthCheckInterval)
            {
                lastHealthCheck = Time.time;
                SystemHealthStatus health = GetSystemHealth();
                
                if (health.overallHealth < 0.5f)
                {
                    Debug.LogWarning($"System health low: {health.overallHealth:P0}");
                    foreach (string issue in health.issues)
                    {
                        Debug.LogWarning($"  - {issue}");
                    }
                }

                healthMetrics["Overall"] = health.overallHealth;
                healthMetrics["EyeTracker"] = health.eyeTrackerHealth;
                healthMetrics["FaceDetection"] = health.faceDetectionHealth;
                healthMetrics["Performance"] = health.performanceHealth;
            }
        }

        public Dictionary<string, float> GetHealthMetrics()
        {
            return new Dictionary<string, float>(healthMetrics);
        }
    }

    [System.Serializable]
    public class SystemHealthStatus
    {
        public float overallHealth;
        public float eyeTrackerHealth;
        public float faceDetectionHealth;
        public float performanceHealth;
        public List<string> issues;
    }
}

