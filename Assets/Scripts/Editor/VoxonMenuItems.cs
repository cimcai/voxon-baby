using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Voxon.EyeTracker;
using Voxon.VolumetricShapes;
using Voxon.CatFace;
using Voxon.GazeDetection;
using Voxon.Utilities;

namespace Voxon.Editor
{
    /// <summary>
    /// Custom menu items for Voxon system
    /// </summary>
    public static class VoxonMenuItems
    {
        [MenuItem("Voxon/Create/Voxel Cube", false, 1)]
        static void CreateVoxelCube()
        {
            // Use Unity's CreatePrimitive to get a cube with all components
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "VoxelCube";
            
            // Add VoxelCube component (VolumetricShape requires Collider, which CreatePrimitive already added)
            VoxelCube voxelCube = cube.AddComponent<VoxelCube>();
            
            // Position cube in front of camera
            UnityEngine.Camera mainCam = UnityEngine.Camera.main;
            if (mainCam != null)
            {
                Vector3 pos = mainCam.transform.position + mainCam.transform.forward * 3f;
                pos.y = 0; // Keep on ground level
                cube.transform.position = pos;
            }
            else
            {
                cube.transform.position = new Vector3(0, 0, 3f);
            }
            
            Selection.activeGameObject = cube;
            Undo.RegisterCreatedObjectUndo(cube, "Create Voxel Cube");
            
            // Force refresh
            EditorUtility.SetDirty(cube);
            if (cube.scene.IsValid())
            {
                EditorSceneManager.MarkSceneDirty(cube.scene);
            }
            
            Debug.Log("VoxelCube created with components: " + 
                (cube.GetComponent<MeshFilter>() != null ? "MeshFilter ✓ " : "MeshFilter ✗ ") +
                (cube.GetComponent<MeshRenderer>() != null ? "MeshRenderer ✓ " : "MeshRenderer ✗ ") +
                (cube.GetComponent<Collider>() != null ? "Collider ✓ " : "Collider ✗ ") +
                (cube.GetComponent<VoxelCube>() != null ? "VoxelCube ✓" : "VoxelCube ✗"));
        }

        [MenuItem("Voxon/Create/Voxel Sphere", false, 2)]
        static void CreateVoxelSphere()
        {
            GameObject sphere = new GameObject("VoxelSphere");
            sphere.AddComponent<VoxelSphere>();
            Selection.activeGameObject = sphere;
            Undo.RegisterCreatedObjectUndo(sphere, "Create Voxel Sphere");
        }

        [MenuItem("Voxon/Create/Voxel Pyramid", false, 3)]
        static void CreateVoxelPyramid()
        {
            GameObject pyramid = new GameObject("VoxelPyramid");
            pyramid.AddComponent<VoxelPyramid>();
            Selection.activeGameObject = pyramid;
            Undo.RegisterCreatedObjectUndo(pyramid, "Create Voxel Pyramid");
        }

        [MenuItem("Voxon/Create/Voxel Cylinder", false, 4)]
        static void CreateVoxelCylinder()
        {
            GameObject cylinder = new GameObject("VoxelCylinder");
            cylinder.AddComponent<VolumetricShapes.VoxelCylinder>();
            Selection.activeGameObject = cylinder;
            Undo.RegisterCreatedObjectUndo(cylinder, "Create Voxel Cylinder");
        }

        [MenuItem("Voxon/Create/Eye Tracker Manager", false, 11)]
        static void CreateEyeTrackerManager()
        {
            GameObject manager = new GameObject("EyeTrackerManager");
            manager.AddComponent<EyeTrackerManager>();
            Selection.activeGameObject = manager;
            Undo.RegisterCreatedObjectUndo(manager, "Create Eye Tracker Manager");
        }

        [MenuItem("Voxon/Create/Gaze Detection", false, 12)]
        static void CreateGazeDetection()
        {
            GameObject gazeDetection = GameObject.Find("GazeDetection");
            if (gazeDetection == null)
            {
                gazeDetection = new GameObject("GazeDetection");
            }
            
            GazeRaycast gazeRaycast = gazeDetection.GetComponent<GazeRaycast>();
            if (gazeRaycast == null)
            {
                gazeRaycast = gazeDetection.AddComponent<GazeRaycast>();
            }
            
            GazeHitDetector hitDetector = gazeDetection.GetComponent<GazeHitDetector>();
            if (hitDetector == null)
            {
                hitDetector = gazeDetection.AddComponent<GazeHitDetector>();
            }
            
            Selection.activeGameObject = gazeDetection;
            Undo.RegisterCreatedObjectUndo(gazeDetection, "Create Gaze Detection");
            EditorSceneManager.MarkSceneDirty(gazeDetection.scene);
            Debug.Log("Gaze Detection system created. Components: GazeRaycast ✓, GazeHitDetector ✓");
        }

