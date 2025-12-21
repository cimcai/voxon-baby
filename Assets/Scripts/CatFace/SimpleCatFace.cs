using UnityEngine;
using Voxon.EyeTracker;

namespace Voxon.CatFace
{
    /// <summary>
    /// Simple placeholder cat face using Unity primitives
    /// This creates a basic cat head you can see and test with
    /// Replace with a proper 3D model later for better visuals
    /// </summary>
    public class SimpleCatFace : MonoBehaviour
    {
        [Header("Cat Face Parts")]
        [SerializeField] private GameObject head;
        [SerializeField] private GameObject leftEar;
        [SerializeField] private GameObject rightEar;
        [SerializeField] private GameObject leftEye;
        [SerializeField] private GameObject rightEye;
        [SerializeField] private GameObject leftPupil;
        [SerializeField] private GameObject rightPupil;
        [SerializeField] private GameObject nose;
        [SerializeField] private GameObject mouth;

        [Header("Colors")]
        [SerializeField] private Color headColor = new Color(0.8f, 0.6f, 0.4f); // Orange/tabby
        [SerializeField] private Color eyeColor = Color.green;
        [SerializeField] private Color noseColor = Color.black;

        [Header("Expression Visual Feedback")]
        [SerializeField] private bool useColorForExpressions = true;
        [SerializeField] private Color happyColor = Color.yellow;
        [SerializeField] private Color sadColor = Color.blue;
        [SerializeField] private Color surprisedColor = Color.white;

        [Header("Gaze Following")]
        [SerializeField] private bool followGaze = true;
        [SerializeField] private float pupilMaxOffset = 0.03f; // Max distance pupil can move within eye
        [SerializeField] private float gazeFollowSmoothing = 5f;

        private ExpressionManager expressionManager;
        private ExpressionType currentExpression = ExpressionType.Neutral;
        private Renderer headRenderer;
        private Vector3 leftEarBaseRotation;
        private Vector3 rightEarBaseRotation;
        private Vector3 leftEyeBaseScale;
        private Vector3 rightEyeBaseScale;
        private Vector3 leftPupilBasePosition;
        private Vector3 rightPupilBasePosition;
        private EyeTrackerManager eyeTrackerManager;
        private UnityEngine.Camera mainCamera;

        private void Awake()
        {
            Debug.Log("SimpleCatFace.Awake called!");
            
            // Get camera reference early
            mainCamera = UnityEngine.Camera.main;
            
            // Force creation even if parts exist (in case they're missing)
            CreateCatFace();
            
            expressionManager = GetComponent<ExpressionManager>();
            
            Debug.Log($"SimpleCatFace.Awake: Cat face created. Head={head != null}, Position: {transform.position}");
        }

        private void OnEnable()
        {
            Debug.Log("SimpleCatFace.OnEnable called!");
            // Ensure cat face is created when enabled
            if (head == null)
            {
                CreateCatFace();
            }
        }

        private void Start()
        {
            // Store base values for animation
            if (leftEar != null) leftEarBaseRotation = leftEar.transform.localEulerAngles;
            if (rightEar != null) rightEarBaseRotation = rightEar.transform.localEulerAngles;
            if (leftEye != null) leftEyeBaseScale = leftEye.transform.localScale;
            if (rightEye != null) rightEyeBaseScale = rightEye.transform.localScale;
            if (leftPupil != null) leftPupilBasePosition = leftPupil.transform.localPosition;
            if (rightPupil != null) rightPupilBasePosition = rightPupil.transform.localPosition;

            // Get eye tracker and camera references
            eyeTrackerManager = EyeTrackerManager.Instance;
            if (mainCamera == null)
            {
                mainCamera = UnityEngine.Camera.main;
            }

            // Debug: Check if eyes and pupils were created
            Debug.Log($"SimpleCatFace: Head={head != null}, LeftEye={leftEye != null}, RightEye={rightEye != null}, LeftPupil={leftPupil != null}, RightPupil={rightPupil != null}");
            if (leftEye != null)
            {
                Debug.Log($"LeftEye position: {leftEye.transform.position}, scale: {leftEye.transform.localScale}");
            }
            if (rightEye != null)
            {
                Debug.Log($"RightEye position: {rightEye.transform.position}, scale: {rightEye.transform.localScale}");
            }

            // Subscribe to expression changes
            if (expressionManager != null)
            {
                expressionManager.OnExpressionChanged += HandleExpressionChanged;
            }
        }

