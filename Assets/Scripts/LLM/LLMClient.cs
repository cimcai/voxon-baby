using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Voxon.LLM
{
    /// <summary>
    /// Client wrapper for LLM API communication
    /// </summary>
    public class LLMClient : MonoBehaviour
    {
        [Header("API Settings")]
        [SerializeField] private string apiKey = "";
        [SerializeField] private string apiEndpoint = "https://api.openai.com/v1/chat/completions";
        [SerializeField] private string model = "gpt-4";
        [SerializeField] private float requestCooldown = 1f;

        [Header("Provider")]
        [SerializeField] private LLMProvider provider = LLMProvider.OpenAI;

        private float lastRequestTime = 0f;
        private bool isRequestInProgress = false;

        public enum LLMProvider
        {
            OpenAI,
            Anthropic,
            Custom
        }

        public event Action<string> OnResponseReceived;
        public event Action<string> OnError;

        /// <summary>
        /// Send a request to the LLM API
        /// </summary>
        public void SendRequest(string prompt, System.Action<string> onComplete = null)
        {
            if (isRequestInProgress)
            {
                Debug.LogWarning("LLM request already in progress.");
                return;
            }

            if (Time.time - lastRequestTime < requestCooldown)
            {
                Debug.LogWarning("Request cooldown active. Please wait.");
                return;
            }

            StartCoroutine(SendRequestCoroutine(prompt, onComplete));
        }

        private IEnumerator SendRequestCoroutine(string prompt, System.Action<string> onComplete)
        {
            isRequestInProgress = true;
            lastRequestTime = Time.time;

            string requestBody = BuildRequestBody(prompt);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);

            using (UnityWebRequest request = new UnityWebRequest(apiEndpoint, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string response = request.downloadHandler.text;
                    string parsedResponse = ParseResponse(response);
                    OnResponseReceived?.Invoke(parsedResponse);
                    onComplete?.Invoke(parsedResponse);
                }
                else
                {
                    string error = $"LLM API Error: {request.error}";
                    Debug.LogError(error);
                    OnError?.Invoke(error);
                }
            }

            isRequestInProgress = false;
        }

        private string BuildRequestBody(string prompt)
        {
            // Build JSON request body based on provider
            if (provider == LLMProvider.OpenAI)
            {
                return $@"{{
                    ""model"": ""{model}"",
                    ""messages"": [
                        {{""role"": ""system"", ""content"": ""You are a helpful assistant that helps determine cat facial expressions based on human emotions and interactions."}},
                        {{""role"": ""user"", ""content"": ""{EscapeJson(prompt)}""}}
                    ],
                    ""temperature"": 0.7,
                    ""max_tokens"": 150
                }}";
            }
            else if (provider == LLMProvider.Anthropic)
            {
                return $@"{{
                    ""model"": ""{model}"",
                    ""max_tokens"": 150,
                    ""messages"": [
                        {{""role"": ""user"", ""content"": ""{EscapeJson(prompt)}""}}
                    ]
                }}";
            }

            return "";
        }

        private string ParseResponse(string jsonResponse)
        {
            try
            {
                // Simple JSON parsing - in production, use JsonUtility or Newtonsoft.Json
                if (jsonResponse.Contains("\"content\""))
                {
                    int startIndex = jsonResponse.IndexOf("\"content\"") + 11;
                    int endIndex = jsonResponse.IndexOf("\"", startIndex);
                    if (endIndex > startIndex)
                    {
                        return jsonResponse.Substring(startIndex, endIndex - startIndex);
                    }
                }
                else if (jsonResponse.Contains("\"text\""))
                {
                    int startIndex = jsonResponse.IndexOf("\"text\"") + 8;
                    int endIndex = jsonResponse.IndexOf("\"", startIndex);
                    if (endIndex > startIndex)
                    {
                        return jsonResponse.Substring(startIndex, endIndex - startIndex);
                    }
                }

                return jsonResponse;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing LLM response: {e.Message}");
                return jsonResponse;
            }
        }

        private string EscapeJson(string input)
        {
            return input.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n");
        }
    }
}

