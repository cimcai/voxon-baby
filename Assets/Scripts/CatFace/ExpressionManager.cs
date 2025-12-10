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

        [Header("Model References")]
        [SerializeField] private SkinnedMeshRenderer faceMeshRenderer;
        [SerializeField] private Animator faceAnimator;
        [SerializeField] private bool useBlendShapes = true;
        [SerializeField] private bool useAnimator = false;

        [Header("Blend Shape Mapping")]
        [SerializeField] private int[] eyeBlendShapeIndices = new int[3]; // 0: blink, 1: half-blink, 2: wide
        [SerializeField] private int[] earBlendShapeIndices = new int[3]; // 0: forward, 1: backward, 2: flattened
        [SerializeField] private int[] mouthBlendShapeIndices = new int[3]; // 0: open, 1: smile, 2: frown

        private CatFaceExpression currentExpression;
        private CatFaceExpression targetExpression;
        private CatFaceExpression startExpression;
        private Queue<CatFaceExpression> expressionQueue = new Queue<CatFaceExpression>();
        private bool isTransitioning = false;
        private float transitionProgress = 0f;

        // Store original blend shape values
        private float[] originalBlendShapeValues;

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

        private void Awake()
        {
            // Auto-find face mesh renderer if not assigned
            if (faceMeshRenderer == null)
            {
                faceMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            }

            // Auto-find animator if not assigned
            if (faceAnimator == null)
            {
                faceAnimator = GetComponent<Animator>();
            }

            // Store original blend shape values
            if (faceMeshRenderer != null && useBlendShapes)
            {
                int blendShapeCount = faceMeshRenderer.sharedMesh.blendShapeCount;
                originalBlendShapeValues = new float[blendShapeCount];
                for (int i = 0; i < blendShapeCount; i++)
                {
                    originalBlendShapeValues[i] = faceMeshRenderer.GetBlendShapeWeight(i);
                }
            }
        }

        private IEnumerator TransitionToExpression(CatFaceExpression expression)
        {
            isTransitioning = true;
            startExpression = currentExpression ?? new CatFaceExpression(ExpressionType.Neutral, 0f, 0f);
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
            if (targetExpression == null) return;

            // Blend between start and target expression
            float startIntensity = startExpression != null ? startExpression.intensity : 0f;
            float targetIntensity = Mathf.Lerp(startIntensity, targetExpression.intensity, blendFactor);

            if (useBlendShapes && faceMeshRenderer != null)
            {
                ApplyBlendShapes(targetExpression.expressionType, targetIntensity, blendFactor);
            }

            if (useAnimator && faceAnimator != null)
            {
                ApplyAnimatorParameters(targetExpression.expressionType, targetIntensity);
            }
        }

        private void ApplyBlendShapes(ExpressionType expressionType, float intensity, float blendFactor)
        {
            if (faceMeshRenderer == null || faceMeshRenderer.sharedMesh == null) return;

            // Reset all blend shapes first (optional - comment out if you want additive blending)
            // for (int i = 0; i < faceMeshRenderer.sharedMesh.blendShapeCount; i++)
            // {
            //     faceMeshRenderer.SetBlendShapeWeight(i, originalBlendShapeValues[i]);
            // }

            // Apply expression-specific blend shapes based on CatFACS research
            switch (expressionType)
            {
                case ExpressionType.Happy:
                    // Relaxed eyes, forward ears, relaxed mouth
                    SetBlendShapeWeight(eyeBlendShapeIndices[0], 0f); // No blink
                    SetBlendShapeWeight(earBlendShapeIndices[0], intensity * 100f * blendFactor); // Forward ears
                    SetBlendShapeWeight(mouthBlendShapeIndices[1], intensity * 50f * blendFactor); // Slight smile
                    break;

                case ExpressionType.Curious:
                    // Wide eyes, forward ears, alert expression
                    SetBlendShapeWeight(eyeBlendShapeIndices[2], intensity * 80f * blendFactor); // Wide eyes
                    SetBlendShapeWeight(earBlendShapeIndices[0], intensity * 100f * blendFactor); // Forward ears
                    break;

                case ExpressionType.Surprised:
                    // Very wide eyes, raised ears
                    SetBlendShapeWeight(eyeBlendShapeIndices[2], intensity * 100f * blendFactor); // Very wide eyes
                    SetBlendShapeWeight(earBlendShapeIndices[0], intensity * 100f * blendFactor); // Raised ears
                    SetBlendShapeWeight(mouthBlendShapeIndices[0], intensity * 30f * blendFactor); // Slightly open mouth
                    break;

                case ExpressionType.Sleepy:
                    // Half-closed eyes, relaxed ears
                    SetBlendShapeWeight(eyeBlendShapeIndices[1], intensity * 100f * blendFactor); // Half-blink
                    break;

                case ExpressionType.Playful:
                    // Alert eyes, forward ears, open mouth
                    SetBlendShapeWeight(eyeBlendShapeIndices[2], intensity * 60f * blendFactor); // Alert eyes
                    SetBlendShapeWeight(earBlendShapeIndices[0], intensity * 100f * blendFactor); // Forward ears
                    SetBlendShapeWeight(mouthBlendShapeIndices[0], intensity * 70f * blendFactor); // Open mouth
                    break;

                case ExpressionType.Focused:
                    // Alert eyes, forward ears, still expression
                    SetBlendShapeWeight(eyeBlendShapeIndices[2], intensity * 40f * blendFactor); // Alert but not wide
                    SetBlendShapeWeight(earBlendShapeIndices[0], intensity * 80f * blendFactor); // Forward ears
                    break;

                case ExpressionType.Sad:
                    // Narrowed eyes, backward/flattened ears, frown
                    SetBlendShapeWeight(eyeBlendShapeIndices[1], intensity * 60f * blendFactor); // Half-closed
                    SetBlendShapeWeight(earBlendShapeIndices[2], intensity * 100f * blendFactor); // Flattened ears
                    SetBlendShapeWeight(mouthBlendShapeIndices[2], intensity * 70f * blendFactor); // Frown
                    break;

                case ExpressionType.Neutral:
                default:
                    // Reset to neutral - blend back to original values
                    for (int i = 0; i < faceMeshRenderer.sharedMesh.blendShapeCount; i++)
                    {
                        float currentWeight = faceMeshRenderer.GetBlendShapeWeight(i);
                        float targetWeight = originalBlendShapeValues != null && i < originalBlendShapeValues.Length 
                            ? originalBlendShapeValues[i] 
                            : 0f;
                        faceMeshRenderer.SetBlendShapeWeight(i, Mathf.Lerp(currentWeight, targetWeight, blendFactor));
                    }
                    break;
            }
        }

        private void SetBlendShapeWeight(int index, float weight)
        {
            if (index >= 0 && index < faceMeshRenderer.sharedMesh.blendShapeCount)
            {
                faceMeshRenderer.SetBlendShapeWeight(index, Mathf.Clamp(weight, 0f, 100f));
            }
        }

        private void ApplyAnimatorParameters(ExpressionType expressionType, float intensity)
        {
            if (faceAnimator == null) return;

            // Set animator parameters for expression-based animations
            // This assumes your Animator Controller has parameters like "ExpressionType" (int) and "Intensity" (float)
            faceAnimator.SetInteger("ExpressionType", (int)expressionType);
            faceAnimator.SetFloat("Intensity", intensity);
        }
    }
}

