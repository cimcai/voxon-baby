using UnityEngine;
using Voxon.EyeTracker;
using Voxon.GazeDetection;
using Voxon.FaceDetection;
using Voxon.LLM;
using Voxon.CatFace;
using Voxon.VolumetricShapes;

namespace Voxon
{
    /// <summary>
    /// System initializer that sets up all Voxon systems automatically
    /// Use this to quickly set up a scene with all required components
    /// </summary>
    public class VoxonSystemInitializer : MonoBehaviour
    {
        [Header("Auto Setup Options")]
        [SerializeField] private bool setupEyeTracker = true;
        [SerializeField] private bool setupGazeDetection = true;
        [SerializeField] private bool setupFaceDetection = true;
        [SerializeField] private bool setupLLM = true;
        [SerializeField] private bool setupCatFace = false;
        [SerializeField] private bool createExampleShapes = false;

        [Header("Example Shapes Settings")]
        [SerializeField] private int numberOfShapes = 5;
        [SerializeField] private float shapeSpacing = 3f;
        [SerializeField] private Vector3 shapeAreaCenter = Vector3.zero;

        [Header("Camera Settings")]
        [SerializeField] private bool setupCamera = true;
        [SerializeField] private Vector3 cameraPosition = new Vector3(0, 1.6f, -5f);
        [SerializeField] private Vector3 cameraRotation = Vector3.zero;

        private void Awake()
        {
            if (setupCamera)
            {
                SetupCamera();
            }
        }

        private void Start()
        {
            if (setupEyeTracker)
            {
                SetupEyeTracker();
            }

            if (setupGazeDetection)
            {
                SetupGazeDetection();
            }

            if (setupFaceDetection)
            {
                SetupFaceDetection();
            }

            if (setupLLM)
            {
                SetupLLM();
            }

            if (setupCatFace)
            {
                SetupCatFace();
            }

            if (createExampleShapes)
            {
                CreateExampleShapes();
            }
        }

        private void SetupCamera()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                GameObject cameraObj = new GameObject("Main Camera");
                mainCamera = cameraObj.AddComponent<Camera>();
                cameraObj.tag = "MainCamera";
            }

            // Add EyeTrackerCamera component
            EyeTrackerCamera eyeTrackerCamera = mainCamera.GetComponent<EyeTrackerCamera>();
            if (eyeTrackerCamera == null)
            {
                eyeTrackerCamera = mainCamera.gameObject.AddComponent<EyeTrackerCamera>();
            }

            // Position camera
            mainCamera.transform.position = cameraPosition;
            mainCamera.transform.rotation = Quaternion.Euler(cameraRotation);

