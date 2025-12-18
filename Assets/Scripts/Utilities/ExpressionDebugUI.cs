using UnityEngine;
using Voxon.CatFace;
using Voxon.FaceDetection;

namespace Voxon.Utilities
{
    /// <summary>
    /// Debug UI for displaying current expressions and system status
    /// </summary>
    public class ExpressionDebugUI : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private bool showDebugUI = true;
        [SerializeField] private KeyCode toggleKey = KeyCode.F1;
        [SerializeField] private Vector2 uiPosition = new Vector2(10, 10);
        [SerializeField] private int fontSize = 14;

        private CatFaceController catFaceController;
        private ExpressionRecognizer expressionRecognizer;
        private bool uiVisible = true;

        private void Start()
        {
            catFaceController = FindObjectOfType<CatFaceController>();
            expressionRecognizer = FindObjectOfType<ExpressionRecognizer>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                uiVisible = !uiVisible;
            }
        }

        private void OnGUI()
        {
            if (!showDebugUI || !uiVisible) return;

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = fontSize;
            style.normal.textColor = Color.white;

            float yOffset = uiPosition.y;
            float lineHeight = fontSize + 5;

            // Title
            GUI.Label(new Rect(uiPosition.x, yOffset, 300, lineHeight), "=== Voxon System Debug ===", style);
            yOffset += lineHeight * 1.5f;

            // Cat Face Expression
            if (catFaceController != null)
            {
                CatFace.ExpressionType catExpression = catFaceController.GetCurrentExpression();
                GUI.Label(new Rect(uiPosition.x, yOffset, 300, lineHeight), 
                    $"Cat Expression: {catExpression}", style);
                yOffset += lineHeight;
            }

            // Human Expression
            if (expressionRecognizer != null)
            {
                FaceDetection.ExpressionType humanExpression = expressionRecognizer.GetCurrentExpression();
                GUI.Label(new Rect(uiPosition.x, yOffset, 300, lineHeight), 
                    $"Human Expression: {humanExpression}", style);
                yOffset += lineHeight;
            }

            // Eye Tracker Status
            EyeTracker.EyeTrackerManager eyeTracker = EyeTracker.EyeTrackerManager.Instance;
            if (eyeTracker != null)
            {
                string status = eyeTracker.IsConnected() ? "Connected" : "Disconnected";
                GUI.Label(new Rect(uiPosition.x, yOffset, 300, lineHeight), 
                    $"Eye Tracker: {status}", style);
                yOffset += lineHeight;
            }

            // Instructions
            yOffset += lineHeight;
            GUI.Label(new Rect(uiPosition.x, yOffset, 300, lineHeight * 2), 
                $"Press {toggleKey} to toggle this UI", style);
        }
    }
}

