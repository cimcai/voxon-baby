# Voxon System Verification Checklist

Follow these steps in order to verify your setup is working correctly.

## ✅ Step 1: Verify Compilation
- [ ] Open Unity Console (Window → General → Console)
- [ ] Check that there are **0 errors** (warnings are OK)
- [ ] Bottom-right should say **"Ready"** (not "Compiling...")

## ✅ Step 2: Verify Voxon Menu
- [ ] Look at the top menu bar in Unity
- [ ] You should see **"Voxon"** menu item
- [ ] Click it to see submenus:
  - Create (shapes, eye tracker, face detection, etc.)
  - Validate Scene Setup
  - Documentation

## ✅ Step 3: Create a New Scene
- [ ] File → New Scene → Basic (Built-in)
- [ ] Save the scene (Ctrl+S / Cmd+S) as "VoxonTest"

## ✅ Step 4: Set Up Basic Systems

### 4a. Create Eye Tracker Manager
- [ ] Voxon → Create → Eye Tracker Manager
- [ ] Check Hierarchy - should see "EyeTrackerManager" GameObject
- [ ] Select it and check Inspector - should have EyeTrackerManager component

### 4b. Create Gaze Detection
- [ ] Voxon → Create → Gaze Detection
- [ ] Check Hierarchy - should see "GazeDetection" GameObject
- [ ] Select it - should have:
  - GazeRaycast component
  - GazeHitDetector component

### 4c. Create Face Detection
- [ ] Voxon → Create → Face Detection → WebCam Provider
- [ ] Check Hierarchy - should see "FaceDetection" GameObject
- [ ] Select it - should have:
  - FaceDetector component
  - WebCamFaceProvider component
  - ExpressionRecognizer component

### 4d. Create a Test Shape
- [ ] Voxon → Create → Voxel Cube
- [ ] Check Hierarchy - should see "VoxelCube" GameObject
- [ ] Select it - should have VoxelCube component
- [ ] In Scene view, you should see a cube

## ✅ Step 5: Validate Scene Setup
- [ ] Voxon → Validate Scene Setup
- [ ] A dialog should appear showing what's found/missing
- [ ] Should show ✓ for components you created

## ✅ Step 6: Test in Play Mode

### 6a. Basic Test
- [ ] Press **Play** button (top center)
- [ ] Check Console for any runtime errors
- [ ] Press **Play** again to stop

### 6b. Test Face Detection (if you have a webcam)
- [ ] Press Play
- [ ] Look at the camera
- [ ] Check Console - should see face detection messages
- [ ] Check Game view - might see webcam feed (depends on implementation)

### 6c. Test Eye Tracker (if you have eye tracker hardware)
- [ ] Connect your eye tracker
- [ ] Press Play
- [ ] Move your eyes - should see gaze detection working

## ✅ Step 7: Check Console for Issues
- [ ] Open Console (Window → General → Console)
- [ ] Look for:
  - ❌ Red errors = fix these
  - ⚠️ Yellow warnings = usually OK, but check them
  - ℹ️ Blue info = normal operation messages

## ✅ Step 8: Test LLM Integration (Optional)
- [ ] Create Empty GameObject → Name it "LLM"
- [ ] Add Component → Type "LLMClient" → Add
- [ ] In Inspector, set your API key (if you have one)
- [ ] Add Component → Type "ContextManager" → Add
- [ ] Add Component → Type "PromptBuilder" → Add

## ✅ Step 9: Test Cat Face (Optional)
- [ ] Voxon → Create → Cat Face Controller
- [ ] Check Hierarchy - should see "CatFace" GameObject
- [ ] Note: You'll need a 3D cat model with blend shapes for this to work fully

## 🎯 Quick Test Order Summary

1. **Compilation** - No errors
2. **Menu** - Voxon menu appears
3. **Scene** - Create new scene
4. **Components** - Create Eye Tracker, Face Detection, Shape
5. **Validation** - Run validation tool
6. **Play Mode** - Test basic functionality
7. **Console** - Check for runtime errors

## 🐛 Common Issues

### Menu doesn't appear
- Wait for compilation to finish
- Check Console for Editor script errors
- Try: Assets → Reimport All

### Components don't appear when created
- Check Hierarchy window
- Check if GameObject was created but component missing
- Try adding component manually

### Runtime errors in Play Mode
- Check Console for specific error messages
- Make sure all required components are in scene
- Check that Main Camera exists (should be there by default)

### Face Detection not working
- Check webcam permissions (macOS: System Preferences → Security & Privacy → Camera)
- Make sure WebCamFaceProvider component is on FaceDetection GameObject
- Check Console for webcam initialization errors

## 📝 Next Steps After Verification

Once everything checks out:
1. Read `QUICK_START.md` for usage instructions
2. Read `SETUP_GUIDE.md` for detailed setup
3. Read `FACE_DETECTION_GUIDE.md` for face detection details
4. Start building your scene!

