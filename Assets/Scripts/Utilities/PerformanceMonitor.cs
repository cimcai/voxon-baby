using UnityEngine;

namespace Voxon.Utilities
{
    /// <summary>
    /// Monitors and displays performance metrics
    /// </summary>
    public class PerformanceMonitor : MonoBehaviour
    {
        [Header("Display Settings")]
        [SerializeField] private bool showFPS = true;
        [SerializeField] private bool showMemory = true;
        [SerializeField] private KeyCode toggleKey = KeyCode.F2;
        [SerializeField] private Vector2 position = new Vector2(10, 50);
        [SerializeField] private int fontSize = 12;

        private float deltaTime = 0.0f;
        private bool isVisible = true;

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                isVisible = !isVisible;
            }

            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            if (!isVisible) return;

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = fontSize;
            style.normal.textColor = Color.green;
            style.alignment = TextAnchor.UpperLeft;

            float yOffset = position.y;
            float lineHeight = fontSize + 5;

            if (showFPS)
            {
                float msec = deltaTime * 1000.0f;
                float fps = 1.0f / deltaTime;
                string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
                GUI.Label(new Rect(position.x, yOffset, 200, lineHeight), text, style);
                yOffset += lineHeight;
            }

            if (showMemory)
            {
                long totalMemory = System.GC.GetTotalMemory(false);
                string memoryText = $"Memory: {totalMemory / 1024 / 1024} MB";
                GUI.Label(new Rect(position.x, yOffset, 200, lineHeight), memoryText, style);
            }
        }
    }
}

