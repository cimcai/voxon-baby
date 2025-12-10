# Voxon - Unity Eye Tracker Integration with Volumetric Shapes and Interactive Cat Face

A Unity application that integrates eye tracking hardware to detect user gaze and highlight volumetric shapes when focused. Features an optional interactive cat face character that detects human facial expressions and evolves its responses using LLM integration.

## Features

- **Eye Tracker Integration**: Modular system supporting multiple eye tracker providers (Tobii, Pupil Labs, EyeLink, or generic simulation)
- **Volumetric Shapes**: Interactive 3D shapes (cubes, spheres, pyramids) that highlight when gazed upon
- **Gaze Detection**: Real-time raycast-based gaze detection with configurable dwell times
- **Highlighting System**: Multiple highlight effects (color change, outline, glow) that can be combined
- **Human Facial Expression Detection**: Real-time facial expression recognition from webcam
- **LLM-Powered Cat Face**: Optional cat character that responds to user emotions using LLM integration
- **Adaptive Behavior**: Cat learns and adapts its personality based on user interactions over time

## Project Structure

```
voxon/
├── Assets/
│   ├── Scripts/
│   │   ├── EyeTracker/          # Eye tracker integration layer
│   │   ├── GazeDetection/        # Gaze raycast and hit detection
│   │   ├── VolumetricShapes/    # Shape classes (Cube, Sphere, Pyramid)
│   │   ├── Highlighting/        # Highlight effect system
│   │   ├── Camera/              # Camera setup for eye tracking
│   │   ├── FaceDetection/       # Human facial expression detection
│   │   ├── LLM/                 # LLM integration and context management
│   │   └── CatFace/             # Cat face character system
│   ├── Models/                  # 3D models and ML assets
│   ├── Animations/              # Animation clips
│   ├── Materials/               # Materials and shaders
│   ├── Prefabs/                 # Prefab assets
│   └── Scenes/                  # Unity scenes
├── Packages/                    # Unity package dependencies
└── README.md                    # This file
```

## Setup Instructions

### Prerequisites

- Unity 2022.3 LTS or later
- Eye Tracker SDK (optional - generic provider included for testing)
- Webcam/Camera for facial expression detection
- LLM API access (OpenAI, Anthropic, or custom)

### Installation

1. Clone or download this repository
2. Open the project in Unity Hub
3. Unity will automatically import packages from `Packages/manifest.json`

### Configuration

#### Eye Tracker Setup

1. Add `EyeTrackerManager` to a GameObject in your scene
2. Assign an `EyeTrackerProvider` component (or use `GenericEyeTrackerProvider` for testing)
3. Configure the eye tracker settings in the inspector

#### Volumetric Shapes Setup

1. Create GameObjects with `VoxelCube`, `VoxelSphere`, or `VoxelPyramid` components
2. Ensure each shape has a Collider component (automatically added)
3. Add `HighlightController` to shapes for highlighting effects

#### Cat Face Setup (Optional)

1. Create a GameObject for the cat face
2. Add `CatFaceController` component
3. Configure expression settings in the inspector
4. Ensure you have a 3D cat face model with blend shapes or animation support

#### Face Detection Setup

1. Add `FaceDetector` component to a GameObject
2. Assign a `FaceDetectionProvider` (or use `GenericFaceDetectionProvider` for testing)
3. Configure camera/webcam settings

#### LLM Integration Setup

1. Add `LLMClient` component to a GameObject
2. Configure API settings:
   - API Key: Your LLM API key
   - API Endpoint: LLM provider endpoint
   - Model: Model name (e.g., "gpt-4", "claude-3")
3. Add `ContextManager`, `PromptBuilder`, and `LLMResponseParser` components
4. Link components in `CatFaceController`

## Usage

### Basic Eye Tracking

1. Initialize the eye tracker system (automatic on start)
2. Look at volumetric shapes - they will highlight when focused
3. Adjust dwell time and highlight settings in the inspector

### Cat Face Interaction

