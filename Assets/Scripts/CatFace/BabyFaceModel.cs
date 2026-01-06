using UnityEngine;
using Voxon.EyeTracker;

namespace Voxon.CatFace
{
    /// <summary>
    /// Component for imported baby face 3D models
    /// Works with SkinnedMeshRenderer and blend shapes for facial expressions
    /// Supports both blend shape and bone-based eye/pupil tracking
    /// </summary>
    public class BabyFaceModel : MonoBehaviour
    {
        [Header("Model References")]
        [SerializeField] private SkinnedMeshRenderer faceMeshRenderer;
        [SerializeField] private Animator faceAnimator;
        
        [Header("Eye Tracking Setup")]
        [SerializeField] private bool followGaze = true;
        [SerializeField] private float gazeFollowSmoothing = 5f;
        
        [Header("Eye Bone/Transform References")]
        [SerializeField] private Transform leftEyeBone;
        [SerializeField] private Transform rightEyeBone;
        [SerializeField] private Transform leftPupilBone;
        [SerializeField] private Transform rightPupilBone;
        
        [Header("Blend Shape Eye Tracking (Alternative)")]
        [SerializeField] private bool useBlendShapesForEyes = false;
        [SerializeField] private int leftEyeXBlendShape = -1; // Blend shape index for left eye X movement
        [SerializeField] private int leftEyeYBlendShape = -1; // Blend shape index for left eye Y movement
        [SerializeField] private int rightEyeXBlendShape = -1;
        [SerializeField] private int rightEyeYBlendShape = -1;
        
        [Header("Eye Movement Limits")]
        [SerializeField] private float maxEyeRotation = 15f; // Max degrees eyes can rotate
        [SerializeField] private float maxPupilOffset = 0.05f; // Max distance pupil can move (if using bones)
        
        [Header("Expression Manager")]
        [SerializeField] private ExpressionManager expressionManager;
        
        private EyeTrackerManager eyeTrackerManager;
        private UnityEngine.Camera mainCamera;
        private Vector3 leftEyeBaseRotation;
        private Vector3 rightEyeBaseRotation;
        private Vector3 leftPupilBasePosition;
        private Vector3 rightPupilBasePosition;
        
        private void Awake()
        {
            // Auto-find components
            if (faceMeshRenderer == null)
            {
                faceMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            }
            
            if (faceAnimator == null)
            {
                faceAnimator = GetComponent<Animator>();
            }
            
            if (expressionManager == null)
            {
                expressionManager = GetComponent<ExpressionManager>();
            }
            
            mainCamera = UnityEngine.Camera.main;
            
            // Store base positions/rotations
            if (leftEyeBone != null)
            {
                leftEyeBaseRotation = leftEyeBone.localEulerAngles;
            }
            if (rightEyeBone != null)
            {
                rightEyeBaseRotation = rightEyeBone.localEulerAngles;
            }
            if (leftPupilBone != null)
            {
                leftPupilBasePosition = leftPupilBone.localPosition;
            }
            if (rightPupilBone != null)
            {
                rightPupilBasePosition = rightPupilBone.localPosition;
            }
            
            Debug.Log($"BabyFaceModel: Initialized. MeshRenderer={faceMeshRenderer != null}, Animator={faceAnimator != null}, BlendShapes={GetBlendShapeCount()}");
        }
        
        private void Start()
        {
            eyeTrackerManager = EyeTrackerManager.Instance;
            
            if (faceMeshRenderer == null)
            {
                Debug.LogWarning("BabyFaceModel: No SkinnedMeshRenderer found! Please assign one in the Inspector.");
            }
            else
            {
                Debug.Log($"BabyFaceModel: Face mesh has {GetBlendShapeCount()} blend shapes available.");
                LogAvailableBlendShapes();
            }
        }
        
        private void Update()
        {
            if (followGaze && eyeTrackerManager != null && eyeTrackerManager.IsConnected())
            {
                UpdateEyeTracking();
            }
        }
        
        private void UpdateEyeTracking()
        {
            GazeData gazeData = eyeTrackerManager.GetGazeData();
            if (!gazeData.isValid || mainCamera == null) return;
            
            // Convert gaze direction to world position
            Vector3 gazeWorldPos = GetGazeWorldPosition(gazeData);
            
            if (gazeWorldPos == Vector3.zero) return;
            
            // Update eyes based on gaze
            if (useBlendShapesForEyes && faceMeshRenderer != null)
            {
                UpdateEyesWithBlendShapes(gazeWorldPos);
            }
            else if (leftEyeBone != null && rightEyeBone != null)
            {
                UpdateEyesWithBones(gazeWorldPos);
            }
        }
        
