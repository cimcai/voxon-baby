using System.Collections.Generic;
using UnityEngine;
using Voxon.FaceDetection;
using Voxon.LLM;
using ExpressionType = Voxon.CatFace.ExpressionType;

namespace Voxon.CatFace
{
    /// <summary>
    /// Defines when expressions should trigger
    /// 
    /// Trigger logic is based on research on cat behavioral responses:
    /// - Cats respond to human gaze and attention (mutual attention studies)
    /// - Cats mirror or respond to human emotional expressions
    /// - Idle expressions occur naturally during periods of inactivity
    /// - Random expressions add naturalistic behavior variation
    /// 
    /// Research-informed trigger patterns:
    /// - Gaze-based: Cats show increased attention when being looked at
    /// - Expression-based: Cats respond to human emotional states
    /// - Time-based: Natural expression variation during idle periods
    /// 
    /// References:
    /// - CatFACS research on expression triggers and contexts
    /// - Studies on cat-human mutual attention and gaze interaction
    /// - Research on feline response patterns to human emotions
    /// </summary>
    public class ExpressionTriggers : MonoBehaviour
    {
        [Header("Trigger Settings")]
        [SerializeField] private float idleExpressionInterval = 5f;
        [SerializeField] private float randomExpressionChance = 0.1f;

        private ExpressionManager expressionManager;
        private GazeInteractionHandler gazeHandler;
        private ExpressionRecognizer expressionRecognizer;
        private LLMExpressionMapper llmMapper;
        private ContextManager contextManager;

        private float lastIdleExpressionTime = 0f;
        private float lastRandomExpressionTime = 0f;

        private void Start()
        {
            expressionManager = GetComponent<ExpressionManager>();
            gazeHandler = GetComponent<GazeInteractionHandler>();
            expressionRecognizer = FindObjectOfType<ExpressionRecognizer>();
            llmMapper = GetComponent<LLMExpressionMapper>();
            contextManager = FindObjectOfType<ContextManager>();
        }

        private void Update()
        {
            // Gaze-based triggers
            CheckGazeTriggers();

            // Human expression-based triggers
            CheckHumanExpressionTriggers();

            // LLM-suggested triggers
            CheckLLMTriggers();

            // Time-based triggers
            CheckTimeBasedTriggers();
        }

        private void CheckGazeTriggers()
        {
            if (gazeHandler == null) return;

            if (gazeHandler.IsBeingLookedAt())
            {
                float gazeDuration = gazeHandler.GetGazeDuration();
                
                if (gazeDuration > 0.5f && gazeDuration < 1f)
                {
                    // Short gaze - curious expression
                    expressionManager?.SetExpression(ExpressionType.Curious, 0.6f, 1f);
                }
                else if (gazeDuration > 2f)
                {
                    // Long gaze - focused expression
                    expressionManager?.SetExpression(ExpressionType.Focused, 0.8f, 2f);
                }
            }
            else
            {
                // Not being looked at - return to neutral after delay
                if (Time.time - lastIdleExpressionTime > idleExpressionInterval)
                {
                    expressionManager?.SetExpression(ExpressionType.Neutral, 0.3f, 1f);
                }
            }
        }

        private void CheckHumanExpressionTriggers()
        {
            if (expressionRecognizer == null) return;

            ExpressionType humanExpression = expressionRecognizer.GetCurrentExpression();

            switch (humanExpression)
            {
                case ExpressionType.Happy:
                    expressionManager?.SetExpression(ExpressionType.Happy, 0.7f, 2f);
                    break;
                case ExpressionType.Sad:
                    expressionManager?.SetExpression(ExpressionType.Sad, 0.6f, 2f);
                    break;
                case ExpressionType.Surprised:
                    expressionManager?.SetExpression(ExpressionType.Surprised, 0.8f, 1.5f);
                    break;
            }
        }

        private void CheckLLMTriggers()
        {
            if (llmMapper == null) return;

            // LLM mapper will handle triggering expressions based on LLM recommendations
            // This is called by the LLM system when responses are received
        }

        private void CheckTimeBasedTriggers()
        {
            // Idle expressions
            if (Time.time - lastIdleExpressionTime > idleExpressionInterval)
            {
                if (!gazeHandler.IsBeingLookedAt())
                {
                    expressionManager?.SetExpression(ExpressionType.Neutral, 0.3f, 1f);
                    lastIdleExpressionTime = Time.time;
                }
            }

            // Random expressions for natural behavior
            if (Time.time - lastRandomExpressionTime > 3f && Random.value < randomExpressionChance)
            {
                ExpressionType randomExpression = (ExpressionType)Random.Range(0, System.Enum.GetValues(typeof(ExpressionType)).Length);
                expressionManager?.SetExpression(randomExpression, 0.4f, 1f);
                lastRandomExpressionTime = Time.time;
            }
        }
    }
}