            Debug.Log("Camera setup complete.");
        }

        private void SetupEyeTracker()
        {
            // EyeTrackerManager is a singleton, so just get the instance
            EyeTrackerManager manager = EyeTrackerManager.Instance;
            Debug.Log("Eye Tracker system initialized.");
        }

        private void SetupGazeDetection()
        {
            GameObject gazeObject = GameObject.Find("GazeDetection");
            if (gazeObject == null)
            {
                gazeObject = new GameObject("GazeDetection");
            }

            GazeRaycast gazeRaycast = gazeObject.GetComponent<GazeRaycast>();
            if (gazeRaycast == null)
            {
                gazeRaycast = gazeObject.AddComponent<GazeRaycast>();
            }

            GazeHitDetector gazeHitDetector = gazeObject.GetComponent<GazeHitDetector>();
            if (gazeHitDetector == null)
            {
                gazeHitDetector = gazeObject.AddComponent<GazeHitDetector>();
            }

            Debug.Log("Gaze Detection system initialized.");
        }

        private void SetupFaceDetection()
        {
            GameObject faceDetectionObject = GameObject.Find("FaceDetection");
            if (faceDetectionObject == null)
            {
                faceDetectionObject = new GameObject("FaceDetection");
            }

            FaceDetector faceDetector = faceDetectionObject.GetComponent<FaceDetector>();
            if (faceDetector == null)
            {
                faceDetector = faceDetectionObject.AddComponent<FaceDetector>();
            }

            ExpressionRecognizer expressionRecognizer = faceDetectionObject.GetComponent<ExpressionRecognizer>();
            if (expressionRecognizer == null)
            {
                expressionRecognizer = faceDetectionObject.AddComponent<ExpressionRecognizer>();
            }

            Debug.Log("Face Detection system initialized.");
        }

        private void SetupLLM()
        {
            GameObject llmObject = GameObject.Find("LLM");
            if (llmObject == null)
            {
                llmObject = new GameObject("LLM");
            }

            LLMClient llmClient = llmObject.GetComponent<LLMClient>();
            if (llmClient == null)
            {
                llmClient = llmObject.AddComponent<LLMClient>();
            }

            ContextManager contextManager = llmObject.GetComponent<ContextManager>();
            if (contextManager == null)
            {
                contextManager = llmObject.AddComponent<ContextManager>();
            }

            PromptBuilder promptBuilder = llmObject.GetComponent<PromptBuilder>();
            if (promptBuilder == null)
            {
                promptBuilder = llmObject.AddComponent<PromptBuilder>();
            }

            LLMResponseParser responseParser = llmObject.GetComponent<LLMResponseParser>();
            if (responseParser == null)
            {
                responseParser = llmObject.AddComponent<LLMResponseParser>();
            }

            ResponseEvolution responseEvolution = llmObject.GetComponent<ResponseEvolution>();
            if (responseEvolution == null)
            {
                responseEvolution = llmObject.AddComponent<ResponseEvolution>();
            }

            Debug.Log("LLM system initialized. Remember to configure API key in LLMClient component.");
        }

        private void SetupCatFace()
        {
            GameObject catFaceObject = GameObject.Find("CatFace");
            if (catFaceObject == null)
            {
                catFaceObject = new GameObject("CatFace");
            }

            CatFaceController catFaceController = catFaceObject.GetComponent<CatFaceController>();
            if (catFaceController == null)
            {
                catFaceController = catFaceObject.AddComponent<CatFaceController>();
            }

            Debug.Log("Cat Face system initialized. Remember to assign a cat face model with blend shapes.");
        }

        private void CreateExampleShapes()
        {
            GameObject shapesParent = new GameObject("VolumetricShapes");
            shapesParent.transform.position = shapeAreaCenter;

            for (int i = 0; i < numberOfShapes; i++)
            {
                GameObject shapeObject = null;
                VolumetricShape shape = null;

                // Alternate between cube, sphere, and pyramid
                int shapeType = i % 3;
                switch (shapeType)
                {
                    case 0:
                        shapeObject = new GameObject($"VoxelCube_{i}");
                        shape = shapeObject.AddComponent<VoxelCube>();
                        break;
                    case 1:
                        shapeObject = new GameObject($"VoxelSphere_{i}");
                        shape = shapeObject.AddComponent<VoxelSphere>();
                        break;
                    case 2:
                        shapeObject = new GameObject($"VoxelPyramid_{i}");
                        shape = shapeObject.AddComponent<VoxelPyramid>();
                        break;
                }

                if (shapeObject != null)
                {
                    shapeObject.transform.SetParent(shapesParent.transform);
                    
                    // Arrange in a circle
                    float angle = (360f / numberOfShapes) * i * Mathf.Deg2Rad;
                    float x = Mathf.Cos(angle) * shapeSpacing;
                    float z = Mathf.Sin(angle) * shapeSpacing;
                    shapeObject.transform.localPosition = new Vector3(x, 0, z);

                    // Randomize colors
                    Renderer renderer = shapeObject.GetComponent<Renderer>();
                    if (renderer != null && renderer.material != null)
                    {
                        renderer.material.color = new Color(
                            Random.Range(0.3f, 1f),
                            Random.Range(0.3f, 1f),
                            Random.Range(0.3f, 1f)
                        );
                    }
                }
            }

            Debug.Log($"Created {numberOfShapes} example volumetric shapes.");
        }
    }
}

