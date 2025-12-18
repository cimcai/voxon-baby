using System;
using System.Collections.Generic;
using UnityEngine;
using Voxon.CatFace;
using Voxon.FaceDetection;
using Voxon.EyeTracker;
using Voxon.VolumetricShapes;

namespace Voxon.Utilities
{
    /// <summary>
    /// Logs system events for debugging and analysis
    /// </summary>
    public class EventLogger : MonoBehaviour
    {
        [Header("Logging Settings")]
        [SerializeField] private bool enableLogging = true;
        [SerializeField] private bool logToFile = false;
        [SerializeField] private bool logToConsole = true;
        [SerializeField] private int maxLogEntries = 1000;

        [Header("Event Types")]
        [SerializeField] private bool logGazeEvents = true;
        [SerializeField] private bool logExpressionEvents = true;
        [SerializeField] private bool logShapeEvents = true;
        [SerializeField] private bool logLLMEvents = true;

        private List<LogEntry> logEntries = new List<LogEntry>();

        [Serializable]
        public class LogEntry
        {
            public float timestamp;
            public string eventType;
            public string message;
            public string details;

            public LogEntry(string type, string msg, string det = "")
            {
                timestamp = Time.time;
                eventType = type;
                message = msg;
                details = det;
            }
        }

        private void Start()
        {
            if (enableLogging)
            {
                SubscribeToEvents();
            }
        }

        private void SubscribeToEvents()
        {
            // Eye Tracker Events
            if (logGazeEvents)
            {
                EyeTrackerManager eyeTracker = EyeTrackerManager.Instance;
                if (eyeTracker != null)
                {
                    eyeTracker.OnGazeDataUpdated += OnGazeDataUpdated;
                    eyeTracker.OnConnectionStateChanged += OnEyeTrackerConnectionChanged;
                }
            }

            // Cat Face Events
            if (logExpressionEvents)
            {
                CatFaceController catFace = FindObjectOfType<CatFaceController>();
                if (catFace != null)
                {
                    ExpressionManager expressionManager = catFace.GetComponent<ExpressionManager>();
                    if (expressionManager != null)
                    {
                        expressionManager.OnExpressionChanged += OnCatExpressionChanged;
                    }
                }
            }

            // Shape Events
            if (logShapeEvents)
            {
                GazeDetection.GazeHitDetector hitDetector = FindObjectOfType<GazeDetection.GazeHitDetector>();
                if (hitDetector != null)
                {
                    hitDetector.OnShapeFocused += OnShapeFocused;
                    hitDetector.OnShapeHighlighted += OnShapeHighlighted;
                }
            }
        }

        private void OnGazeDataUpdated(GazeData gazeData)
        {
            if (gazeData.isValid)
            {
                Log("Gaze", "Gaze data updated", $"Origin: {gazeData.gazeOrigin}, Direction: {gazeData.gazeDirection}");
            }
        }

        private void OnEyeTrackerConnectionChanged(bool connected)
        {
            Log("EyeTracker", connected ? "Connected" : "Disconnected", "");
        }

        private void OnCatExpressionChanged(CatFace.ExpressionType expression)
        {
            Log("CatExpression", $"Expression changed to {expression}", "");
        }

        private void OnShapeFocused(VolumetricShape shape)
        {
            Log("Shape", $"Shape focused: {shape.name}", "");
        }

        private void OnShapeHighlighted(VolumetricShape shape)
        {
            Log("Shape", $"Shape highlighted: {shape.name}", "");
        }

        public void Log(string eventType, string message, string details = "")
        {
            if (!enableLogging) return;

            LogEntry entry = new LogEntry(eventType, message, details);
            logEntries.Add(entry);

            // Trim log if too large
            if (logEntries.Count > maxLogEntries)
            {
                logEntries.RemoveAt(0);
            }

            // Log to console
            if (logToConsole)
            {
                string logMessage = $"[{entry.timestamp:F2}] [{entry.eventType}] {entry.message}";
                if (!string.IsNullOrEmpty(entry.details))
                {
                    logMessage += $" - {entry.details}";
                }
                Debug.Log(logMessage);
            }

            // Log to file (if enabled)
            if (logToFile)
            {
                // File logging would be implemented here
                // For now, just keep in memory
            }
        }

        public List<LogEntry> GetLogEntries()
        {
            return new List<LogEntry>(logEntries);
        }

        public void ClearLog()
        {
            logEntries.Clear();
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            EyeTrackerManager eyeTracker = EyeTrackerManager.Instance;
            if (eyeTracker != null)
            {
                eyeTracker.OnGazeDataUpdated -= OnGazeDataUpdated;
                eyeTracker.OnConnectionStateChanged -= OnEyeTrackerConnectionChanged;
            }
        }
    }
}