        private Vector3 GetGazeWorldPosition(GazeData gazeData)
        {
            if (mainCamera == null) return Vector3.zero;
            
            // Use gaze origin and direction to calculate world position
            // Cast a ray from gaze origin in gaze direction
            Ray gazeRay = new Ray(gazeData.gazeOrigin, gazeData.gazeDirection);
            
            // Get a point 5 units along the gaze ray (or use raycast to hit something)
            return gazeRay.GetPoint(5f);
        }
        
        private void UpdateEyesWithBones(Vector3 targetWorldPos)
        {
            // Update left eye
            if (leftEyeBone != null)
            {
                Vector3 eyeWorldPos = leftEyeBone.position;
                Vector3 directionToTarget = (targetWorldPos - eyeWorldPos).normalized;
                
                // Calculate rotation
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                Quaternion localRotation = Quaternion.Inverse(leftEyeBone.parent.rotation) * targetRotation;
                
                // Clamp rotation
                Vector3 euler = localRotation.eulerAngles;
                euler.x = ClampAngle(euler.x, -maxEyeRotation, maxEyeRotation);
                euler.y = ClampAngle(euler.y, -maxEyeRotation, maxEyeRotation);
                
                leftEyeBone.localRotation = Quaternion.Slerp(
                    leftEyeBone.localRotation,
                    Quaternion.Euler(euler),
                    Time.deltaTime * gazeFollowSmoothing
                );
            }
            
            // Update right eye (same logic)
            if (rightEyeBone != null)
            {
                Vector3 eyeWorldPos = rightEyeBone.position;
                Vector3 directionToTarget = (targetWorldPos - eyeWorldPos).normalized;
                
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                Quaternion localRotation = Quaternion.Inverse(rightEyeBone.parent.rotation) * targetRotation;
                
                Vector3 euler = localRotation.eulerAngles;
                euler.x = ClampAngle(euler.x, -maxEyeRotation, maxEyeRotation);
                euler.y = ClampAngle(euler.y, -maxEyeRotation, maxEyeRotation);
                
                rightEyeBone.localRotation = Quaternion.Slerp(
                    rightEyeBone.localRotation,
                    Quaternion.Euler(euler),
                    Time.deltaTime * gazeFollowSmoothing
                );
            }
            
            // Update pupils (if using separate pupil bones)
            if (leftPupilBone != null && leftEyeBone != null)
            {
                Vector3 offset = CalculatePupilOffset(leftEyeBone, targetWorldPos);
                leftPupilBone.localPosition = Vector3.Lerp(
                    leftPupilBone.localPosition,
                    leftPupilBasePosition + offset,
                    Time.deltaTime * gazeFollowSmoothing
                );
            }
            
            if (rightPupilBone != null && rightEyeBone != null)
            {
                Vector3 offset = CalculatePupilOffset(rightEyeBone, targetWorldPos);
                rightPupilBone.localPosition = Vector3.Lerp(
                    rightPupilBone.localPosition,
                    rightPupilBasePosition + offset,
                    Time.deltaTime * gazeFollowSmoothing
                );
            }
        }
        