        [MenuItem("Voxon/Create/Gaze Debugger", false, 13)]
        static void CreateGazeDebugger()
        {
            GameObject debugger = GameObject.Find("GazeDebugger");
            if (debugger == null)
            {
                debugger = new GameObject("GazeDebugger");
            }
            
            GazeDebugger gazeDebugger = debugger.GetComponent<GazeDebugger>();
            if (gazeDebugger == null)
            {
                gazeDebugger = debugger.AddComponent<GazeDebugger>();
            }
            
            Selection.activeGameObject = debugger;
            Undo.RegisterCreatedObjectUndo(debugger, "Create Gaze Debugger");
            EditorSceneManager.MarkSceneDirty(debugger.scene);
            Debug.Log("Gaze Debugger created. Check console for diagnostics when playing.");
        }

        [MenuItem("Voxon/Create/Cat Face Controller", false, 21)]
        static void CreateCatFaceController()
        {
            GameObject catFace = new GameObject("CatFace");
            catFace.AddComponent<CatFaceController>();
            Selection.activeGameObject = catFace;
            Undo.RegisterCreatedObjectUndo(catFace, "Create Cat Face Controller");
        }

        [MenuItem("Voxon/Create/Face Detection/WebCam Provider", false, 31)]
        static void CreateWebCamFaceProvider()
        {
            GameObject faceDetector = GameObject.Find("FaceDetection");
            if (faceDetector == null)
            {
                faceDetector = new GameObject("FaceDetection");
            }
            
            FaceDetection.FaceDetector detector = faceDetector.GetComponent<FaceDetection.FaceDetector>();
            if (detector == null)
            {
                detector = faceDetector.AddComponent<FaceDetection.FaceDetector>();
            }
            
            // Add WebCam provider
            FaceDetection.WebCamFaceProvider provider = faceDetector.GetComponent<FaceDetection.WebCamFaceProvider>();
            if (provider == null)
            {
                provider = faceDetector.AddComponent<FaceDetection.WebCamFaceProvider>();
            }
            
            // Add Expression Recognizer
            FaceDetection.ExpressionRecognizer recognizer = faceDetector.GetComponent<FaceDetection.ExpressionRecognizer>();
            if (recognizer == null)
            {
                recognizer = faceDetector.AddComponent<FaceDetection.ExpressionRecognizer>();
            }
            
            Selection.activeGameObject = faceDetector;
            Undo.RegisterCreatedObjectUndo(faceDetector, "Create WebCam Face Provider");
        }

        [MenuItem("Voxon/Validate Scene Setup", false, 51)]
        static void ValidateSceneSetup()
        {
            bool hasErrors = false;
            System.Text.StringBuilder report = new System.Text.StringBuilder();
            report.AppendLine("=== Voxon Scene Validation ===\n");

            // Check Eye Tracker
            EyeTrackerManager eyeTracker = Object.FindObjectOfType<EyeTrackerManager>();
            if (eyeTracker == null)
            {
                report.AppendLine("⚠ Missing: EyeTrackerManager");
                hasErrors = true;
            }
            else
            {
                report.AppendLine("✓ EyeTrackerManager found");
            }

            // Check Camera
            UnityEngine.Camera mainCam = UnityEngine.Camera.main;
            if (mainCam == null)
            {
                report.AppendLine("⚠ Missing: Main Camera");
                hasErrors = true;
            }
            else
            {
                report.AppendLine("✓ Main Camera found");
            }

            // Check Gaze Detection
            GazeDetection.GazeRaycast gazeRaycast = Object.FindObjectOfType<GazeDetection.GazeRaycast>();
            if (gazeRaycast == null)
            {
                report.AppendLine("⚠ Missing: GazeRaycast");
                hasErrors = true;
            }
            else
            {
                report.AppendLine("✓ GazeRaycast found");
            }

            // Check Shapes
            VolumetricShape[] shapes = Object.FindObjectsOfType<VolumetricShape>();
            if (shapes.Length == 0)
            {
                report.AppendLine("⚠ No volumetric shapes found in scene");
            }
            else
            {
                report.AppendLine($"✓ Found {shapes.Length} volumetric shape(s)");
            }

            report.AppendLine("\n=== Validation Complete ===");
            
            if (hasErrors)
            {
                EditorUtility.DisplayDialog("Validation Results", report.ToString(), "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Validation Results", 
                    report.ToString() + "\n✓ Scene setup looks good!", "OK");
            }
        }

        [MenuItem("Voxon/Documentation/Open Setup Guide", false, 101)]
        static void OpenSetupGuide()
        {
            string path = "Assets/SETUP_GUIDE.md";
            if (System.IO.File.Exists(path))
            {
                Application.OpenURL("file://" + System.IO.Path.GetFullPath(path));
            }
            else
            {
                EditorUtility.DisplayDialog("Not Found", 
                    "Setup guide not found. Please check the project root directory.", "OK");
            }
        }

        [MenuItem("Voxon/Documentation/Open README", false, 102)]
        static void OpenREADME()
        {
            string path = "Assets/README.md";
            if (System.IO.File.Exists(path))
            {
                Application.OpenURL("file://" + System.IO.Path.GetFullPath(path));
            }
            else
            {
                EditorUtility.DisplayDialog("Not Found", 
                    "README not found. Please check the project root directory.", "OK");
            }
        }
    }
}

