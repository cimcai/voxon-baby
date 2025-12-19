using UnityEngine;
using Voxon.VolumetricShapes;
using Voxon.Highlighting;

namespace Voxon.Utilities
{
    /// <summary>
    /// Simple test script to verify highlighting works
    /// Attach to a cube and press T to test highlight
    /// </summary>
    public class HighlightTester : MonoBehaviour
    {
        private VolumetricShape shape;
        private HighlightController highlightController;
        private ColorHighlight colorHighlight;

        private void Start()
        {
            shape = GetComponent<VolumetricShape>();
            highlightController = GetComponent<HighlightController>();
            colorHighlight = GetComponent<ColorHighlight>();

            Debug.Log($"HighlightTester on {gameObject.name}:");
            Debug.Log($"  VolumetricShape: {shape != null}");
            Debug.Log($"  HighlightController: {highlightController != null}");
            Debug.Log($"  ColorHighlight: {colorHighlight != null}");
            
            if (GetComponent<Renderer>() != null)
            {
                var renderer = GetComponent<Renderer>();
                Debug.Log($"  Renderer: {renderer != null}, Material: {renderer.material != null}, Color: {renderer.material.color}");
            }
        }

        private void Update()
        {
            // Press T to test highlight
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("=== Testing Highlight ===");
                
                // Always try direct material first as a baseline test
                var renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    if (renderer.material != null)
                    {
                        Debug.Log($"Direct material test - Current color: {renderer.material.color}");
                        renderer.material.color = Color.yellow;
                        Debug.Log($"Direct material test - New color: {renderer.material.color}");
                    }
                    else
                    {
                        Debug.LogError("Renderer has no material!");
                    }
                }
                else
                {
                    Debug.LogError("No Renderer component found!");
                }
                
                // Then try the proper way
                if (shape != null)
                {
                    Debug.Log("Testing via VolumetricShape.SetHighlighted(true)");
                    shape.SetHighlighted(true);
                }
                else if (highlightController != null)
                {
                    Debug.Log("Testing via HighlightController.SetHighlighted(true)");
                    highlightController.SetHighlighted(true);
                }
                else if (colorHighlight != null)
                {
                    Debug.Log("Testing via ColorHighlight.ApplyHighlight(1.0)");
                    colorHighlight.ApplyHighlight(1.0f);
                }
            }

            // Press R to remove highlight
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("=== Removing Highlight ===");
                
                // Direct material reset
                var renderer = GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    renderer.material.color = Color.white;
                }
                
                if (shape != null)
                {
                    shape.SetHighlighted(false);
                }
                else if (highlightController != null)
                {
                    highlightController.SetHighlighted(false);
                }
                else if (colorHighlight != null)
                {
                    colorHighlight.RemoveHighlight();
                }
            }
        }
    }
}