        private void CreateCatFace()
        {
            Debug.Log("SimpleCatFace: Starting to create cat face...");
            
            // Create head (sphere) - always recreate to ensure it exists
            if (head == null)
            {
                head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                head.name = "CatHead";
                head.transform.SetParent(transform);
                head.transform.localPosition = Vector3.zero;
                head.transform.localScale = Vector3.one * 0.5f;
                headRenderer = head.GetComponent<Renderer>();
                if (headRenderer != null)
                {
                    headRenderer.material.color = headColor;
                }
                Debug.Log("SimpleCatFace: Head created");
            }

            // Create ears (scaled cubes)
            if (leftEar == null)
            {
                leftEar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                leftEar.name = "LeftEar";
                leftEar.transform.SetParent(head.transform);
                leftEar.transform.localPosition = new Vector3(-0.3f, 0.3f, 0.2f);
                leftEar.transform.localScale = new Vector3(0.15f, 0.2f, 0.1f);
                leftEar.transform.localRotation = Quaternion.Euler(-30f, -20f, 0f);
                var earRenderer = leftEar.GetComponent<Renderer>();
                if (earRenderer != null)
                {
                    earRenderer.material.color = headColor;
                }
            }

            if (rightEar == null)
            {
                rightEar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rightEar.name = "RightEar";
                rightEar.transform.SetParent(head.transform);
                rightEar.transform.localPosition = new Vector3(0.3f, 0.3f, 0.2f);
                rightEar.transform.localScale = new Vector3(0.15f, 0.2f, 0.1f);
                rightEar.transform.localRotation = Quaternion.Euler(-30f, 20f, 0f);
                var earRenderer = rightEar.GetComponent<Renderer>();
                if (earRenderer != null)
                {
                    earRenderer.material.color = headColor;
                }
            }

            // Create eyes (larger, more visible spheres) - ALWAYS create
            if (leftEye == null)
            {
                leftEye = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                leftEye.name = "LeftEye";
                leftEye.transform.SetParent(head.transform);
                leftEye.transform.localPosition = new Vector3(-0.2f, 0.15f, 0.35f); // Even more forward
                leftEye.transform.localScale = Vector3.one * 0.2f; // Even bigger eyes
                var eyeRenderer = leftEye.GetComponent<Renderer>();
                if (eyeRenderer != null)
                {
                    eyeRenderer.material.color = eyeColor;
                    // Make eyes more visible with emission
                    eyeRenderer.material.EnableKeyword("_EMISSION");
                    eyeRenderer.material.SetColor("_EmissionColor", eyeColor * 0.8f);
                }
                Debug.Log($"SimpleCatFace: LeftEye created at {leftEye.transform.position}, scale: {leftEye.transform.localScale}");
            }

            if (rightEye == null)
            {
                rightEye = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                rightEye.name = "RightEye";
                rightEye.transform.SetParent(head.transform);
                rightEye.transform.localPosition = new Vector3(0.2f, 0.15f, 0.35f); // Even more forward
                rightEye.transform.localScale = Vector3.one * 0.2f; // Even bigger eyes
                var eyeRenderer = rightEye.GetComponent<Renderer>();
                if (eyeRenderer != null)
                {
                    eyeRenderer.material.color = eyeColor;
                    // Make eyes more visible with emission
                    eyeRenderer.material.EnableKeyword("_EMISSION");
                    eyeRenderer.material.SetColor("_EmissionColor", eyeColor * 0.8f);
                }
                Debug.Log($"SimpleCatFace: RightEye created at {rightEye.transform.position}, scale: {rightEye.transform.localScale}");
            }

            // Create pupils (larger black spheres inside eyes) - ALWAYS create if eyes exist
            if (leftEye != null && leftPupil == null)
            {
                leftPupil = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                leftPupil.name = "LeftPupil";
                leftPupil.transform.SetParent(leftEye.transform);
                leftPupil.transform.localPosition = Vector3.zero;
                leftPupil.transform.localScale = Vector3.one * 0.6f; // Even larger pupils
                var pupilRenderer = leftPupil.GetComponent<Renderer>();
                if (pupilRenderer != null)
                {
                    pupilRenderer.material.color = Color.black;
                }
                // Remove collider so it doesn't interfere
                var pupilCollider = leftPupil.GetComponent<Collider>();
                if (pupilCollider != null)
                {
                    Destroy(pupilCollider);
                }
                Debug.Log("SimpleCatFace: LeftPupil created");
            }

            if (rightEye != null && rightPupil == null)
            {
                rightPupil = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                rightPupil.name = "RightPupil";
                rightPupil.transform.SetParent(rightEye.transform);
                rightPupil.transform.localPosition = Vector3.zero;
                rightPupil.transform.localScale = Vector3.one * 0.6f; // Even larger pupils
                var pupilRenderer = rightPupil.GetComponent<Renderer>();
                if (pupilRenderer != null)
                {
                    pupilRenderer.material.color = Color.black;
                }
                // Remove collider so it doesn't interfere
                var pupilCollider = rightPupil.GetComponent<Collider>();
                if (pupilCollider != null)
                {
                    Destroy(pupilCollider);
                }
                Debug.Log("SimpleCatFace: RightPupil created");
            }
            
            Debug.Log($"SimpleCatFace: Cat face creation complete. Eyes: L={leftEye != null}, R={rightEye != null}, Pupils: L={leftPupil != null}, R={rightPupil != null}");

            // Create nose (small sphere)
            if (nose == null)
            {
                nose = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                nose.name = "Nose";
                nose.transform.SetParent(head.transform);
                nose.transform.localPosition = new Vector3(0f, -0.05f, 0.3f);
                nose.transform.localScale = Vector3.one * 0.05f;
                var noseRenderer = nose.GetComponent<Renderer>();
                if (noseRenderer != null)
                {
                    noseRenderer.material.color = noseColor;
                }
            }

            // Create mouth (small cube)
            if (mouth == null)
            {
                mouth = GameObject.CreatePrimitive(PrimitiveType.Cube);
                mouth.name = "Mouth";
                mouth.transform.SetParent(head.transform);
                mouth.transform.localPosition = new Vector3(0f, -0.15f, 0.25f);
                mouth.transform.localScale = new Vector3(0.1f, 0.02f, 0.02f);
                var mouthRenderer = mouth.GetComponent<Renderer>();
                if (mouthRenderer != null)
                {
                    mouthRenderer.material.color = Color.black;
                }
            }

            // Position the whole cat face closer to camera and make sure it faces forward
            transform.position = new Vector3(0, 1.5f, 1.5f);
            transform.rotation = Quaternion.identity;
            
            // Make sure the cat faces the camera
            if (mainCamera != null)
            {
                Vector3 directionToCamera = mainCamera.transform.position - transform.position;
                directionToCamera.y = 0; // Keep head level
                if (directionToCamera != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(-directionToCamera);
                }
            }
            else
            {
                // If no camera, just face forward (negative Z)
                transform.rotation = Quaternion.LookRotation(Vector3.back);
            }
            
            Debug.Log($"SimpleCatFace: Final position: {transform.position}, rotation: {transform.rotation.eulerAngles}");
        }

