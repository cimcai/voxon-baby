# How to See the "Voxon" Menu in Unity

## The Problem
The "Voxon" menu doesn't appear in Unity's menu bar.

## Why This Happens
Editor scripts (in `Assets/Scripts/Editor/`) need to compile successfully before the menu appears.

## Solutions

### Solution 1: Wait for Compilation (Most Common)

1. **Check Console** - Look for compilation errors
2. **Wait** - Unity needs to finish compiling Editor scripts
3. **Look at bottom-right** - Should say "Compiling..." then "Ready"
4. **Check menu bar** - "Voxon" should appear after compilation

### Solution 2: Force Recompilation

1. **In Unity**: Assets → Reimport All
2. **Or**: Right-click `Assets/Scripts/Editor` folder → Reimport
3. **Wait** for compilation to finish
4. **Check menu** - "Voxon" should appear

### Solution 3: Check for Errors

1. **Open Console** (Window → General → Console)
2. **Look for errors** in Editor scripts
3. **Fix any errors**
4. **Menu will appear** once errors are fixed

### Solution 4: Restart Unity

1. **Close Unity** completely
2. **Reopen** the project
3. **Wait** for compilation
4. **Check menu** - "Voxon" should appear

## Manual Setup (If Menu Still Doesn't Appear)

If the menu won't appear, you can create things manually:

### Create Face Detection Manually:

1. **Create GameObject**: Right-click Hierarchy → Create Empty → Name it "FaceDetection"
2. **Add Component**: Select FaceDetection → Add Component → Type "FaceDetector" → Add
3. **Add Component**: Select FaceDetection → Add Component → Type "WebCamFaceProvider" → Add
4. **Add Component**: Select FaceDetection → Add Component → Type "ExpressionRecognizer" → Add

### Create Eye Tracker Manually:

1. **Create GameObject**: Right-click Hierarchy → Create Empty → Name it "EyeTrackerManager"
2. **Add Component**: Select EyeTrackerManager → Add Component → Type "EyeTrackerManager" → Add

### Create Shapes Manually:

1. **Create GameObject**: Right-click Hierarchy → Create Empty → Name it "VoxelCube"
2. **Add Component**: Select VoxelCube → Add Component → Type "VoxelCube" → Add

## Check if Editor Scripts Are Compiling

1. **Look at Console** - Should see compilation messages
2. **Check for errors** - Red errors prevent menu from appearing
3. **Wait for "Ready"** - Bottom-right should say "Ready"

## What the Menu Should Look Like

Once it appears, you'll see in the menu bar:
```
Voxon
├── Create
│   ├── Voxel Cube
│   ├── Voxel Sphere
│   ├── Voxel Pyramid
│   ├── Voxel Cylinder
│   ├── Eye Tracker Manager
│   ├── Cat Face Controller
│   └── Face Detection
│       └── WebCam Provider
├── Validate Scene Setup
└── Documentation
    ├── Open Setup Guide
    └── Open README
```

## Quick Test

1. **Check Console** for errors
2. **If no errors**, wait 30 seconds
3. **Look at menu bar** - "Voxon" should appear
4. **If still not there**, try Solution 2 (Reimport)

Let me know what you see in the Console!

