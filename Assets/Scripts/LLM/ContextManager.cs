using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Voxon.FaceDetection;

namespace Voxon.LLM
{
    /// <summary>
    /// Manages interaction context and history
    /// </summary>
    [Serializable]
    public class InteractionContext
    {
        public float timestamp;
        public ExpressionType humanExpression;
        public string catResponse;
        public string notes;

        public InteractionContext(ExpressionType expression, string response, string notes = "")
        {
            timestamp = Time.time;
            humanExpression = expression;
            catResponse = response;
            this.notes = notes;
        }
    }

    public class ContextManager : MonoBehaviour
    {
        [Header("Context Settings")]
        [SerializeField] private int maxHistorySize = 50;
        [SerializeField] private float contextWindowMinutes = 10f;

        private List<InteractionContext> interactionHistory = new List<InteractionContext>();
        private Dictionary<ExpressionType, int> expressionFrequency = new Dictionary<ExpressionType, int>();

        /// <summary>
        /// Add an interaction to the context history
        /// </summary>
        public void AddInteraction(ExpressionType humanExpression, string catResponse, string notes = "")
        {
            InteractionContext context = new InteractionContext(humanExpression, catResponse, notes);
            interactionHistory.Add(context);

            // Update frequency tracking
            if (!expressionFrequency.ContainsKey(humanExpression))
            {
                expressionFrequency[humanExpression] = 0;
            }
            expressionFrequency[humanExpression]++;

            // Trim old history
            TrimHistory();
        }

        /// <summary>
        /// Get a summary of recent context
        /// </summary>
        public string GetContextSummary(int maxEntries = 10)
        {
            if (interactionHistory.Count == 0)
            {
                return "No interaction history yet.";
            }

            StringBuilder summary = new StringBuilder();
            int startIndex = Mathf.Max(0, interactionHistory.Count - maxEntries);

            for (int i = startIndex; i < interactionHistory.Count; i++)
            {
                InteractionContext context = interactionHistory[i];
                summary.AppendLine($"- Human: {context.humanExpression}, Cat: {context.catResponse}");
            }

            return summary.ToString();
        }

        /// <summary>
        /// Get expression frequency trends
        /// </summary>
        public Dictionary<ExpressionType, float> GetExpressionTrends()
        {
            Dictionary<ExpressionType, float> trends = new Dictionary<ExpressionType, float>();
            int totalInteractions = interactionHistory.Count;

            if (totalInteractions == 0)
            {
                return trends;
            }

            foreach (var kvp in expressionFrequency)
            {
                trends[kvp.Key] = (float)kvp.Value / totalInteractions;
            }

            return trends;
        }

        /// <summary>
        /// Clear old history outside the context window
        /// </summary>
        private void TrimHistory()
        {
            float cutoffTime = Time.time - (contextWindowMinutes * 60f);
            
            interactionHistory.RemoveAll(ctx => ctx.timestamp < cutoffTime);

            // Trim to max size if still too large
            if (interactionHistory.Count > maxHistorySize)
            {
                int removeCount = interactionHistory.Count - maxHistorySize;
                interactionHistory.RemoveRange(0, removeCount);
            }
        }

        /// <summary>
        /// Clear all history
        /// </summary>
        public void ClearHistory()
        {
            interactionHistory.Clear();
            expressionFrequency.Clear();
        }
    }
}

