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
        [SerializeField] private bool debugLogging = false;

        private Renderer targetRenderer;
        private Material material;
        private Color originalColor = Color.white; // Initialize to white instead of clear
        private Color originalEmissionColor;
        private bool hasEmission;
        private bool originalColorStored = false;

        private void Awake()
        {
            InitializeMaterial();
        }

        private void Start()
        {
            // Re-initialize in Start in case material was created after Awake
            if (material == null)
            {
                InitializeMaterial();
            }
        }

        private void InitializeMaterial()
        {
            targetRenderer = GetComponent<Renderer>();
            if (targetRenderer != null)
            {
                // Use material property to get/create material instance
                material = targetRenderer.material;
                if (material != null)
                {
                    if (!originalColorStored)
                    {
                        originalColor = material.color;
                        originalColorStored = true;
                    }
                    if (material.HasProperty("_EmissionColor"))
                    {
                        hasEmission = true;
                        originalEmissionColor = material.GetColor("_EmissionColor");
                    }
                    
                    if (debugLogging)
                    {
                        Debug.Log($"ColorHighlight.InitializeMaterial: {gameObject.name} - Material: {material.name}, Original Color: {originalColor}");
                    }
                }
                else
                {
                    if (debugLogging)
                    {
                        Debug.LogWarning($"ColorHighlight.InitializeMaterial: {gameObject.name} - Renderer found but material is null!");
                    }
                }
            }
            else
            {
                if (debugLogging)
                {
                    Debug.LogWarning($"ColorHighlight.InitializeMaterial: {gameObject.name} - No Renderer component found!");
                }
            }
        }

        public override void ApplyHighlight(float intensity = 1f)
        {
            // Ensure we have a valid material - re-initialize if needed
            if (targetRenderer == null)
            {
                InitializeMaterial();
            }
            
            // Get fresh material reference in case it was recreated
            if (targetRenderer != null)
            {
                material = targetRenderer.material;
                if (material != null && !originalColorStored)
                {
                    // Store original color if not already stored
                    originalColor = material.color;
                    originalColorStored = true;
                }
            }

            if (material == null)
            {
                if (debugLogging)
                {
                    Debug.LogWarning($"ColorHighlight: No material found on {gameObject.name}. Renderer: {targetRenderer != null}");
                }
                return;
            }

            isActive = true;
            Color targetColor = Color.Lerp(originalColor, highlightColor, intensity);
            material.color = targetColor;

            if (debugLogging)
            {
                Debug.Log($"ColorHighlight: Applied highlight to {gameObject.name} - Intensity: {intensity}, Color: {targetColor}");
            }

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

