using UnityEngine;
using Voxon.CatFace;

namespace Voxon.Utilities
{
    /// <summary>
    /// Test tool for cycling through cat expressions
    /// Useful for testing expression system without face detection
    /// </summary>
    public class ExpressionTester : MonoBehaviour
    {
        [Header("Testing Settings")]
        [SerializeField] private KeyCode nextExpressionKey = KeyCode.E;
        [SerializeField] [SerializeField] private KeyCode previousExpressionKey = KeyCode.Q;
        [SerializeField] private float expressionDuration = 3f;
        [SerializeField] private bool autoCycle = false;
        [SerializeField] private float autoCycleInterval = 3f;

        private CatFaceController catFaceController;
        private ExpressionManager expressionManager;
        private ExpressionType[] expressionTypes;
        private int currentIndex = 0;
        private float lastCycleTime = 0f;

        private void Start()
        {
            catFaceController = FindObjectOfType<CatFaceController>();
            if (catFaceController != null)
            {
                expressionManager = catFaceController.GetComponent<ExpressionManager>();
            }

            // Get all expression types
            expressionTypes = (ExpressionType[])System.Enum.GetValues(typeof(ExpressionType));
        }

        private void Update()
        {
            if (expressionManager == null) return;

            if (Input.GetKeyDown(nextExpressionKey))
            {
                NextExpression();
            }
            else if (Input.GetKeyDown(previousExpressionKey))
            {
                PreviousExpression();
            }

            if (autoCycle && Time.time - lastCycleTime > autoCycleInterval)
            {
                NextExpression();
                lastCycleTime = Time.time;
            }
        }

        public void NextExpression()
        {
            if (expressionTypes == null || expressionTypes.Length == 0) return;

            currentIndex = (currentIndex + 1) % expressionTypes.Length;
            SetExpression(expressionTypes[currentIndex]);
        }

        public void PreviousExpression()
        {
            if (expressionTypes == null || expressionTypes.Length == 0) return;

            currentIndex = (currentIndex - 1 + expressionTypes.Length) % expressionTypes.Length;
            SetExpression(expressionTypes[currentIndex]);
        }

        public void SetExpression(ExpressionType expression)
        {
            if (expressionManager != null)
            {
                expressionManager.SetExpression(expression, 0.7f, expressionDuration);
                Debug.Log($"Testing expression: {expression}");
            }
        }

        private void OnGUI()
        {
            if (expressionManager == null) return;

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 14;
            style.normal.textColor = Color.cyan;

            float yPos = Screen.height - 150;
            GUI.Label(new Rect(10, yPos, 300, 20), $"Current: {expressionTypes[currentIndex]}", style);
            GUI.Label(new Rect(10, yPos + 20, 300, 20), $"{nextExpressionKey}: Next | {previousExpressionKey}: Previous", style);
            
            if (autoCycle)
            {
                GUI.Label(new Rect(10, yPos + 40, 300, 20), $"Auto-cycling every {autoCycleInterval}s", style);
            }
        }
    }
}

