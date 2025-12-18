# Troubleshoot Unity Crash

## If Unity Crashes Even With Fresh Project

### Step 1: Test With Minimal Scripts First

**Don't import all scripts at once!** Import gradually:

1. **Create fresh Unity project** (3D Core)
2. **Copy ONLY these folders first**:
   - `Assets/Scripts/EyeTracker` (just this one folder)
3. **Wait** for Unity to compile
4. **Check** if Unity stays open
5. **If stable**, add more folders one at a time

### Step 2: Check for Editor Script Issues

Editor scripts can cause crashes. Try this:

1. **Temporarily rename** the Editor folder:
   ```bash
   cd /Users/jdietz/Documents/GitHub/voxon-baby/Assets/Scripts
   mv Editor Editor_backup
   ```
2. **Copy scripts** (without Editor folder)
3. **Test** if Unity opens
4. **If it works**, add Editor folder back later

### Step 3: Check Unity Installation

1. **Unity Hub** → Installs
2. **Try different Unity version**:
   - Install Unity 2021.3 LTS (more stable)
   - Or Unity 2023.1 LTS
3. **Create project** with different version
4. **Test** if it opens

### Step 4: Check Mac System

1. **Check Console.app** (Mac):
   - Open Console.app
   - Look for Unity crash logs
   - Check for error messages

2. **Check available disk space**:
   ```bash
   df -h
   ```
   Need at least 10GB free

3. **Check memory**:
   - Activity Monitor → Check if Unity is using too much RAM

### Step 5: Minimal Test Setup

Create a SUPER minimal test:

1. **New Unity project**
2. **Create ONE test script**:
   - Assets → Create → C# Script
   - Name: `TestScript.cs`
   - Double-click to open
   - Add simple code:
   ```csharp
   using UnityEngine;
   
   public class TestScript : MonoBehaviour
   {
       void Start()
       {
           Debug.Log("Test works!");
       }
   }
   ```
3. **Save** and wait for compilation
4. **If this crashes**, Unity installation is broken

### Step 6: Reinstall Unity (Last Resort)

If nothing works:

1. **Uninstall Unity Hub** completely
2. **Delete Unity folders**:
   ```bash
   rm -rf ~/Library/Unity
   rm -rf ~/Library/Application\ Support/Unity
   ```
3. **Reinstall Unity Hub** from unity.com
4. **Install Unity 2022.3.62f3** fresh
5. **Create new project** and test

## Quick Test: Import Scripts Gradually

**Try this order:**

1. ✅ **First**: `EyeTracker` folder only
2. ✅ **Then**: `GazeDetection` folder
3. ✅ **Then**: `VolumetricShapes` folder
4. ✅ **Then**: `Highlighting` folder
5. ✅ **Then**: Rest of folders
6. ✅ **Last**: `Editor` folder (can cause issues)

## Check Crash Logs

**Mac crash logs location:**
```
~/Library/Logs/Unity/Editor.log
~/Library/Logs/DiagnosticReports/Unity_*.crash
```

**Check these for error messages!**

## Alternative: Use Unity Cloud Build

If local Unity keeps crashing:
1. Use Unity Cloud Build (web-based)
2. Or use Unity on a different machine
3. Or wait and try again later (sometimes Mac needs restart)

Let me know what you find in the crash logs!

