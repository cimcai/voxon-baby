using UnityEngine;

namespace Voxon.Highlighting
{
    /// <summary>
    /// Outline highlight effect using a second renderer
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class OutlineHighlight : HighlightEffect
    {
        [Header("Outline Settings")]
        [SerializeField] private Color outlineColor = Color.yellow;
        [SerializeField] private float outlineWidth = 0.1f;
        [SerializeField] private Material outlineMaterial;

        private Renderer targetRenderer;
        private Renderer outlineRenderer;
        private GameObject outlineObject;

        private void Awake()
        {
            targetRenderer = GetComponent<Renderer>();
            CreateOutline();
        }

        private void CreateOutline()
        {
            if (targetRenderer == null) return;

            // Create outline object
            outlineObject = new GameObject("Outline");
            outlineObject.transform.SetParent(transform);
            outlineObject.transform.localPosition = Vector3.zero;
            outlineObject.transform.localRotation = Quaternion.identity;
            outlineObject.transform.localScale = Vector3.one * (1f + outlineWidth);

            // Copy mesh
            MeshFilter originalFilter = GetComponent<MeshFilter>();
            if (originalFilter != null && originalFilter.sharedMesh != null)
            {
                MeshFilter outlineFilter = outlineObject.AddComponent<MeshFilter>();
                outlineFilter.sharedMesh = originalFilter.sharedMesh;
            }

            // Add renderer with outline material
            outlineRenderer = outlineObject.AddComponent<MeshRenderer>();
            
            if (outlineMaterial == null)
            {
                // Create a simple outline material
                outlineMaterial = new Material(Shader.Find("Standard"));
                outlineMaterial.SetColor("_Color", outlineColor);
                outlineMaterial.SetFloat("_Metallic", 0f);
                outlineMaterial.SetFloat("_Glossiness", 0f);
            }

            outlineRenderer.material = outlineMaterial;
            outlineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            outlineRenderer.receiveShadows = false;

            // Disable by default
            outlineObject.SetActive(false);
        }

        public override void ApplyHighlight(float intensity = 1f)
        {
            if (outlineObject != null)
            {
                isActive = true;
                outlineObject.SetActive(true);
                
                if (outlineMaterial != null)
                {
                    Color color = outlineColor;
                    color.a = intensity;
                    outlineMaterial.SetColor("_Color", color);
                }
            }
        }

        public override void RemoveHighlight()
        {
            if (outlineObject != null)
            {
                isActive = false;
                outlineObject.SetActive(false);
            }
        }

        public override void UpdateIntensity(float intensity)
        {
            if (isActive && outlineMaterial != null)
            {
                Color color = outlineColor;
                color.a = intensity;
                outlineMaterial.SetColor("_Color", color);
            }
        }

        private void OnDestroy()
        {
            if (outlineObject != null)
            {
                Destroy(outlineObject);
            }
        }
    }
}

