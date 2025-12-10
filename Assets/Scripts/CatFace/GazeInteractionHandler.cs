using UnityEngine;
using Voxon.EyeTracker;
using Voxon.GazeDetection;

namespace Voxon.CatFace
{
    /// <summary>
    /// Processes gaze interactions with cat face
    /// </summary>
    public class GazeInteractionHandler : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float mutualGazeThreshold = 1f;
        [SerializeField] private float gazeDetectionDistance = 10f;
        [SerializeField] private LayerMask catFaceLayer;

        private GazeRaycast gazeRaycast;
        private EyeTrackerManager eyeTrackerManager;
        private bool isBeingLookedAt = false;
        private float gazeStartTime = 0f;
        private float lastGazeTime = 0f;

        public event System.Action OnGazeStarted;
        public event System.Action OnGazeEnded;
        public event System.Action<float> OnGazeDurationUpdated;

        private void Start()
        {
            gazeRaycast = FindObjectOfType<GazeRaycast>();
            if (gazeRaycast == null)
            {
                GameObject gazeObject = new GameObject("GazeRaycast");
                gazeRaycast = gazeObject.AddComponent<GazeRaycast>();
            }

            eyeTrackerManager = EyeTrackerManager.Instance;
        }

        private void Update()
        {
            CheckGazeInteraction();
        }

        private void CheckGazeInteraction()
        {
            if (gazeRaycast == null || eyeTrackerManager == null || !eyeTrackerManager.IsConnected())
            {
                return;
            }

            if (gazeRaycast.PerformRaycast(out RaycastHit hitInfo))
            {
                // Check if the hit is this cat face
                if (hitInfo.collider.gameObject == gameObject || 
                    hitInfo.collider.transform.IsChildOf(transform))
                {
                    if (!isBeingLookedAt)
                    {
                        // Gaze started
                        isBeingLookedAt = true;
                        gazeStartTime = Time.time;
                        OnGazeStarted?.Invoke();
                    }
                    else
                    {
                        // Update gaze duration
                        float gazeDuration = Time.time - gazeStartTime;
                        OnGazeDurationUpdated?.Invoke(gazeDuration);
                    }

                    lastGazeTime = Time.time;
                }
                else
                {
                    // Not looking at cat
                    if (isBeingLookedAt && Time.time - lastGazeTime > 0.2f)
                    {
                        isBeingLookedAt = false;
                        OnGazeEnded?.Invoke();
                    }
                }
            }
            else
            {
                // No gaze detected
                if (isBeingLookedAt && Time.time - lastGazeTime > 0.2f)
                {
                    isBeingLookedAt = false;
                    OnGazeEnded?.Invoke();
                }
            }
        }

        /// <summary>
        /// Check if user is currently looking at the cat
        /// </summary>
        public bool IsBeingLookedAt()
        {
            return isBeingLookedAt;
        }

        /// <summary>
        /// Get the duration the user has been looking at the cat
        /// </summary>
        public float GetGazeDuration()
        {
            if (isBeingLookedAt)
            {
                return Time.time - gazeStartTime;
            }
            return 0f;
        }
    }
}

