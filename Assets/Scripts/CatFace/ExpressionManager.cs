using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxon.CatFace
{
    /// <summary>
    /// Manages facial expression transitions
    /// 
    /// Expression transitions are informed by research on cat facial expression dynamics:
    /// - CatFACS studies show that facial expressions change gradually, not instantaneously
    /// - Expression blending allows for more natural, realistic transitions between emotional states
    /// - Duration and intensity parameters are based on observed cat behavior patterns
    /// 
    /// References:
    /// - Caeiro et al. (2017). CatFACS development and application
    /// - Research on temporal dynamics of feline facial expressions
    /// </summary>
    public class ExpressionManager : MonoBehaviour
    {
        [Header("Expression Settings")]
        [SerializeField] private float defaultTransitionDuration = 0.5f;
        [SerializeField] private bool allowExpressionBlending = true;

        private CatFaceExpression currentExpression;
        private CatFaceExpression targetExpression;
        private Queue<CatFaceExpression> expressionQueue = new Queue<CatFaceExpression>();
        private bool isTransitioning = false;
        private float transitionProgress = 0f;

        public event System.Action<ExpressionType> OnExpressionChanged;

        /// <summary>
        /// Set a new expression
        /// </summary>
        public void SetExpression(ExpressionType expressionType, float intensity = 0.5f, float duration = 2f)
        {
            CatFaceExpression newExpression = new CatFaceExpression(expressionType, intensity, duration);
            SetExpression(newExpression);
        }

        /// <summary>
        /// Set a new expression with full control
        /// </summary>
        public void SetExpression(CatFaceExpression expression)
        {
            if (isTransitioning && allowExpressionBlending)
            {
                // Queue the expression
                expressionQueue.Enqueue(expression);
            }
            else
            {
                StartCoroutine(TransitionToExpression(expression));
            }
        }

        /// <summary>
        /// Get the current expression type
        /// </summary>
        public ExpressionType GetCurrentExpressionType()
        {
            return currentExpression != null ? currentExpression.expressionType : ExpressionType.Neutral;
        }

        private IEnumerator TransitionToExpression(CatFaceExpression expression)
        {
            isTransitioning = true;
            targetExpression = expression;
            transitionProgress = 0f;

            float transitionDuration = defaultTransitionDuration;

            while (transitionProgress < 1f)
            {
                transitionProgress += Time.deltaTime / transitionDuration;
                transitionProgress = Mathf.Clamp01(transitionProgress);

                // Apply blended expression
                ApplyExpressionBlend(transitionProgress);

                yield return null;
            }

            currentExpression = targetExpression;
            isTransitioning = false;
            OnExpressionChanged?.Invoke(currentExpression.expressionType);

            // Process queued expressions
            if (expressionQueue.Count > 0)
            {
                CatFaceExpression nextExpression = expressionQueue.Dequeue();
                StartCoroutine(TransitionToExpression(nextExpression));
            }
        }

        private void ApplyExpressionBlend(float blendFactor)
        {
            // This would apply blend shapes or animation parameters
            // Implementation depends on the cat face model structure
            // For now, this is a placeholder that would be implemented based on the actual model
        }
    }
}

