# How to Create Face Detection - Step by Step

## Method 1: Using Unity Menu (Easiest - 30 seconds)

### Steps:

1. **Open Unity** with your Voxon project

2. **Open your scene** (or create a new one)

3. **Go to Menu Bar** → Click `Voxon` → `Create` → `Face Detection` → `WebCam Provider`

4. **Done!** A GameObject called "FaceDetection" is created with:
   - `FaceDetector` component
   - `WebCamFaceProvider` component
   - `ExpressionRecognizer` component (auto-added)

5. **Press Play** - The system will automatically:
   - Access your webcam
   - Start detecting expressions
   - Feed data to the cat system

## Method 2: Manual Setup (More Control)

### Step 1: Create the GameObject

1. In Unity Hierarchy, **Right-click** → `Create Empty`
2. **Rename** it to `FaceDetection`

### Step 2: Add FaceDetector Component

1. **Select** the `FaceDetection` GameObject
2. In Inspector, click **Add Component**
3. Type `FaceDetector` and select it
4. Component is added!

### Step 3: Add WebCamFaceProvider

1. Still on `FaceDetection` GameObject
2. Click **Add Component** again
3. Type `WebCamFaceProvider` and select it
4. Component is added!

### Step 4: Link the Provider

1. In the `FaceDetector` component (Inspector)
2. Find the **Detection Provider** field
3. **Drag** the `WebCamFaceProvider` component into that field
   - OR: It will auto-assign if left empty

### Step 5: Configure Settings (Optional)

In the `WebCamFaceProvider` component:

- **Requested Width**: `640` (or your preferred width)
- **Requested Height**: `480` (or your preferred height)
- **Requested FPS**: `30` (frames per second)
- **Preferred Camera Name**: Leave empty to use default camera
- **Detection Interval**: `0.1` (how often to analyze)
- **Use Basic Analysis**: ✅ Checked (enables expression detection)

### Step 6: Add ExpressionRecognizer (Optional but Recommended)

1. Select `FaceDetection` GameObject
2. **Add Component** → `ExpressionRecognizer`
3. This processes detected expressions and tracks changes

### Step 7: Test It!

1. **Press Play** button
2. **Look at your webcam**
3. **Make different expressions** (smile, frown, surprised)
4. Check the **Console** for detection messages
5. If you have the cat system set up, the cat should respond!

## Method 3: Using VoxonSystemInitializer (Automatic)

### Steps:

1. **Create Empty GameObject** → Name it `SystemInitializer`

2. **Add Component** → `VoxonSystemInitializer`

3. In Inspector, check:
   - ✅ **Setup Face Detection**

4. **Press Play** - Everything is set up automatically!

## Visual Guide

```
Unity Hierarchy:
├── FaceDetection (GameObject)
│   ├── FaceDetector (Component)
│   │   └── Detection Provider: [WebCamFaceProvider]
│   ├── WebCamFaceProvider (Component)
│   │   ├── Requested Width: 640
│   │   ├── Requested Height: 480
│   │   ├── Requested FPS: 30
│   │   └── Use Basic Analysis: ✅
│   └── ExpressionRecognizer (Component)
│       ├── Confidence Threshold: 0.5
│       └── Expression Change Threshold: 0.3
```

## Quick Checklist

- [ ] Created `FaceDetection` GameObject
- [ ] Added `FaceDetector` component
- [ ] Added `WebCamFaceProvider` component
- [ ] Added `ExpressionRecognizer` component (optional)
- [ ] Linked provider to FaceDetector (or auto-assigned)
- [ ] Configured camera settings (optional)
- [ ] Pressed Play to test

## Troubleshooting

### "No cameras found"
- **Solution**: Connect a webcam or enable camera permissions
- Check: `WebCamTexture.devices.Length` in console

### "Camera failed to initialize"
- **Solution**: 
  - Check if another app is using the camera
  - Try a different camera device
  - Lower the resolution settings

### "No expressions detected"
- **Solution**:
  - Make sure you're facing the camera
  - Check lighting conditions
  - Lower the confidence threshold
  - Verify `Use Basic Analysis` is checked

### Camera permission denied
- **Solution**: 
  - Grant camera permissions in Unity
  - Check OS privacy settings
  - Restart Unity

## Testing Your Setup

### Test 1: Camera Access
1. Press Play
2. Check Console for: `"WebCamFaceProvider initialized with camera: [camera name]"`
3. Check Console for: `"WebCamFaceProvider started. Camera is ready."`

### Test 2: Expression Detection
1. Add `ExpressionDebugUI` component to any GameObject
2. Press **F1** to show debug UI
3. Make expressions - you should see "Human Expression: [type]" updating

### Test 3: Cat Response
1. Make sure `CatFaceController` is in scene
2. Make expressions at camera
3. Cat should respond with matching expressions

## Next Steps

Once face detection is working:

1. **Test expressions**: Try Happy, Sad, Surprised faces
2. **Adjust settings**: Fine-tune confidence thresholds
3. **Add debug UI**: Use `ExpressionDebugUI` to monitor
4. **Enhance accuracy**: Integrate MediaPipe/OpenCV for better detection

## Example Scene Setup

```
Scene Hierarchy:
├── Main Camera
├── EyeTrackerManager
├── GazeDetection
├── FaceDetection ← Your new face detection system!
│   ├── FaceDetector
│   ├── WebCamFaceProvider
│   └── ExpressionRecognizer
├── LLM
├── CatFace
└── VolumetricShapes
```

That's it! Your face detection system is ready! 🎉

