using UnityEngine;
using Voxon.EyeTracker;
using Voxon.FaceDetection;
using Voxon.LLM;

namespace Voxon.CatFace
{
    /// <summary>
    /// Main controller for the cat face character
    /// 
    /// This system integrates research on cat-human interaction and feline communication:
    /// - Studies show humans can identify cats' affective states from subtle facial expressions
    ///   (Humans can identify cats' affective states from subtle facial expressions, Cambridge Core)
    /// - CatFACS provides a systematic framework for coding cat facial expressions
    /// - Research demonstrates that facial expressions are key to cat-human communication
    /// 
    /// The controller coordinates expression display based on:
    /// 1. Human facial expression detection (mirroring/empathy responses)
    /// 2. Gaze interaction patterns (mutual attention)
    /// 3. LLM-generated contextual responses (adaptive behavior)
    /// 
    /// References:
    /// - Caeiro, C. C., et al. (2017). CatFACS development
    /// - Research on human-cat communication and expression recognition
    /// - Comprehensive feline ethogram for emotion identification (Irish Veterinary Journal, 2021)
    /// </summary>
    public class CatFaceController : MonoBehaviour
    {
        [Header("Cat Face Settings")]
        [SerializeField] private bool enableCatFace = true;
        [SerializeField] private Vector3 defaultPosition = Vector3.zero;
        [SerializeField] private Quaternion defaultRotation = Quaternion.identity;

        [Header("Components")]
        [SerializeField] private ExpressionManager expressionManager;
        [SerializeField] private GazeInteractionHandler gazeHandler;
        [SerializeField] private ExpressionTriggers expressionTriggers;
        [SerializeField] private LLMExpressionMapper llmMapper;

        private EyeTrackerManager eyeTrackerManager;
        private FaceDetector faceDetector;
        private LLMClient llmClient;
        private bool isInitialized = false;

        private void Awake()
        {
            // Get or create components
            if (expressionManager == null)
                expressionManager = GetComponent<ExpressionManager>();
            if (gazeHandler == null)
                gazeHandler = GetComponent<GazeInteractionHandler>();
            if (expressionTriggers == null)
                expressionTriggers = GetComponent<ExpressionTriggers>();
            if (llmMapper == null)
                llmMapper = GetComponent<LLMExpressionMapper>();

            // Initialize position
            transform.position = defaultPosition;
            transform.rotation = defaultRotation;
        }

        private void Start()
        {
            if (enableCatFace)
            {
                Initialize();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Initialize the cat face system
        /// </summary>
        public void Initialize()
        {
            eyeTrackerManager = EyeTrackerManager.Instance;
            faceDetector = FindObjectOfType<FaceDetector>();
            llmClient = FindObjectOfType<LLMClient>();

            // Ensure components exist
            if (expressionManager == null)
                expressionManager = gameObject.AddComponent<ExpressionManager>();
            if (gazeHandler == null)
                gazeHandler = gameObject.AddComponent<GazeInteractionHandler>();
            if (expressionTriggers == null)
                expressionTriggers = gameObject.AddComponent<ExpressionTriggers>();
            if (llmMapper == null)
                llmMapper = gameObject.AddComponent<LLMExpressionMapper>();

            isInitialized = true;
            Debug.Log("CatFaceController initialized.");
        }

        /// <summary>
        /// Enable or disable the cat face
        /// </summary>
        public void SetEnabled(bool enabled)
        {
            enableCatFace = enabled;
            gameObject.SetActive(enabled);

            if (enabled && !isInitialized)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Request an expression update from LLM
        /// </summary>
        public void RequestExpressionUpdate()
        {
            if (llmMapper != null)
            {
                llmMapper.RequestExpressionRecommendation();
            }
        }

        /// <summary>
        /// Get the current expression
        /// </summary>
        public ExpressionType GetCurrentExpression()
        {
            if (expressionManager != null)
            {
                return expressionManager.GetCurrentExpressionType();
            }
            return ExpressionType.Neutral;
        }

        private void Update()
        {
            if (!enableCatFace || !isInitialized)
                return;

            // Periodic LLM updates (can be optimized based on needs)
            // This would typically be triggered by events rather than Update
        }
    }
}

