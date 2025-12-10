using System.Collections.Generic;
using UnityEngine;
using Voxon.FaceDetection;

namespace Voxon.LLM
{
    /// <summary>
    /// Handles evolving cat responses over time
    /// </summary>
    [System.Serializable]
    public class ResponseEffectiveness
    {
        public ExpressionType humanExpression;
        public string catResponse;
        public int successCount;
        public int totalCount;
        public float effectiveness => totalCount > 0 ? (float)successCount / totalCount : 0f;
    }

    public class ResponseEvolution : MonoBehaviour
    {
        [Header("Evolution Settings")]
        [SerializeField] private float evolutionRate = 0.1f;
        [SerializeField] private int minInteractionsForEvolution = 5;

        private ContextManager contextManager;
        private List<ResponseEffectiveness> responseDatabase = new List<ResponseEffectiveness>();
        private Dictionary<string, float> responseWeights = new Dictionary<string, float>();

        private void Start()
        {
            contextManager = GetComponent<ContextManager>();
            if (contextManager == null)
            {
                contextManager = FindObjectOfType<ContextManager>();
            }
        }

        /// <summary>
        /// Record the effectiveness of a response
        /// </summary>
        public void RecordResponse(ExpressionType humanExpression, string catResponse, bool wasEffective)
        {
            ResponseEffectiveness response = FindOrCreateResponse(humanExpression, catResponse);
            
            response.totalCount++;
            if (wasEffective)
            {
                response.successCount++;
            }

            UpdateResponseWeight(catResponse, response.effectiveness);
        }

        /// <summary>
        /// Get the best response for a given human expression
        /// </summary>
        public string GetBestResponse(ExpressionType humanExpression)
        {
            ResponseEffectiveness bestResponse = null;
            float bestScore = -1f;

            foreach (var response in responseDatabase)
            {
                if (response.humanExpression == humanExpression)
                {
                    float score = response.effectiveness * GetResponseWeight(response.catResponse);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestResponse = response;
                    }
                }
            }

            return bestResponse != null ? bestResponse.catResponse : "";
        }

        /// <summary>
        /// Check if enough data exists to evolve responses
        /// </summary>
        public bool CanEvolve()
        {
            return responseDatabase.Count >= minInteractionsForEvolution;
        }

        private ResponseEffectiveness FindOrCreateResponse(ExpressionType humanExpression, string catResponse)
        {
            foreach (var response in responseDatabase)
            {
                if (response.humanExpression == humanExpression && response.catResponse == catResponse)
                {
                    return response;
                }
            }

            ResponseEffectiveness newResponse = new ResponseEffectiveness
            {
                humanExpression = humanExpression,
                catResponse = catResponse,
                successCount = 0,
                totalCount = 0
            };

            responseDatabase.Add(newResponse);
            return newResponse;
        }

        private void UpdateResponseWeight(string response, float effectiveness)
        {
            if (!responseWeights.ContainsKey(response))
            {
                responseWeights[response] = 1f;
            }

            // Evolve weight based on effectiveness
            float targetWeight = effectiveness;
            responseWeights[response] = Mathf.Lerp(responseWeights[response], targetWeight, evolutionRate);
        }

        private float GetResponseWeight(string response)
        {
            return responseWeights.ContainsKey(response) ? responseWeights[response] : 1f;
        }
    }
}

