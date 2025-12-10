# Face Detection Integration Guide

## Overview

The Voxon system now includes **real face detection providers** that analyze actual camera feeds. Choose the provider that best fits your needs:

1. **WebCamFaceProvider** - ✅ Ready to use (uses Unity WebCamTexture)
2. **MediaPipeFaceProvider** - Template for MediaPipe integration
3. **OpenCVFaceProvider** - Template for OpenCV integration  
4. **AzureFaceProvider** - Template for Azure Face API integration

## Quick Start: WebCam Provider (Easiest)

### Setup in 2 Minutes

1. **Create Face Detection System**:
   - Menu: `Voxon > Create > Face Detection > WebCam Provider`
   - OR manually: Create GameObject, add `FaceDetector` + `WebCamFaceProvider`

2. **Configure**:
   - Select camera device (if multiple)
   - Set resolution and FPS
   - Enable "Use Basic Analysis" for testing

3. **Press Play**:
   - System accesses your webcam
   - Analyzes facial expressions in real-time
   - Cat responds to your expressions!

### How It Works

```
Webcam → WebCamTexture → Frame Analysis → Expression Detection → Cat Response
```

The `WebCamFaceProvider`:
- ✅ Accesses your webcam/camera
- ✅ Captures frames in real-time
- ✅ Analyzes expressions (basic analysis included)
- ✅ Provides expression data to cat system
- ✅ Works immediately, no SDK installation needed

## Provider Comparison

| Provider | Status | Accuracy | Setup Difficulty | Cost |
|----------|--------|----------|------------------|------|
| **WebCamFaceProvider** | ✅ Ready | Basic | Easy | Free |
| **MediaPipe** | 📝 Template | High | Medium | Free |
| **OpenCV** | 📝 Template | Medium-High | Medium | Free |
| **Azure Face API** | 📝 Template | Very High | Easy | Paid |

## WebCamFaceProvider Details

### Features
- Real webcam access via Unity WebCamTexture
- Configurable resolution and FPS
- Basic expression analysis (can be enhanced)
- Expression stability filtering
- Confidence and intensity calculation
- Multiple camera support

### Configuration Options

```csharp
[Header("Camera Settings")]
- Requested Width/Height: Camera resolution
- Requested FPS: Frame rate (default: 30)
- Preferred Camera Name: Specific camera selection

[Header("Detection Settings")]
- Detection Interval: How often to analyze (default: 0.1s)
- Expression Change Threshold: Sensitivity (default: 0.2)
- Enable Visual Debug: Show camera feed

[Header("Expression Analysis")]
- Use Basic Analysis: Enable basic expression detection
- Analysis Sensitivity: Detection sensitivity (0-1)
```

### Basic Analysis

The included basic analysis provides:
- Expression type detection (Happy, Sad, Neutral, etc.)
- Confidence scores
- Intensity measurement
- Expression stability (prevents jitter)

**Note**: For production use, enhance with ML models or integrate MediaPipe/OpenCV for higher accuracy.

## MediaPipe Integration

### Installation

1. **Install MediaPipe Unity Plugin**:
   ```bash
   # Clone repository
   git clone https://github.com/homuler/MediaPipeUnityPlugin.git
   
   # Follow installation instructions
   ```

2. **Import into Unity**:
   - Import MediaPipe package
   - Add MediaPipe namespaces

3. **Configure Provider**:
   - Use `MediaPipeFaceProvider` template
   - Replace placeholder code with MediaPipe API calls
   - Configure face detection graph

### Implementation Steps

1. **Initialize MediaPipe**:
   ```csharp
   detector = new MediaPipe.FaceDetection.FaceDetector();
   detector.Initialize();
   ```

2. **Detect Faces**:
   ```csharp
   var results = detector.Detect(cameraFrame);
   ```

3. **Analyze Landmarks**:
   ```csharp
   var landmarks = landmarkCalculator.Calculate(face);
   ExpressionType expression = AnalyzeLandmarks(landmarks);
   ```

4. **Map to Expression**:
   - Use 468 facial landmarks
   - Analyze mouth, eyes, eyebrows
   - Determine expression type

### MediaPipe Advantages
- ✅ 468 facial landmarks
- ✅ High accuracy
- ✅ Real-time performance
- ✅ Open source
- ✅ Well documented

## OpenCV Integration

### Installation

1. **Install OpenCV for Unity**:
   ```bash
   # Download from: https://github.com/EnoxSoftware/OpenCVForUnity
   # Or use Unity Package Manager
   ```

2. **Import into Unity**:
   - Import OpenCV package
   - Add OpenCV namespaces

3. **Configure Provider**:
   - Use `OpenCVFaceProvider` template
   - Load face detection models
   - Implement expression classification

### Implementation Steps

1. **Load Face Detection Model**:
   ```csharp
   // Option 1: Haar Cascade
   faceCascade = new CascadeClassifier();
   faceCascade.load("haarcascade_frontalface_default.xml");
   
   // Option 2: DNN Model (better accuracy)
   dnnNet = Dnn.readNetFromTensorflow("opencv_face_detector.pb");
   ```

2. **Detect Faces**:
   ```csharp
   Mat grayFrame = ConvertToGray(frame);
   faceCascade.detectMultiScale(grayFrame, faces);
   ```

