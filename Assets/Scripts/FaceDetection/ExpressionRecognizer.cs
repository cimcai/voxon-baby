using UnityEngine;
using Voxon.FaceDetection;

namespace Voxon.FaceDetection
{
    /// <summary>
    /// Recognizes facial expressions from detected faces
    /// </summary>
    public class ExpressionRecognizer : MonoBehaviour
    {
        [Header("Recognition Settings")]
        [SerializeField] private float confidenceThreshold = 0.5f;
        [SerializeField] private float expressionChangeThreshold = 0.3f;

        private FaceDetector faceDetector;
        private HumanExpressionData lastExpression;
        private ExpressionType currentExpression = ExpressionType.Neutral;

        private void Start()
        {
            faceDetector = GetComponent<FaceDetector>();
            if (faceDetector == null)
            {
                faceDetector = FindObjectOfType<FaceDetector>();
            }

            lastExpression = new HumanExpressionData();
        }

        private void Update()
        {
            if (faceDetector != null)
            {
                HumanExpressionData expressionData = faceDetector.GetCurrentExpression();
                
                if (expressionData != null && expressionData.confidence >= confidenceThreshold)
                {
                    // Check if expression changed significantly
                    if (HasExpressionChanged(expressionData))
                    {
                        currentExpression = expressionData.expressionType;
                        lastExpression = expressionData;
                        OnExpressionRecognized(expressionData);
                    }
                }
            }
        }

        private bool HasExpressionChanged(HumanExpressionData newExpression)
        {
            if (lastExpression == null)
            {
                return true;
            }

            // Check if expression type changed
            if (newExpression.expressionType != lastExpression.expressionType)
            {
                return true;
            }

            // Check if intensity changed significantly
            if (Mathf.Abs(newExpression.intensity - lastExpression.intensity) > expressionChangeThreshold)
            {
                return true;
            }

            return false;
        }

        private void OnExpressionRecognized(HumanExpressionData expressionData)
        {
            // Expression recognized, can trigger events or update systems
        }

        /// <summary>
        /// Get the current recognized expression
        /// </summary>
        public ExpressionType GetCurrentExpression()
        {
            return currentExpression;
        }
    }
}

