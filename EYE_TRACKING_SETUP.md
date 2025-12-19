# Eye Tracking Hardware Setup Guide

This guide explains how to configure real eye tracking hardware instead of mouse simulation.

**⚠️ Mac Users**: See `MAC_EYE_TRACKING_SETUP.md` for Mac-specific setup. Many eye tracking SDKs have limited Mac support. Pupil Labs is recommended for Mac.

## Current Setup

By default, the system uses `GenericEyeTrackerProvider` which simulates eye tracking using mouse position. To use real hardware, you need to:

1. **Choose your eye tracker hardware**
2. **Install the SDK** for that hardware
3. **Configure the EyeTrackerManager** to use the real provider

## Supported Eye Trackers

### 1. Tobii Eye Trackers
- **Hardware**: Tobii EyeX, Tobii 4C, Tobii Pro, etc.
- **SDK**: Tobii Unity SDK
- **Provider**: `TobiiEyeTrackerProvider` (template included, needs SDK integration)

### 2. Pupil Labs
- **Hardware**: Pupil Core, Pupil Invisible
- **SDK**: Pupil Labs Unity Plugin
- **Provider**: Need to create `PupilLabsEyeTrackerProvider`

### 3. SR Research EyeLink
- **Hardware**: EyeLink 1000, EyeLink Portable Duo
- **SDK**: SR Research Unity SDK
- **Provider**: Need to create `EyeLinkEyeTrackerProvider`

### 4. Other Hardware
- You can create custom providers by extending `EyeTrackerProvider`

## Setup Steps

### Step 1: Install Eye Tracker SDK

**For Tobii:**
1. Download Tobii Unity SDK from [Tobii Developer Portal](https://developer.tobii.com/)
2. Import the SDK package into Unity
3. Follow Tobii's setup instructions

**For Pupil Labs:**
1. Install Pupil Capture software
2. Install Pupil Labs Unity Plugin
3. Connect your Pupil hardware

**For EyeLink:**
1. Install EyeLink SDK
2. Import Unity package
3. Configure EyeLink software

### Step 2: Configure EyeTrackerManager

1. **Select EyeTrackerManager** in Hierarchy
2. **In Inspector**, find "Eye Tracker Settings"
3. **Remove GenericEyeTrackerProvider**:
   - If you see `GenericEyeTrackerProvider` component, remove it
4. **Add your hardware provider**:
   - Click "Add Component"
   - Search for your provider (e.g., "TobiiEyeTrackerProvider")
   - Add it
5. **Assign to EyeTrackerManager**:
   - In EyeTrackerManager component
   - Drag your provider component to the "Eye Tracker Provider" field
   - OR: The system will auto-detect if only one provider exists

### Step 3: Configure Provider Settings

Each provider has different settings:

**TobiiEyeTrackerProvider:**
- Device Name (optional, leave empty for auto-detect)
- Auto Connect (recommended: true)

**Pupil Labs:**
- IP Address (default: 127.0.0.1)
- Port (default: 50020)

**EyeLink:**
- EDF File Name
- Sample Rate

### Step 4: Test Connection

1. **Make sure your eye tracker hardware is:**
   - Connected via USB
   - Powered on
   - Software/drivers installed
   - Calibrated (if required)

2. **Press Play** in Unity

3. **Check Console** for connection messages:
   - ✅ "Eye tracker initialized successfully"
   - ✅ "Eye tracker connected successfully"
   - ❌ If errors, check hardware connection and SDK setup

## Quick Setup (Tobii Example)

1. **Install Tobii SDK** in Unity
2. **Open your scene**
3. **Select EyeTrackerManager** GameObject
4. **Remove GenericEyeTrackerProvider** component
5. **Add Component** → `TobiiEyeTrackerProvider`
6. **In EyeTrackerManager**, the provider should auto-assign
7. **Press Play** - should connect to Tobii hardware

## Troubleshooting

### "Eye tracker not connected"
- Check USB connection
- Verify hardware is powered on
- Check if SDK/drivers are installed
- Look at Console for specific error messages

### "Provider not found"
- Make sure you've added the provider component
- Check that the SDK is properly imported
- Verify the provider script compiles without errors

### "Failed to initialize"
- Check SDK documentation for initialization requirements
- Verify API keys/licenses if required
- Check Console for specific error details

## Creating Custom Providers

If your eye tracker isn't listed, you can create a custom provider:

1. **Create new script** extending `EyeTrackerProvider`
2. **Implement required methods**:
   - `Initialize()` - Set up SDK connection
   - `Connect()` - Connect to hardware
   - `Disconnect()` - Clean up connection
   - `GetGazeData()` - Return current gaze data

3. **Example structure:**
```csharp
public class MyEyeTrackerProvider : EyeTrackerProvider
{
    public override bool Initialize()
    {
        // Initialize your SDK here
        return true;
    }
    
    public override bool Connect()
    {
        // Connect to hardware
        isConnected = true;
        return true;
    }
    
    public override void Disconnect()
    {
        // Disconnect from hardware
        isConnected = false;
    }
    
    public override GazeData GetGazeData()
    {
        // Get gaze data from your SDK
        // Return GazeData with origin, direction, timestamp, isValid
        return new GazeData(origin, direction, true);
    }
}
```

## Disabling Mouse Simulation

To ensure mouse simulation doesn't interfere:

1. **Remove GenericEyeTrackerProvider** from EyeTrackerManager GameObject
2. **Make sure your hardware provider is assigned** in EyeTrackerManager
3. **Test with hardware disconnected** - should show "not connected" not mouse simulation

## Next Steps

Once your eye tracker is connected:
- Test gaze detection on volumetric shapes
- Verify gaze raycast is working
- Check that shapes highlight when you look at them
- Test with face detection for full interaction

## Need Help?

- Check your eye tracker manufacturer's Unity SDK documentation
- Look at `TobiiEyeTrackerProvider.cs` as a template
- Check Console for specific error messages
- Verify hardware is working in manufacturer's software first