        private void UpdateEyesWithBlendShapes(Vector3 targetWorldPos)
        {
            if (faceMeshRenderer == null) return;
            
            // Calculate eye movement direction relative to face
            Vector3 faceForward = transform.forward;
            Vector3 faceRight = transform.right;
            Vector3 faceUp = transform.up;
            
            Vector3 localDirection = transform.InverseTransformDirection((targetWorldPos - transform.position).normalized);
            
            // Map to blend shapes (assuming blend shapes are normalized -1 to 1)
            float leftEyeX = Mathf.Clamp(localDirection.x * 50f, -50f, 50f);
            float leftEyeY = Mathf.Clamp(localDirection.y * 50f, -50f, 50f);
            float rightEyeX = Mathf.Clamp(localDirection.x * 50f, -50f, 50f);
            float rightEyeY = Mathf.Clamp(localDirection.y * 50f, -50f, 50f);
            
            if (leftEyeXBlendShape >= 0)
            {
                faceMeshRenderer.SetBlendShapeWeight(leftEyeXBlendShape, Mathf.Lerp(
                    faceMeshRenderer.GetBlendShapeWeight(leftEyeXBlendShape),
                    leftEyeX + 50f, // Offset to 0-100 range
                    Time.deltaTime * gazeFollowSmoothing
                ));
            }
            
            if (leftEyeYBlendShape >= 0)
            {
                faceMeshRenderer.SetBlendShapeWeight(leftEyeYBlendShape, Mathf.Lerp(
                    faceMeshRenderer.GetBlendShapeWeight(leftEyeYBlendShape),
                    leftEyeY + 50f,
                    Time.deltaTime * gazeFollowSmoothing
                ));
            }
            
            if (rightEyeXBlendShape >= 0)
            {
                faceMeshRenderer.SetBlendShapeWeight(rightEyeXBlendShape, Mathf.Lerp(
                    faceMeshRenderer.GetBlendShapeWeight(rightEyeXBlendShape),
                    rightEyeX + 50f,
                    Time.deltaTime * gazeFollowSmoothing
                ));
            }
            
            if (rightEyeYBlendShape >= 0)
            {
                faceMeshRenderer.SetBlendShapeWeight(rightEyeYBlendShape, Mathf.Lerp(
                    faceMeshRenderer.GetBlendShapeWeight(rightEyeYBlendShape),
                    rightEyeY + 50f,
                    Time.deltaTime * gazeFollowSmoothing
                ));
            }
        }
        
        private Vector3 CalculatePupilOffset(Transform eyeBone, Vector3 targetWorldPos)
        {
            Vector3 eyeWorldPos = eyeBone.position;
            Vector3 directionToTarget = (targetWorldPos - eyeWorldPos).normalized;
            
            // Convert to local space
            Vector3 localDirection = eyeBone.InverseTransformDirection(directionToTarget);
            
            // Clamp to max offset
            localDirection.x = Mathf.Clamp(localDirection.x, -maxPupilOffset, maxPupilOffset);
            localDirection.y = Mathf.Clamp(localDirection.y, -maxPupilOffset, maxPupilOffset);
            localDirection.z = 0f; // Keep pupil on eye surface
            
            return localDirection;
        }
        
        private float ClampAngle(float angle, float min, float max)
        {
            if (angle > 180f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }
        
        /// <summary>
        /// Get the number of blend shapes available on the face mesh
        /// </summary>
        public int GetBlendShapeCount()
        {
            if (faceMeshRenderer != null && faceMeshRenderer.sharedMesh != null)
            {
                return faceMeshRenderer.sharedMesh.blendShapeCount;
            }
            return 0;
        }
        
        /// <summary>
        /// Log all available blend shape names (useful for setup)
        /// </summary>
        public void LogAvailableBlendShapes()
        {
            if (faceMeshRenderer == null || faceMeshRenderer.sharedMesh == null)
            {
                Debug.LogWarning("BabyFaceModel: No mesh to read blend shapes from.");
                return;
            }
            
            Mesh mesh = faceMeshRenderer.sharedMesh;
            int count = mesh.blendShapeCount;
            
            Debug.Log($"BabyFaceModel: Available Blend Shapes ({count} total):");
            for (int i = 0; i < count; i++)
            {
                string name = mesh.GetBlendShapeName(i);
                Debug.Log($"  [{i}] {name}");
            }
        }
        
        /// <summary>
        /// Find and assign eye bones automatically (if using standard naming)
        /// </summary>
        [ContextMenu("Auto-Find Eye Bones")]
        public void AutoFindEyeBones()
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            
            foreach (Transform child in allChildren)
            {
                string name = child.name.ToLower();
                
                if (name.Contains("eye") && name.Contains("left") && !name.Contains("pupil"))
                {
                    leftEyeBone = child;
                    Debug.Log($"BabyFaceModel: Found left eye bone: {child.name}");
                }
                else if (name.Contains("eye") && name.Contains("right") && !name.Contains("pupil"))
                {
                    rightEyeBone = child;
                    Debug.Log($"BabyFaceModel: Found right eye bone: {child.name}");
                }
                else if (name.Contains("pupil") && name.Contains("left"))
                {
                    leftPupilBone = child;
                    Debug.Log($"BabyFaceModel: Found left pupil bone: {child.name}");
                }
                else if (name.Contains("pupil") && name.Contains("right"))
                {
                    rightPupilBone = child;
                    Debug.Log($"BabyFaceModel: Found right pupil bone: {child.name}");
                }
            }
        }
    }
}


