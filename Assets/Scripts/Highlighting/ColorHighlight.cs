using UnityEngine;

namespace Voxon.Highlighting
{
    /// <summary>
    /// Color change highlight effect
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class ColorHighlight : HighlightEffect
    {
        [Header("Color Settings")]
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private bool useEmission = false;

        private Renderer targetRenderer;
        private Material material;
        private Color originalColor;
        private Color originalEmissionColor;
        private bool hasEmission;

        private void Awake()
        {
            targetRenderer = GetComponent<Renderer>();
            if (targetRenderer != null)
            {
                material = targetRenderer.material;
                if (material != null)
                {
                    originalColor = material.color;
                    if (material.HasProperty("_EmissionColor"))
                    {
                        hasEmission = true;
                        originalEmissionColor = material.GetColor("_EmissionColor");
                    }
                }
            }
        }

        public override void ApplyHighlight(float intensity = 1f)
        {
            if (material == null) return;

            isActive = true;
            Color targetColor = Color.Lerp(originalColor, highlightColor, intensity);
            material.color = targetColor;

            if (useEmission && hasEmission)
            {
                material.SetColor("_EmissionColor", highlightColor * intensity);
                material.EnableKeyword("_EMISSION");
            }
        }

        public override void RemoveHighlight()
        {
            if (material == null) return;

            isActive = false;
            material.color = originalColor;

            if (useEmission && hasEmission)
            {
                material.SetColor("_EmissionColor", originalEmissionColor);
                if (originalEmissionColor == Color.black)
                {
                    material.DisableKeyword("_EMISSION");
                }
            }
        }

        public override void UpdateIntensity(float intensity)
        {
            if (isActive)
            {
                ApplyHighlight(intensity);
            }
        }
    }
}

