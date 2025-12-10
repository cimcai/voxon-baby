using UnityEngine;
using Voxon.EyeTracker;
using Voxon.GazeDetection;
using Voxon.FaceDetection;
using Voxon.LLM;

namespace Voxon.Utilities
{
    /// <summary>
    /// Scene configuration preset for quick scene setup
    /// Attach to a GameObject and configure settings, then call ApplyConfiguration()
    /// </summary>
    [CreateAssetMenu(fileName = "SceneConfig", menuName = "Voxon/Scene Configuration")]
    public class SceneConfig : ScriptableObject
    {
        [Header("Eye Tracker")]
        public bool enableEyeTracker = true;
        public float gazeRaycastDistance = 100f;
        public float focusDwellTime = 0.5f;

        [Header("Face Detection")]
        public bool enableFaceDetection = true;
        public int faceDetectionFPS = 30;
        public float expressionConfidenceThreshold = 0.5f;

        [Header("LLM")]
        public bool enableLLM = true;
        public string apiKey = "";
        public string apiEndpoint = "https://api.openai.com/v1/chat/completions";
        public string model = "gpt-4";
        public float requestCooldown = 1f;
        public int maxContextHistory = 50;

        [Header("Cat Face")]
        public bool enableCatFace = false;
        public Vector3 catFacePosition = Vector3.zero;
        public float expressionTransitionDuration = 0.5f;

        [Header("Shapes")]
        public bool createDefaultShapes = true;
        public int numberOfShapes = 5;
        public float shapeSpacing = 3f;

        /// <summary>
        /// Apply this configuration to the current scene
        /// </summary>
        public void ApplyConfiguration()
        {
            if (enableEyeTracker)
            {
                ConfigureEyeTracker();
            }

            if (enableFaceDetection)
            {
                ConfigureFaceDetection();
            }

            if (enableLLM)
            {
                ConfigureLLM();
            }

            if (enableCatFace)
            {
                ConfigureCatFace();
            }

            if (createDefaultShapes)
            {
                CreateShapes();
            }
        }

        private void ConfigureEyeTracker()
        {
            EyeTrackerManager manager = EyeTrackerManager.Instance;
            
            GazeRaycast gazeRaycast = FindObjectOfType<GazeRaycast>();
            if (gazeRaycast != null)
            {
                // Set raycast distance via reflection or public field
            }

            GazeHitDetector hitDetector = FindObjectOfType<GazeHitDetector>();
            if (hitDetector != null)
            {
                // Set dwell time via reflection or public field
            }
        }

        private void ConfigureFaceDetection()
        {
            FaceDetector faceDetector = FindObjectOfType<FaceDetector>();
            if (faceDetector != null)
            {
                // Configure face detector
            }

            ExpressionRecognizer recognizer = FindObjectOfType<ExpressionRecognizer>();
            if (recognizer != null)
            {
                // Configure recognizer
            }
        }

        private void ConfigureLLM()
        {
            LLMClient llmClient = FindObjectOfType<LLMClient>();
            if (llmClient != null)
            {
                // Configure LLM client via reflection
                SetPrivateField(llmClient, "apiKey", apiKey);
                SetPrivateField(llmClient, "apiEndpoint", apiEndpoint);
                SetPrivateField(llmClient, "model", model);
                SetPrivateField(llmClient, "requestCooldown", requestCooldown);
            }

            ContextManager contextManager = FindObjectOfType<ContextManager>();
            if (contextManager != null)
            {
                SetPrivateField(contextManager, "maxHistorySize", maxContextHistory);
            }
        }

        private void ConfigureCatFace()
        {
            CatFace.CatFaceController catFace = FindObjectOfType<CatFace.CatFaceController>();
            if (catFace != null)
            {
                catFace.transform.position = catFacePosition;
            }
        }

        private void CreateShapes()
        {
            // Shape creation logic
        }

        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(obj, value);
            }
        }

        private T FindObjectOfType<T>() where T : MonoBehaviour
        {
            return Object.FindObjectOfType<T>();
        }
    }
}

