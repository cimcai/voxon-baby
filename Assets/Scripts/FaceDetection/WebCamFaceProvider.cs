using UnityEngine;
using System.Collections;
using Voxon.FaceDetection;

namespace Voxon.FaceDetection
{
    /// <summary>
    /// WebCam-based face detection provider
    /// Uses Unity's WebCamTexture to access camera feed
    /// Provides foundation for expression analysis
    /// </summary>
    public class WebCamFaceProvider : FaceDetectionProvider
    {
        [Header("Camera Settings")]
        [SerializeField] private int requestedWidth = 640;
        [SerializeField] private int requestedHeight = 480;
        [SerializeField] private int requestedFPS = 30;
        [SerializeField] private string preferredCameraName = "";

        [Header("Detection Settings")]
        [SerializeField] private float detectionInterval = 0.1f;
        [SerializeField] private float expressionChangeThreshold = 0.2f;
        [SerializeField] private bool enableVisualDebug = false;

        [Header("Expression Analysis")]
        [SerializeField] private bool useBasicAnalysis = true;
        [SerializeField] private float analysisSensitivity = 0.5f;

        private WebCamTexture webCamTexture;
        private Texture2D snapshotTexture;
        private HumanExpressionData currentExpression;
        private Coroutine detectionCoroutine;
        private string deviceName;
        private bool isCameraReady = false;

        // Expression tracking
        private ExpressionType lastDetectedExpression = ExpressionType.Neutral;
        private float lastExpressionChangeTime = 0f;
        private float expressionStabilityTime = 0.3f;

        public override bool Initialize()
        {
            try
            {
                // Check for available cameras
                if (WebCamTexture.devices.Length == 0)
                {
                    Debug.LogError("No cameras found. Please connect a webcam.");
                    return false;
                }

                // Select camera
                if (!string.IsNullOrEmpty(preferredCameraName))
                {
                    deviceName = preferredCameraName;
                }
                else
                {
                    deviceName = WebCamTexture.devices[0].name;
                }

                // Initialize webcam texture
                webCamTexture = new WebCamTexture(deviceName, requestedWidth, requestedHeight, requestedFPS);
                
                // Create snapshot texture for analysis
                snapshotTexture = new Texture2D(webCamTexture.width, webCamTexture.height);

                isInitialized = true;
                Debug.Log($"WebCamFaceProvider initialized with camera: {deviceName}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to initialize WebCamFaceProvider: {e.Message}");
                return false;
            }
        }

        public override void StartDetection()
        {
            if (!isInitialized)
            {
                Debug.LogError("WebCamFaceProvider not initialized. Call Initialize() first.");
                return;
            }

            if (webCamTexture == null)
            {
                Debug.LogError("WebCamTexture is null. Cannot start detection.");
                return;
            }

            // Start webcam
            webCamTexture.Play();
            
            // Wait for camera to be ready
            StartCoroutine(WaitForCameraReady());
        }

        private IEnumerator WaitForCameraReady()
        {
            yield return new WaitForSeconds(0.5f); // Give camera time to initialize
            
            if (webCamTexture.width > 16 && webCamTexture.height > 16)
            {
                isCameraReady = true;
                isRunning = true;
                detectionCoroutine = StartCoroutine(DetectionLoop());
                Debug.Log("WebCamFaceProvider started. Camera is ready.");
            }
            else
            {
                Debug.LogError("Camera failed to initialize properly.");
            }
        }

        public override void StopDetection()
        {
            isRunning = false;
            isCameraReady = false;

            if (detectionCoroutine != null)
            {
                StopCoroutine(detectionCoroutine);
                detectionCoroutine = null;
            }

            if (webCamTexture != null && webCamTexture.isPlaying)
            {
                webCamTexture.Stop();
            }

            Debug.Log("WebCamFaceProvider stopped.");
        }

        public override HumanExpressionData GetCurrentExpression()
        {
            return currentExpression ?? new HumanExpressionData();
        }

        /// <summary>
        /// Get the current webcam texture for display or analysis
        /// </summary>
        public WebCamTexture GetWebCamTexture()
        {
            return webCamTexture;
        }

        private IEnumerator DetectionLoop()
        {
            while (isRunning && isCameraReady)
            {
                if (webCamTexture != null && webCamTexture.isPlaying && webCamTexture.didUpdateThisFrame)
                {
                    AnalyzeFrame();
                }

                yield return new WaitForSeconds(detectionInterval);
            }
        }

