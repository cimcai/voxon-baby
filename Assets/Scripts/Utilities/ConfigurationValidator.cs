using UnityEngine;
using Voxon.EyeTracker;
using Voxon.GazeDetection;
using Voxon.FaceDetection;
using Voxon.LLM;
using Voxon.CatFace;

namespace Voxon.Utilities
{
    /// <summary>
    /// Validates system configuration at runtime
    /// </summary>
    public class ConfigurationValidator : MonoBehaviour
    {
        [Header("Validation Settings")]
        [SerializeField] private bool validateOnStart = true;
        [SerializeField] private bool showWarnings = true;
        [SerializeField] private bool showErrors = true;

        private void Start()
        {
            if (validateOnStart)
            {
                ValidateConfiguration();
            }
        }

        public void ValidateConfiguration()
        {
            System.Text.StringBuilder report = new System.Text.StringBuilder();
            report.AppendLine("=== Voxon Configuration Validation ===\n");

            bool hasErrors = false;
            bool hasWarnings = false;

            // Validate Eye Tracker
            EyeTrackerManager eyeTracker = EyeTrackerManager.Instance;
            if (eyeTracker == null)
            {
                report.AppendLine("❌ ERROR: EyeTrackerManager not found");
                hasErrors = true;
            }
            else
            {
                report.AppendLine("✓ EyeTrackerManager found");
                if (!eyeTracker.IsConnected())
                {
                    report.AppendLine("⚠ WARNING: Eye tracker not connected");
                    hasWarnings = true;
                }
            }

            // Validate Camera
            Camera mainCam = Camera.main;
            if (mainCam == null)
            {
                report.AppendLine("❌ ERROR: Main Camera not found");
                hasErrors = true;
            }
            else
            {
                report.AppendLine("✓ Main Camera found");
            }

            // Validate Gaze Detection
            GazeRaycast gazeRaycast = FindObjectOfType<GazeRaycast>();
            if (gazeRaycast == null)
            {
                report.AppendLine("❌ ERROR: GazeRaycast not found");
                hasErrors = true;
            }
            else
            {
                report.AppendLine("✓ GazeRaycast found");
            }

            GazeHitDetector hitDetector = FindObjectOfType<GazeHitDetector>();
            if (hitDetector == null)
            {
                report.AppendLine("❌ ERROR: GazeHitDetector not found");
                hasErrors = true;
            }
            else
            {
                report.AppendLine("✓ GazeHitDetector found");
            }

            // Validate Face Detection
            FaceDetector faceDetector = FindObjectOfType<FaceDetector>();
            if (faceDetector == null)
            {
                report.AppendLine("⚠ WARNING: FaceDetector not found (optional)");
                hasWarnings = true;
            }
            else
            {
                report.AppendLine("✓ FaceDetector found");
            }

            // Validate LLM
            LLMClient llmClient = FindObjectOfType<LLMClient>();
            if (llmClient == null)
            {
                report.AppendLine("⚠ WARNING: LLMClient not found (optional)");
                hasWarnings = true;
            }
            else
            {
                report.AppendLine("✓ LLMClient found");
                // Check API key (would need reflection or public field)
                report.AppendLine("  ⚠ Remember to configure API key");
                hasWarnings = true;
            }

            // Validate Cat Face
            CatFaceController catFace = FindObjectOfType<CatFaceController>();
            if (catFace != null)
            {
                report.AppendLine("✓ CatFaceController found");
                ExpressionManager exprManager = catFace.GetComponent<ExpressionManager>();
                if (exprManager != null)
                {
                    // Check if blend shapes are configured
                    report.AppendLine("  ⚠ Verify blend shape indices are configured");
                    hasWarnings = true;
                }
            }

            // Validate Shapes
            VolumetricShapes.VolumetricShape[] shapes = FindObjectsOfType<VolumetricShapes.VolumetricShape>();
            if (shapes.Length == 0)
            {
                report.AppendLine("⚠ WARNING: No volumetric shapes found in scene");
                hasWarnings = true;
            }
            else
            {
                report.AppendLine($"✓ Found {shapes.Length} volumetric shape(s)");
            }

            report.AppendLine("\n=== Validation Complete ===");
            
            if (hasErrors && showErrors)
            {
                Debug.LogError(report.ToString());
            }
            else if (hasWarnings && showWarnings)
            {
                Debug.LogWarning(report.ToString());
            }
            else
            {
                Debug.Log(report.ToString());
            }
        }

        [ContextMenu("Validate Configuration")]
        private void ValidateFromContextMenu()
        {
            ValidateConfiguration();
        }
    }
}

