# Quick Test Setup: Cat Face + Eye Tracking

This guide helps you quickly test the full system with mouse simulation.

## Quick Setup Steps

### Step 1: Create/Open Your Scene
1. **File → New Scene** (or use existing scene)
2. **Save** the scene (Cmd+S)

### Step 2: Set Up Core Systems

#### A. Eye Tracker (Mouse Simulation)
1. **Voxon → Create → Eye Tracker Manager**
   - This creates EyeTrackerManager with GenericEyeTrackerProvider (mouse simulation)
   - ✅ Already configured for mouse!

#### B. Gaze Detection
1. **Voxon → Create → Gaze Detection**
   - Creates GazeDetection GameObject with GazeRaycast and GazeHitDetector
   - ✅ Ready to detect what you're "looking at"

#### C. Face Detection (Webcam)
1. **Voxon → Create → Face Detection → WebCam Provider**
   - Creates FaceDetection GameObject with all needed components
   - ⚠️ **Make sure your webcam is connected and permissions granted**

#### D. Create Test Shapes
1. **Voxon → Create → Voxel Cube** (create 2-3 cubes)
2. **Voxon → Create → Voxel Sphere** (create 1-2 spheres)
3. **Arrange them** in front of the camera in Scene view

#### E. Cat Face Controller
1. **Voxon → Create → Cat Face Controller**
   - Creates CatFace GameObject
   - ⚠️ **Note**: You'll need a 3D cat model with blend shapes for full functionality
   - For testing, it will work but won't show visual changes without a model

#### F. LLM Integration (Optional but Recommended)
1. **Create Empty GameObject** → Name it "LLM"
2. **Add Component** → `LLMClient`
3. **Add Component** → `ContextManager`
4. **Add Component** → `PromptBuilder`
5. **Add Component** → `LLMResponseParser`
6. **Configure LLMClient**:
   - Set your API Key (OpenAI or Anthropic)
   - Set API Endpoint
   - Set Model name

### Step 3: Verify Setup

1. **Voxon → Validate Scene Setup**
   - Should show what's found/missing
   - Check that all systems are detected

### Step 4: Test in Play Mode

1. **Press Play**
2. **Move your mouse** - should simulate eye tracking
3. **Look at shapes** - they should highlight when mouse hovers
4. **Look at webcam** - face detection should analyze your expressions
5. **Check Console** - should see face detection messages

## What Should Happen

### Eye Tracking (Mouse)
- Move mouse around screen
- Gaze raycast follows mouse position
- Shapes highlight when mouse hovers over them

### Face Detection
- Webcam analyzes your face
- Detects expressions (Happy, Sad, Neutral, etc.)
- Sends expression data to cat face system

### Cat Face Response
- Cat face receives:
  - Your facial expressions
  - What you're looking at (gaze data)
- Cat responds with appropriate expressions
- If LLM is configured, cat gets "smarter" responses

### Shape Interaction
- Look at shapes (move mouse over them)
- Shapes highlight with color/glow
- Dwell time triggers focus events

## Troubleshooting

### "No face detected"
- Check webcam permissions: System Preferences → Security & Privacy → Camera
- Make sure webcam is connected
- Check Console for webcam initialization errors

### "Cat face not responding"
- Check if CatFaceController is in scene
- Check Console for errors
- Make sure ExpressionManager component exists on CatFace GameObject

### "Shapes not highlighting"
- Check GazeDetection GameObject exists
- Verify GazeRaycast component is present
- Check that shapes have Colliders (should be automatic)

### "LLM errors"
- Check API key is set correctly
- Verify internet connection
- Check Console for specific API errors
- LLM is optional - system works without it

## Testing Checklist

- [ ] Eye Tracker Manager created
- [ ] Gaze Detection created
- [ ] Face Detection created
- [ ] At least 2-3 shapes created
- [ ] Cat Face Controller created
- [ ] Press Play - no errors in Console
- [ ] Move mouse - shapes highlight
- [ ] Webcam detects face
- [ ] Console shows expression detection

## Next Steps After Testing

1. **Add a 3D cat model** with blend shapes for visual cat expressions
2. **Configure LLM API** for smarter cat responses
3. **Add more shapes** for richer interaction
4. **Calibrate face detection** sensitivity if needed
5. **Test with real eye tracker** when hardware is available

## Quick Test Scene Setup (All-in-One)

If you want everything set up automatically:

1. **Create Empty GameObject** → Name it "VoxonSystemInitializer"
2. **Add Component** → `VoxonSystemInitializer`
3. **In Inspector**, enable:
   - ✅ Setup Eye Tracker
   - ✅ Setup Gaze Detection
   - ✅ Setup Face Detection
   - ✅ Setup Cat Face
   - ✅ Create Example Shapes
4. **Press Play** - everything should be created automatically!

## What to Watch For

**In Console:**
- "Eye tracker initialized successfully"
- "Face detection started"
- Expression detection messages
- Any errors (red text)

**In Game View:**
- Shapes visible
- Shapes highlight when mouse hovers
- Webcam feed (if WebCamFaceProvider shows it)

**In Scene View:**
- All GameObjects created
- Components attached correctly

