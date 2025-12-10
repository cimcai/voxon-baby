using System;
using UnityEngine;
using Voxon.VolumetricShapes;

namespace Voxon.GazeDetection
{
    /// <summary>
    /// Processes raycast hits and manages focus state
    /// </summary>
    public class GazeHitDetector : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField] private float focusDwellTime = 0.5f;
        [SerializeField] private float unfocusTime = 0.2f;

        private GazeRaycast gazeRaycast;
        private VolumetricShape currentFocusedShape;
        private float focusStartTime;
        private bool isFocused = false;

        public event Action<VolumetricShape> OnShapeFocused;
        public event Action<VolumetricShape> OnShapeUnfocused;
        public event Action<VolumetricShape> OnShapeHighlighted;

        private void Start()
        {
            gazeRaycast = GetComponent<GazeRaycast>();
            if (gazeRaycast == null)
            {
                gazeRaycast = gameObject.AddComponent<GazeRaycast>();
            }
        }

        private void Update()
        {
            if (gazeRaycast == null) return;

            if (gazeRaycast.PerformRaycast(out RaycastHit hitInfo))
            {
                VolumetricShape shape = hitInfo.collider.GetComponent<VolumetricShape>();
                
                if (shape != null)
                {
                    HandleShapeHit(shape);
                }
                else
                {
                    ClearFocus();
                }
            }
            else
            {
                ClearFocus();
            }
        }

        private void HandleShapeHit(VolumetricShape shape)
        {
            if (currentFocusedShape != shape)
            {
                // New shape focused
                if (currentFocusedShape != null)
                {
                    OnShapeUnfocused?.Invoke(currentFocusedShape);
                    currentFocusedShape.SetFocused(false);
                }

                currentFocusedShape = shape;
                focusStartTime = Time.time;
                isFocused = false;
                OnShapeFocused?.Invoke(shape);
                shape.SetFocused(true);
            }
            else
            {
                // Same shape, check if dwell time reached
                if (!isFocused && Time.time - focusStartTime >= focusDwellTime)
                {
                    isFocused = true;
                    OnShapeHighlighted?.Invoke(shape);
                    shape.SetHighlighted(true);
                }
            }
        }

        private void ClearFocus()
        {
            if (currentFocusedShape != null)
            {
                if (Time.time - focusStartTime >= unfocusTime)
                {
                    OnShapeUnfocused?.Invoke(currentFocusedShape);
                    currentFocusedShape.SetFocused(false);
                    currentFocusedShape.SetHighlighted(false);
                    currentFocusedShape = null;
                    isFocused = false;
                }
            }
        }
    }
}