        private void Update()
        {
            // Animate based on current expression
            if (expressionManager != null)
            {
                ExpressionType newExpression = expressionManager.GetCurrentExpressionType();
                if (newExpression != currentExpression)
                {
                    currentExpression = newExpression;
                    ApplyExpressionVisuals(currentExpression);
                }
            }

            // Update pupil positions to follow gaze
            if (followGaze)
            {
                UpdatePupilGaze();
            }
        }

        private void UpdatePupilGaze()
        {
            if (eyeTrackerManager == null || !eyeTrackerManager.IsConnected())
                return;

            GazeData gazeData = eyeTrackerManager.GetGazeData();
            if (!gazeData.isValid)
                return;

            // Calculate where the gaze ray hits (or would hit) in world space
            // We'll use the gaze direction to determine where the cat should look
            Vector3 gazeWorldPoint;
            
            // Cast ray from camera through gaze direction to find what the user is looking at
            if (mainCamera != null)
            {
                // Convert gaze direction to world space relative to camera
                Ray gazeRay = new Ray(gazeData.gazeOrigin, gazeData.gazeDirection);
                
                // Find intersection point at a reasonable distance (where objects might be)
                float lookDistance = 5f;
                gazeWorldPoint = gazeRay.origin + gazeRay.direction * lookDistance;
            }
            else
            {
                // Fallback: use gaze direction directly
                gazeWorldPoint = transform.position + gazeData.gazeDirection * 5f;
            }

            // Update left pupil
            if (leftPupil != null && leftEye != null)
            {
                Vector3 eyeWorldPos = leftEye.transform.position;
                Vector3 lookDirection = (gazeWorldPoint - eyeWorldPos).normalized;
                
                // Convert to local space relative to eye
                Vector3 localLookDirection = leftEye.transform.InverseTransformDirection(lookDirection);
                
                // Clamp the offset within the eye sphere
                Vector3 targetOffset = Vector3.ClampMagnitude(localLookDirection * pupilMaxOffset, pupilMaxOffset);
                
                // Smoothly move towards target
                Vector3 currentOffset = leftPupil.transform.localPosition;
                Vector3 newOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * gazeFollowSmoothing);
                leftPupil.transform.localPosition = newOffset;
            }

