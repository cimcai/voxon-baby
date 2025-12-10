using UnityEngine;
using Voxon.VolumetricShapes;

namespace Voxon.Utilities
{
    /// <summary>
    /// Animates volumetric shapes with various effects
    /// </summary>
    public class ShapeAnimator : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private AnimationType animationType = AnimationType.Rotate;
        [SerializeField] private float animationSpeed = 1f;
        [SerializeField] private Vector3 animationAxis = Vector3.up;
        [SerializeField] private float amplitude = 1f;
        [SerializeField] private bool randomizeStartPhase = true;

        private VolumetricShape shape;
        private Vector3 startPosition;
        private Vector3 startRotation;
        private float phase = 0f;

        public enum AnimationType
        {
            Rotate,
            Bob,
            Pulse,
            Spin
        }

        private void Start()
        {
            shape = GetComponent<VolumetricShape>();
            startPosition = transform.localPosition;
            startRotation = transform.localEulerAngles;

            if (randomizeStartPhase)
            {
                phase = Random.Range(0f, Mathf.PI * 2f);
            }
        }

        private void Update()
        {
            phase += Time.deltaTime * animationSpeed;

            switch (animationType)
            {
                case AnimationType.Rotate:
                    transform.Rotate(animationAxis, animationSpeed * 30f * Time.deltaTime);
                    break;

                case AnimationType.Bob:
                    float bobOffset = Mathf.Sin(phase) * amplitude;
                    transform.localPosition = startPosition + Vector3.up * bobOffset;
                    break;

                case AnimationType.Pulse:
                    float scale = 1f + Mathf.Sin(phase) * amplitude * 0.1f;
                    transform.localScale = Vector3.one * scale;
                    break;

                case AnimationType.Spin:
                    transform.RotateAround(transform.position, animationAxis, animationSpeed * 90f * Time.deltaTime);
                    break;
            }
        }

        public void SetAnimationType(AnimationType type)
        {
            animationType = type;
            phase = 0f;
        }

        public void SetAnimationSpeed(float speed)
        {
            animationSpeed = speed;
        }
    }
}

