using System.Text;
using UnityEngine;
using Voxon.FaceDetection;
using Voxon.EyeTracker;

namespace Voxon.LLM
{
    /// <summary>
    /// Constructs prompts for LLM based on context
    /// </summary>
    public class PromptBuilder : MonoBehaviour
    {
        [Header("Prompt Settings")]
        [SerializeField] private string systemPrompt = "You are a cat character that responds to human emotions and interactions. Determine the most appropriate cat facial expression based on the context provided.";
        [SerializeField] private int maxContextHistory = 10;

        private ContextManager contextManager;
        private ExpressionRecognizer expressionRecognizer;
        private EyeTrackerManager eyeTrackerManager;

        private void Start()
        {
            contextManager = GetComponent<ContextManager>();
            if (contextManager == null)
            {
                contextManager = FindObjectOfType<ContextManager>();
            }

            expressionRecognizer = FindObjectOfType<ExpressionRecognizer>();
            eyeTrackerManager = EyeTrackerManager.Instance;
        }

        /// <summary>
        /// Build a prompt for the LLM based on current context
        /// </summary>
        public string BuildPrompt()
        {
            StringBuilder prompt = new StringBuilder();
            
            // System prompt
            prompt.AppendLine(systemPrompt);
            prompt.AppendLine();

            // Current human expression
            if (expressionRecognizer != null)
            {
                ExpressionType currentExpression = expressionRecognizer.GetCurrentExpression();
                prompt.AppendLine($"Current human expression: {currentExpression}");
            }

            // Interaction history
            if (contextManager != null)
            {
                string history = contextManager.GetContextSummary(maxContextHistory);
                if (!string.IsNullOrEmpty(history))
                {
                    prompt.AppendLine("Recent interaction history:");
                    prompt.AppendLine(history);
                }
            }

            // Gaze information
            if (eyeTrackerManager != null && eyeTrackerManager.IsConnected())
            {
                GazeData gazeData = eyeTrackerManager.GetGazeData();
                if (gazeData.isValid)
                {
                    prompt.AppendLine($"User is actively looking around (gaze detected)");
                }
            }

            // Request for expression recommendation
            prompt.AppendLine();
            prompt.AppendLine("Based on this context, what cat facial expression should be displayed? Respond with JSON format: {\"expression\": \"expression_name\", \"intensity\": 0.0-1.0, \"duration\": seconds, \"reasoning\": \"brief explanation\"}");

            return prompt.ToString();
        }

        /// <summary>
        /// Build a prompt with specific focus
        /// </summary>
        public string BuildPrompt(string focus)
        {
            string basePrompt = BuildPrompt();
            return $"{basePrompt}\n\nAdditional context: {focus}";
        }
    }
}

