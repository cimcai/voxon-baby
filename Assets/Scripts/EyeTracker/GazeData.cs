using System;
using UnityEngine;

namespace Voxon.EyeTracker
{
    /// <summary>
    /// Data structure containing gaze information from eye tracker
    /// </summary>
    [Serializable]
    public class GazeData
    {
        public Vector3 gazeOrigin;
        public Vector3 gazeDirection;
        public float timestamp;
        public bool isValid;

        public GazeData()
        {
            gazeOrigin = Vector3.zero;
            gazeDirection = Vector3.forward;
            timestamp = 0f; // Set during runtime, not during serialization
            isValid = false;
        }

        public GazeData(Vector3 origin, Vector3 direction, bool valid = true)
        {
            gazeOrigin = origin;
            gazeDirection = direction.normalized;
            // Only set timestamp if actually playing (not during serialization)
            try
            {
                timestamp = Application.isPlaying ? Time.time : 0f;
            }
            catch (UnityException)
            {
                // Called during serialization, use default
                timestamp = 0f;
            }
            isValid = valid;
        }
    }
}

