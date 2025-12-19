using System.Collections.Generic;
using UnityEngine;

namespace Voxon.Highlighting
{
    /// <summary>
    /// Central manager for highlight effects
    /// </summary>
    public class HighlightController : MonoBehaviour
    {
        [Header("Highlight Settings")]
        [SerializeField] private bool useColorHighlight = true;
        [SerializeField] private bool useOutlineHighlight = false;
        [SerializeField] private bool useGlowHighlight = false;
        [SerializeField] private float highlightIntensity = 1f;

        private List<HighlightEffect> highlightEffects = new List<HighlightEffect>();
        private bool isFocused = false;
        private bool isHighlighted = false;

        private void Awake()
        {
            InitializeEffects();
        }

        private void Start()
        {
            // Re-initialize effects in Start in case components were added after Awake
            // This ensures ColorHighlight gets the material that might be created in VoxelCube.Start()
            InitializeEffects();
        }

        private void InitializeEffects()
        {
            // Add effects based on settings
            if (useColorHighlight)
            {
                ColorHighlight colorHighlight = GetComponent<ColorHighlight>();
                if (colorHighlight == null)
                {
                    colorHighlight = gameObject.AddComponent<ColorHighlight>();
                }
                highlightEffects.Add(colorHighlight);
            }

            if (useGlowHighlight)
            {
                GlowHighlight glowHighlight = GetComponent<GlowHighlight>();
                if (glowHighlight == null)
                {
                    glowHighlight = gameObject.AddComponent<GlowHighlight>();
                }
                highlightEffects.Add(glowHighlight);
            }

            if (useOutlineHighlight)
            {
                OutlineHighlight outlineHighlight = GetComponent<OutlineHighlight>();
                if (outlineHighlight == null)
                {
                    outlineHighlight = gameObject.AddComponent<OutlineHighlight>();
                }
                highlightEffects.Add(outlineHighlight);
            }
        }

        /// <summary>
        /// Set focused state (pre-highlight)
        /// </summary>
        public void SetFocused(bool focused)
        {
            isFocused = focused;
            if (focused && !isHighlighted)
            {
                // Show subtle highlight immediately when focused (before full highlight)
                ApplyFocusedHighlight();
            }
            else if (!focused && !isHighlighted)
            {
                RemoveAllHighlights();
            }
        }

        private void ApplyFocusedHighlight()
        {
            // Apply a subtle highlight (50% intensity) when focused
            foreach (var effect in highlightEffects)
            {
                if (effect != null)
                {
                    effect.ApplyHighlight(highlightIntensity * 0.5f);
                }
            }
        }

        /// <summary>
        /// Set highlighted state (full highlight)
        /// </summary>
        public void SetHighlighted(bool highlighted)
        {
            isHighlighted = highlighted;
            if (highlighted)
            {
                ApplyAllHighlights();
            }
            else
            {
                RemoveAllHighlights();
            }
        }

        private void ApplyAllHighlights()
        {
            foreach (var effect in highlightEffects)
            {
                if (effect != null)
                {
                    effect.ApplyHighlight(highlightIntensity);
                }
            }
        }

        private void RemoveAllHighlights()
        {
            foreach (var effect in highlightEffects)
            {
                if (effect != null)
                {
                    effect.RemoveHighlight();
                }
            }
        }
    }
}