        private void AnalyzeFrame()
        {
            try
            {
                // Get current frame
                if (snapshotTexture.width != webCamTexture.width || snapshotTexture.height != webCamTexture.height)
                {
                    snapshotTexture.Reinitialize(webCamTexture.width, webCamTexture.height);
                }

                snapshotTexture.SetPixels(webCamTexture.GetPixels());
                snapshotTexture.Apply();

                // Analyze expression
                ExpressionType detectedExpression = AnalyzeExpression(snapshotTexture);
                float confidence = CalculateConfidence();
                float intensity = CalculateIntensity(detectedExpression);

                // Only update if expression changed significantly or enough time passed
                if (ShouldUpdateExpression(detectedExpression, intensity))
                {
                    currentExpression = new HumanExpressionData(detectedExpression, confidence, intensity);
                    currentExpression.facePosition = EstimateFacePosition();
                    currentExpression.faceRotation = EstimateFaceRotation();
                    
                    InvokeExpressionDetected(currentExpression);
                    lastDetectedExpression = detectedExpression;
                    lastExpressionChangeTime = Time.time;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Error analyzing frame: {e.Message}");
            }
        }

        private ExpressionType AnalyzeExpression(Texture2D frame)
        {
            if (!useBasicAnalysis)
            {
                // Placeholder for ML-based analysis
                // This is where you would integrate MediaPipe, OpenCV, or ML models
                return ExpressionType.Neutral;
            }

            // Basic analysis using pixel data
            // This is a simplified approach - real ML would be much more accurate
            return AnalyzeBasicExpression(frame);
        }

        private ExpressionType AnalyzeBasicExpression(Texture2D frame)
        {
            // Basic expression analysis using color and brightness patterns
            // This is a placeholder - real implementations would use:
            // - Facial landmark detection
            // - Machine learning models
            // - Pre-trained expression classifiers

            // For now, simulate based on time and random factors
            // In production, replace this with actual ML analysis
            
            float timeFactor = Mathf.Sin(Time.time * 0.5f);
            float randomFactor = Random.Range(-0.3f, 0.3f);
            float combined = timeFactor + randomFactor;

            // Map to expressions (this is just for demonstration)
            if (combined > 0.5f)
                return ExpressionType.Happy;
            else if (combined < -0.5f)
                return ExpressionType.Sad;
            else if (Mathf.Abs(combined) < 0.2f)
                return ExpressionType.Neutral;
            else if (combined > 0.3f)
                return ExpressionType.Excited;
            else
                return ExpressionType.Surprised;
        }

        private bool ShouldUpdateExpression(ExpressionType newExpression, float intensity)
        {
            // Update if expression type changed
            if (newExpression != lastDetectedExpression)
            {
                return true;
            }

            // Update if enough time passed (for stability)
            if (Time.time - lastExpressionChangeTime > expressionStabilityTime)
            {
                return true;
            }

            // Update if intensity changed significantly
            if (currentExpression != null)
            {
                float intensityDiff = Mathf.Abs(intensity - currentExpression.intensity);
                if (intensityDiff > expressionChangeThreshold)
                {
                    return true;
                }
            }

            return false;
        }

        private float CalculateConfidence()
        {
            // Confidence based on camera quality and analysis
            // In real implementation, this would come from ML model confidence scores
            float baseConfidence = isCameraReady ? 0.7f : 0.3f;
            return Mathf.Clamp01(baseConfidence + Random.Range(-0.1f, 0.1f));
        }

        private float CalculateIntensity(ExpressionType expression)
        {
            // Calculate expression intensity
            // In real implementation, this would come from ML model outputs
            return Mathf.Clamp01(0.5f + Random.Range(-0.2f, 0.3f));
        }

        private Vector3 EstimateFacePosition()
        {
            // Estimate face position in 3D space
            // In real implementation, this would come from face detection SDK
            return Vector3.zero; // Placeholder
        }

        private Quaternion EstimateFaceRotation()
        {
            // Estimate face rotation
            // In real implementation, this would come from face detection SDK
            return Quaternion.identity; // Placeholder
        }

        protected override void OnDestroy()
        {
            StopDetection();

            if (webCamTexture != null)
            {
                webCamTexture.Stop();
                Destroy(webCamTexture);
            }

            if (snapshotTexture != null)
            {
                Destroy(snapshotTexture);
            }
        }

        /// <summary>
        /// Get list of available camera devices
        /// </summary>
        public static string[] GetAvailableCameras()
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            string[] names = new string[devices.Length];
            for (int i = 0; i < devices.Length; i++)
            {
                names[i] = devices[i].name;
            }
            return names;
        }
    }
}

