using UnityEngine;
using Voxon.Highlighting;

namespace Voxon.VolumetricShapes
{
    /// <summary>
    /// Base MonoBehaviour class for all volumetric shapes
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class VolumetricShape : MonoBehaviour, IHighlightable
    {
        [Header("Shape Properties")]
        [SerializeField] protected Color baseColor = Color.white;
        [SerializeField] protected float size = 1f;

        protected bool isFocused = false;
        protected bool isHighlighted = false;
        protected HighlightController highlightController;

        public bool IsFocused => isFocused;
        public bool IsHighlighted => isHighlighted;

        protected virtual void Awake()
        {
            highlightController = GetComponent<HighlightController>();
            if (highlightController == null)
            {
                highlightController = gameObject.AddComponent<HighlightController>();
            }

            // Ensure collider exists
            if (GetComponent<Collider>() == null)
            {
                gameObject.AddComponent<BoxCollider>();
            }
        }

        protected virtual void Start()
        {
            ApplyBaseColor();
        }

        public virtual void SetFocused(bool focused)
        {
            isFocused = focused;
            if (highlightController != null)
            {
                highlightController.SetFocused(focused);
            }
        }

        public virtual void SetHighlighted(bool highlighted)
        {
            isHighlighted = highlighted;
            if (highlightController != null)
            {
                highlightController.SetHighlighted(highlighted);
            }
        }

        protected virtual void ApplyBaseColor()
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                renderer.material.color = baseColor;
            }
        }

        protected virtual void OnValidate()
        {
            if (Application.isPlaying)
            {
                ApplyBaseColor();
            }
        }
    }
}

