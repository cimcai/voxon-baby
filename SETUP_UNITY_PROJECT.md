# Setting Up Unity Project

## The Problem
Unity Hub won't recognize the folder because it's missing the `ProjectSettings` folder.

## Solution: Create New Unity Project and Copy Scripts

### Option 1: Create New Unity Project (Recommended)

1. **Open Unity Hub** on your Mac

2. **Click "New Project"**
   - Template: **3D (Core)**
   - Project Name: `voxon-baby`
   - Location: Choose a location (or use default)
   - Click **Create**

3. **Wait for Unity to open** (first time takes a few minutes)

4. **Copy the Scripts**:
   - In Finder, navigate to: `/Users/jdietz/Documents/GitHub/voxon-baby/Assets/Scripts`
   - Copy the entire `Scripts` folder
   - In Unity, go to `Assets` folder in Project window
   - Right-click → `Show in Finder`
   - Paste the `Scripts` folder there (replace if asked)

5. **Unity will automatically compile** the scripts

6. **Done!** Your project is ready

### Option 2: Fix Current Folder (Advanced)

If you want to keep using the current folder, you need to create a Unity project there:

1. **Open Unity Hub**

2. **Click "New Project"** but DON'T create yet

3. **Instead, in Terminal** (Mac):
   ```bash
   cd /Users/jdietz/Documents/GitHub/voxon-baby
   ```

4. **Create minimal ProjectSettings**:
   ```bash
   mkdir -p ProjectSettings
   echo "m_EditorVersion: 2022.3.0f1" > ProjectSettings/ProjectVersion.txt
   ```

5. **Then in Unity Hub**:
   - Click "Open"
   - Navigate to `/Users/jdietz/Documents/GitHub/voxon-baby`
   - Click "Open"

6. **Unity will generate** the rest of ProjectSettings automatically

## Quick Method (Easiest)

**Just create a new Unity project and copy the Scripts folder over!**

1. Unity Hub → New Project → 3D Core → Name: `voxon-baby`
2. Copy `/Users/jdietz/Documents/GitHub/voxon-baby/Assets/Scripts` to new project's `Assets` folder
3. Done!

## After Setup

Once Unity recognizes your project:

1. **Create a scene**: File → New Scene → Basic (Built-in)
2. **Create face detection**: Menu → `Voxon` → `Create` → `Face Detection` → `WebCam Provider`
3. **Press Play** ▶️

That's it!

