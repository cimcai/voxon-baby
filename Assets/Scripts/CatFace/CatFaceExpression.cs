using UnityEngine;

namespace Voxon.CatFace
{
    /// <summary>
    /// Base class for individual expressions
    /// 
    /// Expression parameters are based on CatFACS (Cat Facial Action Coding System) research:
    /// - Caeiro, C. C., Burrows, A. M., & Waller, B. M. (2017). Development and application of CatFACS:
    ///   Are human cat adopters influenced by cat facial expressions? Applied Animal Behaviour Science, 
    ///   173, 42-48. https://doi.org/10.1016/j.applanim.2015.11.003
    /// 
    /// Blend shape values correspond to facial action units (AUs) identified in CatFACS:
    /// - Eye region: Blinking, half-blinking, eye widening
    /// - Ear position: Forward, backward, flattened
    /// - Mouth: Lip parting, jaw dropping, tongue showing
    /// - Nose: Wrinkling, upper lip raising
    /// </summary>
    [System.Serializable]
    public class CatFaceExpression
    {
        public ExpressionType expressionType;
        public float intensity;
        public float duration;
        public AnimationCurve blendCurve;

        // Blend shape values or animation parameters
        public float[] blendShapeValues;

        public CatFaceExpression(ExpressionType type, float intens = 0.5f, float dur = 2f)
        {
            expressionType = type;
            intensity = Mathf.Clamp01(intens);
            duration = dur;
            blendCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            blendShapeValues = new float[10]; // Adjust based on model
        }

        /// <summary>
        /// Get blend shape value at normalized time (0-1)
        /// </summary>
        public float GetBlendValue(int index, float normalizedTime)
        {
            if (index < 0 || index >= blendShapeValues.Length)
                return 0f;

            float curveValue = blendCurve.Evaluate(normalizedTime);
            return blendShapeValues[index] * intensity * curveValue;
        }
    }
}

