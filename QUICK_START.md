# Voxon Quick Start Guide

Get up and running with Voxon in 5 minutes!

## Method 1: Unity Editor Wizard (Easiest)

1. **Open Unity** and create/open a scene
2. **Menu**: `Voxon > Setup Wizard`
3. **Configure**:
   - ✅ Setup Eye Tracker
   - ✅ Setup Gaze Detection  
   - ✅ Setup Face Detection
   - ✅ Setup LLM (remember to add API key!)
   - ✅ Create Example Shapes
   - ✅ Add Debug UI
4. **Click "Setup"**
5. **Press Play** - Move your mouse to simulate eye tracking!

## Method 2: Automatic Initializer

1. **Create** an empty GameObject
2. **Add Component**: `VoxonSystemInitializer`
3. **Configure** in Inspector:
   - Enable systems you want
   - Set number of example shapes
4. **Press Play**

## Method 3: Manual Setup

### Minimum Setup (Eye Tracking + Shapes)

1. **Eye Tracker**: `Voxon > Create > Eye Tracker Manager`
2. **Gaze Detection**: Create GameObject, add `GazeRaycast` and `GazeHitDetector`
3. **Shapes**: `Voxon > Create > Voxel Cube` (repeat for Sphere, Pyramid, Cylinder)
4. **Press Play** and move mouse!

### Full Setup (All Systems)

1. Follow Minimum Setup above
2. **Face Detection**: Create GameObject, add `FaceDetector` and `ExpressionRecognizer`
3. **LLM**: Create GameObject, add:
   - `LLMClient` (add API key!)
   - `ContextManager`
   - `PromptBuilder`
   - `LLMResponseParser`
   - `ResponseEvolution`
4. **Cat Face** (optional): `Voxon > Create > Cat Face Controller`

## Testing

### Test Eye Tracking
- Move mouse → Shapes should highlight when mouse hovers
- Add `GazeVisualizer` component to see gaze ray

### Test Expressions
- Add `ExpressionTester` component
- Press `E` to cycle through cat expressions
- Press `Q` for previous expression

### Test LLM
- Add `LLMTestClient` component  
- Press `T` to send test request
- Check console for response

### Debug Tools
- **F1**: Toggle debug UI (shows expressions, status)
- **F2**: Toggle performance monitor
- **1/2/3**: Spawn shapes (if `ShapeSpawner` added)
- **C**: Clear spawned shapes

## Common Issues

**Shapes not highlighting?**
- Check `GazeHitDetector` exists in scene
- Verify eye tracker is connected (check debug UI - F1)
- Ensure shapes have colliders (auto-added)

**LLM not working?**
- Add API key to `LLMClient` component
- Check internet connection
- Review console for errors

**Cat face not responding?**
- Assign cat face model to `ExpressionManager`
- Configure blend shape indices
- Check face detection is running

## Next Steps

1. **Customize**: Adjust dwell times, highlight colors, expression durations
2. **Add Shapes**: Use `ShapeSpawner` or create manually
3. **Integrate Hardware**: Replace generic providers with real SDKs
4. **Build**: Create your application!

## Keyboard Shortcuts

| Key | Action |
|-----|--------|
| F1 | Toggle Debug UI |
| F2 | Toggle Performance Monitor |
| E | Next Expression (ExpressionTester) |
| Q | Previous Expression (ExpressionTester) |
| T | Test LLM (LLMTestClient) |
| 1 | Spawn Cube (ShapeSpawner) |
| 2 | Spawn Sphere (ShapeSpawner) |
| 3 | Spawn Pyramid (ShapeSpawner) |
| C | Clear Shapes (ShapeSpawner) |

## Example Scene Structure

```
Scene
├── Main Camera
├── EyeTrackerManager
├── GazeDetection
│   ├── GazeRaycast
│   └── GazeHitDetector
├── FaceDetection (optional)
│   ├── FaceDetector
│   └── ExpressionRecognizer
├── LLM (optional)
│   ├── LLMClient
│   ├── ContextManager
│   └── ...
├── CatFace (optional)
│   └── CatFaceController
└── VolumetricShapes
    ├── VoxelCube_0
    ├── VoxelSphere_1
    └── ...
```

That's it! You're ready to build with Voxon! 🚀