1. Enable the cat face in `CatFaceController`
2. The cat will detect when you look at it
3. Your facial expressions will be detected and the cat will respond
4. The cat's responses evolve over time based on your interactions

### Testing Without Hardware

- Use `GenericEyeTrackerProvider` for mouse-based gaze simulation
- Use `GenericFaceDetectionProvider` for simulated facial expressions
- LLM integration can be tested with mock responses

## Dependencies

### Required
- Unity 2022.3 LTS or later
- Unity TextMeshPro (included in manifest)

### Optional
- Eye Tracker SDK (Tobii, Pupil Labs, EyeLink, etc.)
- Face Detection Library (MediaPipe, OpenCV, Azure Face API)
- LLM API Access (OpenAI, Anthropic, etc.)
- 3D Cat Face Model with blend shapes

## Configuration Options

### Eye Tracker
- Gaze raycast distance
- Focus detection threshold (dwell time)
- Connection settings

### Shapes
- Highlight transition duration
- Highlight intensity
- Shape size and color

### Cat Face
- Expression transition duration
- Gaze interaction thresholds
- Expression trigger probabilities
- LLM request frequency

### Face Detection
- Update rate (FPS)
- Confidence threshold
- Camera device selection

### LLM
- API key and endpoint
- Model selection
- Context history size
- Response evolution rate

## Architecture

The system uses a modular architecture:

- **Eye Tracker Layer**: Abstracts different eye tracker hardware
- **Gaze Detection**: Performs raycasts and detects hits
- **Shape System**: Manages volumetric shapes and highlighting
- **Face Detection**: Detects and recognizes human expressions
- **LLM Integration**: Generates contextual responses
- **Cat Face System**: Manages cat character and expressions

See the plan document for detailed architecture diagrams.

## Development

### Adding New Eye Tracker Providers

1. Create a new class inheriting from `EyeTrackerProvider`
2. Implement `Initialize()`, `Connect()`, `Disconnect()`, and `GetGazeData()`
3. Assign to `EyeTrackerManager`

### Adding New Shape Types

1. Create a new class inheriting from `VolumetricShape`
2. Implement shape-specific geometry creation
3. Add to scene as needed

### Adding New Highlight Effects

1. Create a new class inheriting from `HighlightEffect`
2. Implement `ApplyHighlight()` and `RemoveHighlight()`
3. Add to `HighlightController` component

### Customizing Cat Expressions

1. Modify `ExpressionTypes` enum
2. Update `CatFaceExpression` blend shape values
3. Adjust `LLMExpressionMapper` mapping logic

## Testing

1. Use generic providers for development without hardware
2. Test with actual eye tracker hardware
3. Validate highlight effects and transitions
4. Test LLM integration with mock responses
5. Verify context management and evolution

## Troubleshooting

### Eye Tracker Not Connecting
- Check SDK installation
- Verify provider initialization
- Use generic provider for testing

### Shapes Not Highlighting
- Ensure shapes have colliders
- Check gaze raycast distance
- Verify highlight components are attached

### Cat Face Not Responding
- Check LLM API configuration
- Verify face detection is running
- Ensure components are linked correctly

### Face Detection Not Working
- Check camera/webcam permissions
- Verify provider initialization
- Use generic provider for testing

## License

[Specify your license here]

## Contributing

[Contributing guidelines if applicable]

## References

This project incorporates research on cat facial expressions:

- **CatFACS (Cat Facial Action Coding System)**: Caeiro, C. C., Burrows, A. M., & Waller, B. M. (2017). Development and application of CatFACS. Applied Animal Behaviour Science, 173, 42-48.
- **Human Recognition of Cat Emotions**: Research demonstrating humans can identify cats' affective states from subtle facial expressions.
- **Comprehensive Feline Ethogram**: Structured approach to understanding cat behaviors and expressions.

See [REFERENCES.md](REFERENCES.md) for complete citations and research details.

## Acknowledgments

- Unity Technologies
- Eye Tracker SDK providers
- LLM API providers
- CatFACS research team and related feline behavior research