3. **Classify Expression**:
   ```csharp
   Mat faceROI = ExtractFaceRegion(frame, faceRect);
   ExpressionType expression = expressionClassifier.Classify(faceROI);
   ```

### OpenCV Advantages
- ✅ Mature, stable library
- ✅ Multiple detection algorithms
- ✅ Can use pre-trained models
- ✅ Good performance
- ✅ Extensive documentation

## Azure Face API Integration

### Setup

1. **Create Azure Resource**:
   - Go to Azure Portal
   - Create "Face" resource
   - Get API key and endpoint

2. **Configure Provider**:
   - Add API key to `AzureFaceProvider`
   - Set endpoint URL
   - Enable features (emotions, landmarks)

3. **Use Provider**:
   - Assign to `FaceDetector`
   - System automatically calls Azure API

### Implementation Steps

1. **Capture Frame**:
   ```csharp
   Texture2D frame = CaptureCameraFrame();
   byte[] imageData = frame.EncodeToJPG();
   ```

2. **Call Azure API**:
   ```csharp
   UnityWebRequest request = new UnityWebRequest(endpoint, "POST");
   request.SetRequestHeader("Ocp-Apim-Subscription-Key", apiKey);
   request.uploadHandler = new UploadHandlerRaw(imageData);
   ```

3. **Parse Response**:
   ```csharp
   AzureFaceResponse response = JsonUtility.FromJson(responseText);
   ExpressionType expression = MapEmotionToExpression(response.emotion);
   ```

### Azure Advantages
- ✅ Very high accuracy
- ✅ Built-in emotion detection
- ✅ Cloud processing (no local ML)
- ✅ Easy integration
- ✅ Handles multiple faces

### Azure Disadvantages
- ❌ Requires internet connection
- ❌ API costs (pay per request)
- ❌ Latency (network calls)
- ❌ Privacy considerations (sends images to cloud)

## Expression Analysis Methods

### Method 1: Landmark Analysis (MediaPipe/OpenCV)

Analyze facial landmarks to determine expressions:

```csharp
// Mouth curvature (smile/frown)
float mouthCurve = CalculateMouthCurvature(landmarks);
if (mouthCurve > 0.3f) return ExpressionType.Happy;
if (mouthCurve < -0.3f) return ExpressionType.Sad;

// Eye openness
float eyeOpenness = CalculateEyeOpenness(landmarks);
if (eyeOpenness > 0.8f) return ExpressionType.Surprised;

// Eyebrow position
float eyebrowHeight = CalculateEyebrowHeight(landmarks);
if (eyebrowHeight > 0.5f) return ExpressionType.Surprised;
```

### Method 2: ML Classification (OpenCV/MediaPipe)

Use pre-trained models:

```csharp
// Extract features
Mat features = ExtractFeatures(faceROI);

// Classify with model
float[] predictions = expressionModel.Predict(features);

// Map to expression
return MapToExpressionType(predictions);
```

### Method 3: Emotion Scores (Azure)

Use built-in emotion detection:

```csharp
// Azure provides emotion scores
if (emotion.happiness > 0.5f) return ExpressionType.Happy;
if (emotion.sadness > 0.5f) return ExpressionType.Sad;
if (emotion.surprise > 0.5f) return ExpressionType.Surprised;
```

## Testing Your Integration

### 1. Test Camera Access
```csharp
// Check if camera is available
if (WebCamTexture.devices.Length == 0)
{
    Debug.LogError("No camera found!");
}
```

### 2. Test Expression Detection
- Use `ExpressionDebugUI` (Press F1)
- Shows detected expressions in real-time
- Verify expressions are being detected

### 3. Test Cat Response
- Make expressions in front of camera
- Verify cat responds appropriately
- Check expression mappings

### 4. Performance Testing
- Monitor FPS with `PerformanceMonitor`
- Adjust detection interval if needed
- Optimize for your hardware

## Troubleshooting

### Camera Not Working
- Check camera permissions
- Verify camera is not used by another app
- Try different camera device
- Check Unity console for errors

### Low Accuracy
- Improve lighting conditions
- Face camera directly
- Increase detection resolution
- Use higher-quality ML model

### Performance Issues
- Reduce detection FPS
- Lower camera resolution
- Optimize analysis algorithm
- Use hardware acceleration

### Expression Not Detected
- Check confidence threshold
- Verify expression change threshold
- Ensure face is visible
- Check detection interval

## Next Steps

1. **Start with WebCam Provider** - Test basic functionality
2. **Enhance Analysis** - Add ML models or integrate SDK
3. **Fine-tune Parameters** - Adjust thresholds and sensitivity
4. **Optimize Performance** - Balance accuracy vs. speed
5. **Production Ready** - Deploy with chosen provider

## Resources

- **MediaPipe**: https://mediapipe.dev/
- **OpenCV**: https://opencv.org/
- **Azure Face API**: https://azure.microsoft.com/services/cognitive-services/face/
- **Unity WebCamTexture**: https://docs.unity3d.com/ScriptReference/WebCamTexture.html

## Summary

✅ **WebCamFaceProvider is ready to use now!**
- Real camera access
- Basic expression analysis
- Works immediately
- Can be enhanced with ML

📝 **ML Providers are templates**
- MediaPipe, OpenCV, Azure templates provided
- Follow integration steps
- Replace placeholder code
- High accuracy options available

The system is ready for real facial expression analysis! 🎉

