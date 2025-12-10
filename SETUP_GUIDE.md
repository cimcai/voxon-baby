# Voxon Setup Guide

This guide will help you set up and configure the Voxon system in Unity.

## Quick Start

### Option 1: Automatic Setup (Recommended for First Time)

1. Open Unity and create a new scene or open an existing one
2. Create an empty GameObject in your scene
3. Add the `VoxonSystemInitializer` component to it
4. Configure the initializer:
   - Enable "Setup Eye Tracker"
   - Enable "Setup Gaze Detection"
   - Enable "Setup Face Detection"
   - Enable "Setup LLM" (remember to add API key later)
   - Optionally enable "Setup Cat Face" if you have a cat model
   - Optionally enable "Create Example Shapes" to spawn test shapes
5. Press Play - the system will automatically set up all components

### Option 2: Manual Setup

Follow the detailed instructions below for manual component setup.

## Detailed Setup Instructions

### 1. Camera Setup

**Automatic:**
- The `VoxonSystemInitializer` will set up the camera automatically

**Manual:**
1. Ensure you have a Main Camera in your scene
2. Add `EyeTrackerCamera` component to the Main Camera
3. Configure camera position and settings as needed

### 2. Eye Tracker Setup

**Automatic:**
- `EyeTrackerManager` is created automatically as a singleton

**Manual:**
1. Create an empty GameObject named "EyeTrackerManager"
2. Add `EyeTrackerManager` component
3. The system will automatically create a `GenericEyeTrackerProvider` if none is assigned
4. For hardware eye trackers, replace `GenericEyeTrackerProvider` with your specific provider

**Testing Without Hardware:**
- `GenericEyeTrackerProvider` uses mouse position to simulate gaze
- Move your mouse to simulate eye tracking
- Works immediately without any configuration

### 3. Gaze Detection Setup

**Automatic:**
- Creates "GazeDetection" GameObject with required components

**Manual:**
1. Create an empty GameObject named "GazeDetection"
2. Add `GazeRaycast` component
3. Add `GazeHitDetector` component
4. Configure raycast distance and layer masks in the inspector

**Optional - Gaze Visualization:**
- Add `GazeVisualizer` component to any GameObject to see the gaze ray in the scene
- Useful for debugging and calibration

### 4. Volumetric Shapes Setup

**Automatic:**
- Enable "Create Example Shapes" in `VoxonSystemInitializer`

**Manual:**
1. Create a GameObject for your shape
2. Add one of: `VoxelCube`, `VoxelSphere`, or `VoxelPyramid`
3. The shape will automatically get a collider and `HighlightController`
4. Configure highlight effects in the `HighlightController`:
   - Color Highlight (default)
   - Outline Highlight
   - Glow Highlight

**Runtime Spawning:**
- Add `ShapeSpawner` component to any GameObject
- Press 1, 2, or 3 to spawn cubes, spheres, or pyramids
- Press C to clear all spawned shapes

### 5. Face Detection Setup

**Automatic:**
- Creates "FaceDetection" GameObject with required components

**Manual:**
1. Create an empty GameObject named "FaceDetection"
2. Add `FaceDetector` component
3. Add `ExpressionRecognizer` component
4. The system will automatically create a `GenericFaceDetectionProvider` if none is assigned

**Testing Without Hardware:**
- `GenericFaceDetectionProvider` simulates random expressions
- Useful for testing the cat face response system

**With Real Face Detection:**
- Replace `GenericFaceDetectionProvider` with your face detection SDK provider
- Configure camera/webcam settings

### 6. LLM Integration Setup

**Automatic:**
- Creates "LLM" GameObject with all required components

**Manual:**
1. Create an empty GameObject named "LLM"
2. Add `LLMClient` component
3. Add `ContextManager` component
4. Add `PromptBuilder` component
5. Add `LLMResponseParser` component
6. Add `ResponseEvolution` component

**Configuration:**
1. Select the `LLMClient` component
2. Enter your API Key in the inspector
3. Set API Endpoint (default: OpenAI)
4. Select your model (e.g., "gpt-4", "gpt-3.5-turbo")
5. Configure request cooldown to limit API calls

**Supported Providers:**
- OpenAI (default)
- Anthropic
- Custom endpoints

