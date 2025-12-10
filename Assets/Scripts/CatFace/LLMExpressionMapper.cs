using UnityEngine;
using Voxon.LLM;
using Voxon.FaceDetection;

namespace Voxon.CatFace
{
    /// <summary>
    /// Maps LLM responses to cat expressions
    /// 
    /// Expression mapping is informed by research on cat responses to human emotions:
    /// - Cats show sensitivity to human emotional states and can mirror or respond to them
    /// - Research indicates cats may display empathetic responses to human distress
    /// - Positive human expressions often elicit positive cat responses (relaxed, forward ears)
    /// - Negative human expressions may elicit concern or withdrawal in cats
    /// 
    /// The mapping system translates LLM-generated emotional context into appropriate
    /// cat facial expressions based on CatFACS research and observed cat behavior.
    /// 
    /// References:
    /// - CatFACS research on emotion-expression relationships
    /// - Studies on cat-human emotional communication
    /// - Research on feline empathy and social responses
    /// </summary>
    public class LLMExpressionMapper : MonoBehaviour
    {
        [Header("Mapping Settings")]
        [SerializeField] private bool autoApplyLLMResponses = true;

        private ExpressionManager expressionManager;
        private LLMResponseParser responseParser;
        private LLMClient llmClient;
        private PromptBuilder promptBuilder;
        private ContextManager contextManager;

        private void Start()
        {
            expressionManager = GetComponent<ExpressionManager>();
            responseParser = GetComponent<LLMResponseParser>();
            if (responseParser == null)
            {
                responseParser = FindObjectOfType<LLMResponseParser>();
            }

            llmClient = FindObjectOfType<LLMClient>();
            promptBuilder = FindObjectOfType<PromptBuilder>();
            contextManager = FindObjectOfType<ContextManager>();

            // Subscribe to LLM responses
            if (llmClient != null)
            {
                llmClient.OnResponseReceived += HandleLLMResponse;
            }
        }

        /// <summary>
        /// Request an expression recommendation from LLM
        /// </summary>
        public void RequestExpressionRecommendation()
        {
            if (promptBuilder == null || llmClient == null)
            {
                Debug.LogWarning("PromptBuilder or LLMClient not found. Cannot request expression.");
                return;
            }

            string prompt = promptBuilder.BuildPrompt();
            llmClient.SendRequest(prompt);
        }

        private void HandleLLMResponse(string response)
        {
            if (responseParser == null)
            {
                Debug.LogWarning("LLMResponseParser not found. Cannot parse response.");
                return;
            }

            LLMExpressionResponse llmResponse = responseParser.ParseResponse(response);

            if (autoApplyLLMResponses && expressionManager != null)
            {
                // Map LLM expression to cat expression
                ExpressionType catExpression = MapToCatExpression(llmResponse.expression);
                expressionManager.SetExpression(catExpression, llmResponse.intensity, llmResponse.duration);

                // Record interaction in context
                if (contextManager != null)
                {
                    ExpressionRecognizer recognizer = FindObjectOfType<ExpressionRecognizer>();
                    FaceDetection.ExpressionType humanExpression = recognizer != null ? recognizer.GetCurrentExpression() : FaceDetection.ExpressionType.Neutral;
                    // Map human expression to cat expression for context
                    ExpressionType mappedCatExpression = MapToCatExpression(humanExpression);
                    contextManager.AddInteraction(mappedCatExpression, catExpression.ToString(), llmResponse.reasoning);
                }
            }
        }

        private ExpressionType MapToCatExpression(FaceDetection.ExpressionType llmExpression)
        {
            // Map human expression types to cat expression types
            // Some expressions map directly, others need translation
            switch (llmExpression)
            {
                case FaceDetection.ExpressionType.Happy:
                    return ExpressionType.Happy;
                case FaceDetection.ExpressionType.Sad:
                    return ExpressionType.Sad;
                case FaceDetection.ExpressionType.Surprised:
                    return ExpressionType.Surprised;
                case FaceDetection.ExpressionType.Neutral:
                    return ExpressionType.Neutral;
                case FaceDetection.ExpressionType.Excited:
                    return ExpressionType.Playful;
                case FaceDetection.ExpressionType.Confused:
                    return ExpressionType.Curious;
                case FaceDetection.ExpressionType.Angry:
                    return ExpressionType.Sad; // Cat shows concern for angry human
                default:
                    return ExpressionType.Neutral;
            }
        }

        private void OnDestroy()
        {
            if (llmClient != null)
            {
                llmClient.OnResponseReceived -= HandleLLMResponse;
            }
        }
    }
}

