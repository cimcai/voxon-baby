using UnityEngine;

namespace Voxon.Highlighting
{
    /// <summary>
    /// Base highlight effect class
    /// </summary>
    public abstract class HighlightEffect : MonoBehaviour
    {
        [Header("Effect Settings")]
        [SerializeField] protected float transitionDuration = 0.3f;
        [SerializeField] protected AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        protected bool isActive = false;
        protected float transitionProgress = 0f;

        /// <summary>
        /// Apply the highlight effect
        /// </summary>
        public abstract void ApplyHighlight(float intensity = 1f);

        /// <summary>
        /// Remove the highlight effect
        /// </summary>
        public abstract void RemoveHighlight();

        /// <summary>
        /// Update the effect intensity
        /// </summary>
        public abstract void UpdateIntensity(float intensity);
    }
}

