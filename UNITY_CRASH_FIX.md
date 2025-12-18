# Fix Unity Crash - Step by Step

## The Problem
Unity crashes when trying to open the project, likely because:
1. Missing essential ProjectSettings files
2. Too many scripts compiling at once
3. Project structure issues

## Solution: Create Fresh Unity Project (EASIEST)

### Step 1: Create New Unity Project
1. **Open Unity Hub**
2. **Click "New Project"**
3. **Template**: Select **"3D (Core)"**
4. **Project Name**: `voxon-baby-new`
5. **Location**: Choose a location
6. **Click "Create"**
7. **Wait** for Unity to open (2-3 minutes)

### Step 2: Copy Scripts
1. **In Finder**, navigate to:
   `/Users/jdietz/Documents/GitHub/voxon-baby/Assets/Scripts`
2. **Copy** the entire `Scripts` folder
3. **In Unity**, in the Project window (bottom), find `Assets` folder
4. **Right-click** on `Assets` → **Show in Finder**
5. **Paste** the `Scripts` folder there
6. **Unity will automatically compile** (wait for it to finish)

### Step 3: Test
1. **Create a scene**: File → New Scene → Basic (Built-in)
2. **Save it**: File → Save Scene As → "MainScene"
3. **Try the menu**: Voxon → Create → Eye Tracker Manager
4. **If menu appears**, scripts are working!

## Alternative: Fix Current Project

If you want to keep the current folder:

### Step 1: Delete Problematic Files
```bash
# In Terminal:
cd /Users/jdietz/Documents/GitHub/voxon-baby
rm -rf Library
rm -rf Temp
rm -rf obj
```

### Step 2: Try Opening Again
1. **Unity Hub** → Select project → **Open**
2. **Wait** for Unity to regenerate Library folder
3. **Be patient** - first open takes 5-10 minutes

### Step 3: If Still Crashes
- Check **Console.app** (Mac) for crash logs
- Look for error messages
- Try opening with **different Unity version**

## Recommended: Fresh Project Method

**I recommend creating a fresh Unity project** because:
- ✅ Clean start
- ✅ No corrupted files
- ✅ Faster compilation
- ✅ More reliable

The scripts will work exactly the same in a new project!

## After Project Opens Successfully

1. **Create scene**: File → New Scene
2. **Create face detection**: Voxon → Create → Face Detection → WebCam Provider
3. **Press Play** ▶️

Let me know which method you want to try!