            // Update right pupil
            if (rightPupil != null && rightEye != null)
            {
                Vector3 eyeWorldPos = rightEye.transform.position;
                Vector3 lookDirection = (gazeWorldPoint - eyeWorldPos).normalized;
                
                // Convert to local space relative to eye
                Vector3 localLookDirection = rightEye.transform.InverseTransformDirection(lookDirection);
                
                // Clamp the offset within the eye sphere
                Vector3 targetOffset = Vector3.ClampMagnitude(localLookDirection * pupilMaxOffset, pupilMaxOffset);
                
                // Smoothly move towards target
                Vector3 currentOffset = rightPupil.transform.localPosition;
                Vector3 newOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * gazeFollowSmoothing);
                rightPupil.transform.localPosition = newOffset;
            }
        }

        private void HandleExpressionChanged(ExpressionType expressionType)
        {
            currentExpression = expressionType;
            ApplyExpressionVisuals(expressionType);
        }

        private void ApplyExpressionVisuals(ExpressionType expressionType)
        {
            float intensity = 1f;

            // Change head color based on expression
            if (useColorForExpressions && headRenderer != null)
            {
                Color targetColor = headColor;
                switch (expressionType)
                {
                    case ExpressionType.Happy:
                        targetColor = Color.Lerp(headColor, happyColor, 0.3f);
                        break;
                    case ExpressionType.Sad:
                        targetColor = Color.Lerp(headColor, sadColor, 0.3f);
                        break;
                    case ExpressionType.Surprised:
                        targetColor = Color.Lerp(headColor, surprisedColor, 0.5f);
                        break;
                    default:
                        targetColor = headColor;
                        break;
                }
                headRenderer.material.color = targetColor;
            }

            // Animate ears
            switch (expressionType)
            {
                case ExpressionType.Happy:
                case ExpressionType.Curious:
                case ExpressionType.Playful:
                    // Ears forward
                    if (leftEar != null)
                        leftEar.transform.localRotation = Quaternion.Euler(leftEarBaseRotation.x - 20f, leftEarBaseRotation.y, leftEarBaseRotation.z);
                    if (rightEar != null)
                        rightEar.transform.localRotation = Quaternion.Euler(rightEarBaseRotation.x - 20f, rightEarBaseRotation.y, rightEarBaseRotation.z);
                    break;

                case ExpressionType.Sad:
                    // Ears back/flattened
                    if (leftEar != null)
                        leftEar.transform.localRotation = Quaternion.Euler(leftEarBaseRotation.x + 30f, leftEarBaseRotation.y - 30f, leftEarBaseRotation.z);
                    if (rightEar != null)
                        rightEar.transform.localRotation = Quaternion.Euler(rightEarBaseRotation.x + 30f, rightEarBaseRotation.y + 30f, rightEarBaseRotation.z);
                    break;

                case ExpressionType.Surprised:
                    // Ears up
                    if (leftEar != null)
                        leftEar.transform.localRotation = Quaternion.Euler(leftEarBaseRotation.x - 40f, leftEarBaseRotation.y, leftEarBaseRotation.z);
                    if (rightEar != null)
                        rightEar.transform.localRotation = Quaternion.Euler(rightEarBaseRotation.x - 40f, rightEarBaseRotation.y, rightEarBaseRotation.z);
                    break;

                default:
                    // Neutral - reset to base
                    if (leftEar != null)
                        leftEar.transform.localRotation = Quaternion.Euler(leftEarBaseRotation);
                    if (rightEar != null)
                        rightEar.transform.localRotation = Quaternion.Euler(rightEarBaseRotation);
                    break;
            }

            // Animate eyes
            switch (expressionType)
            {
                case ExpressionType.Surprised:
                case ExpressionType.Curious:
                    // Wide eyes
                    if (leftEye != null)
                        leftEye.transform.localScale = leftEyeBaseScale * 1.5f;
                    if (rightEye != null)
                        rightEye.transform.localScale = rightEyeBaseScale * 1.5f;
                    break;

                case ExpressionType.Sleepy:
                case ExpressionType.Sad:
                    // Half-closed eyes
                    if (leftEye != null)
                        leftEye.transform.localScale = new Vector3(leftEyeBaseScale.x, leftEyeBaseScale.y * 0.3f, leftEyeBaseScale.z);
                    if (rightEye != null)
                        rightEye.transform.localScale = new Vector3(rightEyeBaseScale.x, rightEyeBaseScale.y * 0.3f, rightEyeBaseScale.z);
                    break;

                default:
                    // Normal eyes
                    if (leftEye != null)
                        leftEye.transform.localScale = leftEyeBaseScale;
                    if (rightEye != null)
                        rightEye.transform.localScale = rightEyeBaseScale;
                    break;
            }
        }

        private void OnDestroy()
        {
            if (expressionManager != null)
            {
                expressionManager.OnExpressionChanged -= HandleExpressionChanged;
            }
        }
    }
}