### 7. Cat Face Setup (Optional)

**Prerequisites:**
- A 3D cat face model with blend shapes OR an Animator Controller

**Automatic:**
- Creates "CatFace" GameObject with `CatFaceController`

**Manual:**
1. Import your cat face model into Unity
2. Create a GameObject for the cat face
3. Add `CatFaceController` component
4. Assign the cat face model's SkinnedMeshRenderer to `ExpressionManager`
5. Configure blend shape indices in `ExpressionManager`:
   - Eye blend shapes (blink, half-blink, wide)
   - Ear blend shapes (forward, backward, flattened)
   - Mouth blend shapes (open, smile, frown)

**Blend Shape Configuration:**
1. Select your cat face model in the scene
2. Open the `ExpressionManager` component
3. In the inspector, expand "Blend Shape Mapping"
4. For each expression type, set the blend shape indices:
   - Check your model's blend shape names in the SkinnedMeshRenderer
   - Map them to the appropriate indices
   - Example: If "Eye_Wide" is blend shape index 5, set `eyeBlendShapeIndices[2] = 5`

**Alternative - Animator Setup:**
1. Create an Animator Controller with expression states
2. Add integer parameter "ExpressionType" (0-7 for each expression)
3. Add float parameter "Intensity" (0-1)
4. Enable "Use Animator" in `ExpressionManager`
5. Assign your Animator Controller to the cat face GameObject

## Testing and Debugging

### Debug UI
- Add `ExpressionDebugUI` component to any GameObject
- Press F1 to toggle debug information
- Shows current cat and human expressions, eye tracker status

### Performance Monitor
- Add `PerformanceMonitor` component to any GameObject
- Press F2 to toggle performance display
- Shows FPS and memory usage

### Gaze Visualization
- Add `GazeVisualizer` component to see gaze rays
- Red line shows gaze direction
- Red sphere shows hit point

## Common Issues and Solutions

### Shapes Not Highlighting
- Ensure shapes have colliders (automatically added)
- Check that `GazeHitDetector` is in the scene
- Verify eye tracker is connected (check debug UI)
- Adjust raycast distance in `GazeRaycast`

### Cat Face Not Responding
- Check that `CatFaceController` is enabled
- Verify face detection is running (check debug UI)
- Ensure LLM API key is configured
- Check that blend shape indices are correct

### Eye Tracker Not Working
- If using `GenericEyeTrackerProvider`, move your mouse
- Check that `EyeTrackerManager` exists in scene
- Verify camera is tagged as "MainCamera"

### LLM Not Responding
- Verify API key is correct
- Check internet connection
- Review request cooldown settings
- Check Unity console for error messages

## Next Steps

1. **Customize Expressions**: Adjust blend shape mappings for your cat model
2. **Add More Shapes**: Create custom volumetric shapes
3. **Integrate Real Hardware**: Replace generic providers with actual SDKs
4. **Fine-tune Settings**: Adjust dwell times, highlight intensities, etc.
5. **Create Scenes**: Build your application scenes with the configured systems

## Example Scene Structure

```
Scene
├── Main Camera (EyeTrackerCamera)
├── EyeTrackerManager (EyeTrackerManager, GenericEyeTrackerProvider)
├── GazeDetection (GazeRaycast, GazeHitDetector)
├── FaceDetection (FaceDetector, ExpressionRecognizer, GenericFaceDetectionProvider)
├── LLM (LLMClient, ContextManager, PromptBuilder, LLMResponseParser, ResponseEvolution)
├── CatFace (CatFaceController, ExpressionManager, GazeInteractionHandler, ExpressionTriggers, LLMExpressionMapper)
├── VolumetricShapes
│   ├── VoxelCube_0
│   ├── VoxelSphere_1
│   └── VoxelPyramid_2
└── Utilities (GazeVisualizer, ExpressionDebugUI, PerformanceMonitor, ShapeSpawner)
```

## Tips

- Start with `GenericEyeTrackerProvider` and `GenericFaceDetectionProvider` for testing
- Use the debug UI to monitor system status
- Test one system at a time before integrating everything
- Save your scene frequently during setup
- Create prefabs of configured GameObjects for reuse

