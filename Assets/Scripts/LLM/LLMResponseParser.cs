using System;
using System.Text.RegularExpressions;
using UnityEngine;
using Voxon.FaceDetection;

namespace Voxon.LLM
{
    /// <summary>
    /// Parses LLM API responses
    /// </summary>
    [Serializable]
    public class LLMExpressionResponse
    {
        public FaceDetection.ExpressionType expression;
        public float intensity;
        public float duration;
        public string reasoning;

        public LLMExpressionResponse()
        {
            expression = FaceDetection.ExpressionType.Neutral;
            intensity = 0.5f;
            duration = 2f;
            reasoning = "";
        }
    }

    public class LLMResponseParser : MonoBehaviour
    {
        [Header("Parser Settings")]
        [SerializeField] private FaceDetection.ExpressionType defaultExpression = FaceDetection.ExpressionType.Neutral;
        [SerializeField] private float defaultIntensity = 0.5f;
        [SerializeField] private float defaultDuration = 2f;

        /// <summary>
        /// Parse LLM response into structured data
        /// </summary>
        public LLMExpressionResponse ParseResponse(string llmResponse)
        {
            LLMExpressionResponse response = new LLMExpressionResponse();

            try
            {
                // Try to extract JSON from response
                string json = ExtractJson(llmResponse);
                
                if (!string.IsNullOrEmpty(json))
                {
                    // Parse JSON (simplified - in production use JsonUtility or Newtonsoft.Json)
                    response.expression = ParseExpression(json);
                    response.intensity = ParseFloat(json, "intensity", defaultIntensity);
                    response.duration = ParseFloat(json, "duration", defaultDuration);
                    response.reasoning = ParseString(json, "reasoning", "");
                }
                else
                {
                    // Fallback: try to extract expression from natural language
                    response.expression = ParseExpressionFromText(llmResponse);
                    response.intensity = defaultIntensity;
                    response.duration = defaultDuration;
                    response.reasoning = llmResponse;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing LLM response: {e.Message}");
                response.expression = defaultExpression;
                response.intensity = defaultIntensity;
                response.duration = defaultDuration;
            }

            return response;
        }

        private string ExtractJson(string text)
        {
            // Try to find JSON object in the response
            int startIndex = text.IndexOf('{');
            int endIndex = text.LastIndexOf('}');
            
            if (startIndex >= 0 && endIndex > startIndex)
            {
                return text.Substring(startIndex, endIndex - startIndex + 1);
            }

            return "";
        }

        private FaceDetection.ExpressionType ParseExpression(string json)
        {
            // Try to find expression in JSON
            Match match = Regex.Match(json, @"""expression""\s*:\s*""([^""]+)""", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string expressionStr = match.Groups[1].Value.ToLower();
                return ParseExpressionFromText(expressionStr);
            }

            return defaultExpression;
        }

        private FaceDetection.ExpressionType ParseExpressionFromText(string text)
        {
            string lowerText = text.ToLower();

            if (lowerText.Contains("happy") || lowerText.Contains("smile") || lowerText.Contains("joy"))
                return FaceDetection.ExpressionType.Happy;
            if (lowerText.Contains("curious") || lowerText.Contains("interested"))
                return FaceDetection.ExpressionType.Confused; // Map to closest human expression
            if (lowerText.Contains("surprised") || lowerText.Contains("surprise"))
                return FaceDetection.ExpressionType.Surprised;
            if (lowerText.Contains("sleepy") || lowerText.Contains("tired"))
                return FaceDetection.ExpressionType.Neutral;
            if (lowerText.Contains("playful") || lowerText.Contains("wink"))
                return FaceDetection.ExpressionType.Excited;
            if (lowerText.Contains("focused") || lowerText.Contains("attentive"))
                return FaceDetection.ExpressionType.Neutral;
            if (lowerText.Contains("sad") || lowerText.Contains("frown"))
                return FaceDetection.ExpressionType.Sad;

            return defaultExpression;
        }

        private float ParseFloat(string json, string key, float defaultValue)
        {
            Match match = Regex.Match(json, $@"""{key}""\s*:\s*([0-9.]+)", RegexOptions.IgnoreCase);
            if (match.Success && float.TryParse(match.Groups[1].Value, out float value))
            {
                // Only clamp intensity to 0-1, duration can be any positive value
                if (key == "intensity")
                {
                    return Mathf.Clamp01(value);
                }
                return Mathf.Max(0f, value);
            }
            return defaultValue;
        }

        private string ParseString(string json, string key, string defaultValue)
        {
            Match match = Regex.Match(json, $@"""{key}""\s*:\s*""([^""]+)""", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return defaultValue;
        }
    }
}

