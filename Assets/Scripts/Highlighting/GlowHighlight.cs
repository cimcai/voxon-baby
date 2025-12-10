using UnityEngine;

namespace Voxon.Highlighting
{
    /// <summary>
    /// Glow/emission highlight effect
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class GlowHighlight : HighlightEffect
    {
        [Header("Glow Settings")]
        [SerializeField] private Color glowColor = Color.yellow;
        [SerializeField] private float baseEmissionIntensity = 1f;
        [SerializeField] private float maxEmissionIntensity = 5f;

        private Renderer targetRenderer;
        private Material material;
        private Color originalEmissionColor;
        private bool hasEmission;

        private void Awake()
        {
            targetRenderer = GetComponent<Renderer>();
            if (targetRenderer != null)
            {
                material = targetRenderer.material;
                if (material != null && material.HasProperty("_EmissionColor"))
                {
                    hasEmission = true;
                    originalEmissionColor = material.GetColor("_EmissionColor");
                }
            }
        }

        public override void ApplyHighlight(float intensity = 1f)
        {
            if (material == null || !hasEmission) return;

            isActive = true;
            float emissionIntensity = Mathf.Lerp(baseEmissionIntensity, maxEmissionIntensity, intensity);
            material.SetColor("_EmissionColor", glowColor * emissionIntensity);
            material.EnableKeyword("_EMISSION");
        }

        public override void RemoveHighlight()
        {
            if (material == null || !hasEmission) return;

            isActive = false;
            material.SetColor("_EmissionColor", originalEmissionColor);
            if (originalEmissionColor == Color.black)
            {
                material.DisableKeyword("_EMISSION");
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

