using UnityEngine;
using UnityEditor;
using Voxon;
using Voxon.EyeTracker;
using Voxon.GazeDetection;
using Voxon.FaceDetection;
using Voxon.LLM;
using Voxon.CatFace;
using Voxon.VolumetricShapes;

namespace Voxon.Editor
{
    /// <summary>
    /// Unity Editor wizard for setting up Voxon systems
    /// </summary>
    public class VoxonSetupWizard : ScriptableWizard
    {
        [Header("System Setup")]
        [SerializeField] private bool setupEyeTracker = true;
        [SerializeField] private bool setupGazeDetection = true;
        [SerializeField] private bool setupFaceDetection = true;
        [SerializeField] private bool setupLLM = true;
        [SerializeField] private bool setupCatFace = false;

        [Header("Example Content")]
        [SerializeField] private bool createExampleShapes = true;
        [SerializeField] private int numberOfShapes = 5;
        [SerializeField] private float shapeSpacing = 3f;

        [Header("Utilities")]
        [SerializeField] private bool addDebugUI = true;
        [SerializeField] private bool addGazeVisualizer = true;
        [SerializeField] private bool addPerformanceMonitor = true;

        [MenuItem("Voxon/Setup Wizard")]
        static void CreateWizard()
        {
            DisplayWizard<VoxonSetupWizard>("Voxon Setup Wizard", "Setup");
        }

        void OnWizardCreate()
        {
            GameObject setupParent = new GameObject("VoxonSystems");
            Undo.RegisterCreatedObjectUndo(setupParent, "Create Voxon Systems");

            if (setupEyeTracker)
            {
                SetupEyeTracker(setupParent);
            }

            if (setupGazeDetection)
            {
                SetupGazeDetection(setupParent);
            }

            if (setupFaceDetection)
            {
                SetupFaceDetection(setupParent);
            }

            if (setupLLM)
            {
                SetupLLM(setupParent);
            }

            if (setupCatFace)
            {
                SetupCatFace(setupParent);
            }

            if (createExampleShapes)
            {
                CreateExampleShapes();
            }

            if (addDebugUI)
            {
                AddUtility<Utilities.ExpressionDebugUI>(setupParent, "DebugUI");
            }

            if (addGazeVisualizer)
            {
                AddUtility<Utilities.GazeVisualizer>(setupParent, "GazeVisualizer");
            }

            if (addPerformanceMonitor)
            {
                AddUtility<Utilities.PerformanceMonitor>(setupParent, "PerformanceMonitor");
            }

            Selection.activeGameObject = setupParent;
            EditorUtility.DisplayDialog("Setup Complete", 
                "Voxon systems have been set up successfully!\n\n" +
                "Remember to:\n" +
                "- Configure LLM API key in LLMClient\n" +
                "- Assign cat face model if using CatFace system\n" +
                "- Test with generic providers first", 
                "OK");
        }

        private void SetupEyeTracker(GameObject parent)
        {
            GameObject eyeTrackerObj = new GameObject("EyeTrackerManager");
            eyeTrackerObj.transform.SetParent(parent.transform);
            eyeTrackerObj.AddComponent<EyeTrackerManager>();
            Undo.RegisterCreatedObjectUndo(eyeTrackerObj, "Create EyeTrackerManager");
        }

        private void SetupGazeDetection(GameObject parent)
        {
            GameObject gazeObj = new GameObject("GazeDetection");
            gazeObj.transform.SetParent(parent.transform);
            gazeObj.AddComponent<GazeRaycast>();
            gazeObj.AddComponent<GazeHitDetector>();
            Undo.RegisterCreatedObjectUndo(gazeObj, "Create GazeDetection");
        }

        private void SetupFaceDetection(GameObject parent)
        {
            GameObject faceObj = new GameObject("FaceDetection");
            faceObj.transform.SetParent(parent.transform);
            faceObj.AddComponent<FaceDetector>();
            faceObj.AddComponent<ExpressionRecognizer>();
            Undo.RegisterCreatedObjectUndo(faceObj, "Create FaceDetection");
        }

        private void SetupLLM(GameObject parent)
        {
            GameObject llmObj = new GameObject("LLM");
            llmObj.transform.SetParent(parent.transform);
            llmObj.AddComponent<LLMClient>();
            llmObj.AddComponent<ContextManager>();
            llmObj.AddComponent<PromptBuilder>();
            llmObj.AddComponent<LLMResponseParser>();
            llmObj.AddComponent<ResponseEvolution>();
            Undo.RegisterCreatedObjectUndo(llmObj, "Create LLM");
        }

        private void SetupCatFace(GameObject parent)
        {
            GameObject catObj = new GameObject("CatFace");
            catObj.transform.SetParent(parent.transform);
            catObj.AddComponent<CatFaceController>();
            Undo.RegisterCreatedObjectUndo(catObj, "Create CatFace");
        }

        private void CreateExampleShapes()
        {
            GameObject shapesParent = new GameObject("VolumetricShapes");
            Undo.RegisterCreatedObjectUndo(shapesParent, "Create Example Shapes");

            for (int i = 0; i < numberOfShapes; i++)
            {
                GameObject shapeObj = null;
                int shapeType = i % 3;

                switch (shapeType)
                {
                    case 0:
                        shapeObj = new GameObject($"VoxelCube_{i}");
                        shapeObj.AddComponent<VoxelCube>();
                        break;
                    case 1:
                        shapeObj = new GameObject($"VoxelSphere_{i}");
                        shapeObj.AddComponent<VoxelSphere>();
                        break;
                    case 2:
                        shapeObj = new GameObject($"VoxelPyramid_{i}");
                        shapeObj.AddComponent<VoxelPyramid>();
                        break;
                }

                if (shapeObj != null)
                {
                    shapeObj.transform.SetParent(shapesParent.transform);
                    float angle = (360f / numberOfShapes) * i * Mathf.Deg2Rad;
                    shapeObj.transform.position = new Vector3(
                        Mathf.Cos(angle) * shapeSpacing,
                        0,
                        Mathf.Sin(angle) * shapeSpacing
                    );

                    Undo.RegisterCreatedObjectUndo(shapeObj, $"Create {shapeObj.name}");
                }
            }
        }

        private void AddUtility<T>(GameObject parent, string name) where T : Component
        {
            GameObject utilObj = new GameObject(name);
            utilObj.transform.SetParent(parent.transform);
            utilObj.AddComponent<T>();
            Undo.RegisterCreatedObjectUndo(utilObj, $"Create {name}");
        }
    }
}

