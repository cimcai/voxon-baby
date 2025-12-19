# Mac Eye Tracking Setup Guide

## Mac-Compatible Eye Trackers

Unfortunately, many eye tracking SDKs have limited Mac support. Here are your best options:

### ✅ **Pupil Labs** (Recommended for Mac)
- **Fully Mac compatible** (macOS 10.14+)
- **Open source** and well-documented
- **Works with**: Pupil Core, Pupil Invisible
- **Connection**: Network-based (ZMQ/WebSocket)
- **Provider**: `PupilLabsEyeTrackerProvider` (included)

### ✅ **SR Research EyeLink**
- **Mac support**: Yes (with limitations)
- **Professional grade** eye tracker
- **Connection**: USB/Network
- **Provider**: Need to create `EyeLinkEyeTrackerProvider`

### ⚠️ **Tobii**
- **Mac support**: Limited (Tobii Pro SDK 2.1+ supports macOS 13+)
- **Issues**: Drivers and software may not work properly
- **Workaround**: Use Windows VM (not officially supported)

## Recommended: Pupil Labs Setup

### Step 1: Install Pupil Capture

1. **Download Pupil Capture**:
   - Visit: https://pupil-labs.com/products/core/
   - Download macOS version
   - Install the application

2. **Hardware Setup**:
   - Connect your Pupil Labs hardware (USB)
   - Power on the device
   - Launch Pupil Capture

### Step 2: Configure Pupil Capture

1. **Enable Network API**:
   - In Pupil Capture, go to Settings
   - Enable "Network API" or "ZMQ"
   - Note the IP address and port (default: 127.0.0.1:50020)

2. **Calibrate**:
   - Follow Pupil Capture's calibration process
   - Make sure gaze data is being recorded

### Step 3: Configure Unity

1. **Select EyeTrackerManager** in Hierarchy

2. **Remove GenericEyeTrackerProvider**:
   - Remove the mouse simulation provider

3. **Add PupilLabsEyeTrackerProvider**:
   - Click "Add Component"
   - Search for "PupilLabsEyeTrackerProvider"
   - Add it

4. **Configure Settings**:
   - IP Address: `127.0.0.1` (or your Pupil Capture IP)
   - Port: `50020` (or your configured port)
   - Auto Connect: `true`

5. **Assign to EyeTrackerManager**:
   - In EyeTrackerManager component
   - Drag PupilLabsEyeTrackerProvider to "Eye Tracker Provider" field

### Step 4: Test Connection

1. **Make sure Pupil Capture is running**
2. **Press Play** in Unity
3. **Check Console** for connection messages

## Alternative: Network-Based Provider

If you have eye tracking software running on another machine:

1. **Create a network provider** that connects via TCP/UDP
2. **Send gaze data** from the eye tracking software to Unity
3. **Parse the data** and convert to GazeData format

## Troubleshooting Mac Issues

### "Connection refused" or "Cannot connect"
- Check that Pupil Capture is running
- Verify Network API is enabled in Pupil Capture
- Check firewall settings (allow Unity/Pupil on port 50020)
- Try `127.0.0.1` instead of `localhost`

### "No gaze data received"
- Check Pupil Capture shows gaze data in its interface
- Verify calibration is complete
- Check that gaze topic is subscribed in Pupil Capture

### Tobii SDK Issues
- If using Tobii Pro SDK 2.1+, make sure you're on macOS 13+
- Check Tobii documentation for Mac-specific setup
- Consider using Pupil Labs instead for better Mac support

## Using ZMQ Library (Advanced)

For better Pupil Labs integration, you can use the ZMQ library:

1. **Install ZMQ**:
   ```bash
   brew install zeromq
   ```

2. **Add ZMQ Unity package**:
   - Use a ZMQ Unity plugin
   - Or use WebSocket instead (simpler)

3. **Update PupilLabsEyeTrackerProvider**:
   - Replace UDP with ZMQ connection
   - Parse Pupil Labs ZMQ messages properly

## Quick Test Without Hardware

If you don't have hardware yet but want to test:

1. **Keep GenericEyeTrackerProvider** (mouse simulation)
2. **Test the system** with mouse
3. **Switch to real provider** when hardware arrives

## Next Steps

Once connected:
- Test gaze detection on shapes
- Verify gaze raycast accuracy
- Calibrate if needed
- Test with face detection for full interaction

## Need Help?

- Pupil Labs Docs: https://docs.pupil-labs.com/
- Pupil Labs Forum: https://github.com/pupil-labs/pupil/discussions
- Check Console for specific error messages
- Verify Pupil Capture is working first before Unity integration

