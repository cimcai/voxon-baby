using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voxon.FaceDetection
{
    /// <summary>
    /// Enumeration of detected facial expressions
    /// </summary>
    public enum ExpressionType
    {
        Neutral,
        Happy,
        Sad,
        Surprised,
        Angry,
        Confused,
        Excited
    }

    /// <summary>
    /// Data structure for detected expressions
    /// </summary>
    [Serializable]
    public class HumanExpressionData
    {
        public ExpressionType expressionType;
        public float confidence;
        public float timestamp;
        public float intensity; // 0-1 scale
        public Vector3 facePosition;
        public Quaternion faceRotation;
        public List<ExpressionType> expressionHistory;

        public HumanExpressionData()
        {
            expressionType = ExpressionType.Neutral;
            confidence = 0f;
            timestamp = 0f; // Set during runtime, not during serialization
            intensity = 0f;
            facePosition = Vector3.zero;
            faceRotation = Quaternion.identity;
            expressionHistory = new List<ExpressionType>();
        }

        public HumanExpressionData(ExpressionType type, float conf, float intens = 0.5f)
        {
            expressionType = type;
            confidence = conf;
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
            intensity = intens;
            facePosition = Vector3.zero;
            faceRotation = Quaternion.identity;
            expressionHistory = new List<ExpressionType>();
            expressionHistory.Add(type);
        }
    }
}

