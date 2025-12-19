using UnityEngine;

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

        private ExpressionManager expressionManager;
        private ExpressionType currentExpression = ExpressionType.Neutral;
        private Renderer headRenderer;
        private Vector3 leftEarBaseRotation;
        private Vector3 rightEarBaseRotation;
        private Vector3 leftEyeBaseScale;
        private Vector3 rightEyeBaseScale;

        private void Awake()
        {
            CreateCatFace();
            expressionManager = GetComponent<ExpressionManager>();
        }

        private void Start()
        {
            // Store base values for animation
            if (leftEar != null) leftEarBaseRotation = leftEar.transform.localEulerAngles;
            if (rightEar != null) rightEarBaseRotation = rightEar.transform.localEulerAngles;
            if (leftEye != null) leftEyeBaseScale = leftEye.transform.localScale;
            if (rightEye != null) rightEyeBaseScale = rightEye.transform.localScale;

            // Subscribe to expression changes
            if (expressionManager != null)
            {
                expressionManager.OnExpressionChanged += HandleExpressionChanged;
            }
        }

        private void CreateCatFace()
        {
            // Create head (sphere)
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

            // Create eyes (small spheres)
            if (leftEye == null)
            {
                leftEye = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                leftEye.name = "LeftEye";
                leftEye.transform.SetParent(head.transform);
                leftEye.transform.localPosition = new Vector3(-0.15f, 0.1f, 0.25f);
                leftEye.transform.localScale = Vector3.one * 0.08f;
                var eyeRenderer = leftEye.GetComponent<Renderer>();
                if (eyeRenderer != null)
                {
                    eyeRenderer.material.color = eyeColor;
                }
            }

            if (rightEye == null)
            {
                rightEye = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                rightEye.name = "RightEye";
                rightEye.transform.SetParent(head.transform);
                rightEye.transform.localPosition = new Vector3(0.15f, 0.1f, 0.25f);
                rightEye.transform.localScale = Vector3.one * 0.08f;
                var eyeRenderer = rightEye.GetComponent<Renderer>();
                if (eyeRenderer != null)
                {
                    eyeRenderer.material.color = eyeColor;
                }
            }

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

            // Position the whole cat face in front of camera
            transform.position = new Vector3(0, 1.5f, 3f);
            transform.rotation = Quaternion.identity;
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

