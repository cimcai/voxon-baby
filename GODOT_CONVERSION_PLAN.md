# Godot Conversion Plan

## Overview

Converting the Voxon project from Unity to Godot. Godot is more stable, lighter, and should work better on your Mac.

## What Needs Conversion

### 1. Scripts (C# → GDScript or C#)

**Option A: Keep C# (Easier)**
- Godot supports C# with .NET
- Minimal syntax changes needed
- Mostly namespace/using statement updates

**Option B: Convert to GDScript (More Godot-native)**
- Godot's native scripting language
- Python-like syntax
- Better integration with Godot

**Recommendation**: Start with C# support, can convert to GDScript later if needed

### 2. Scene Structure

**Unity**: GameObjects + Components
**Godot**: Nodes (tree structure)

**Conversion**:
- GameObject → Node
- Component → Node (as child or script)
- Scene → Scene (same concept)

### 3. Systems

**Eye Tracker System**:
- ✅ Same logic, different API calls
- WebCamTexture → CameraFeed node

**Face Detection**:
- ✅ Same concepts
- WebCamTexture → CameraFeed
- Expression detection → Same logic

**Volumetric Shapes**:
- ✅ Mesh creation → Same concepts
- MeshRenderer → MeshInstance node

**Highlighting**:
- ✅ Material changes → Same concepts
- Shader access → Similar

**Cat Face**:
- ✅ Blend shapes → Same concept
- Animator → AnimationPlayer node

## Conversion Steps

### Phase 1: Setup (30 min)
1. Download Godot
2. Create new project
3. Set up folder structure
4. Import basic assets

### Phase 2: Core Systems (2-3 hours)
1. Convert Eye Tracker system
2. Convert Gaze Detection
3. Convert Volumetric Shapes
4. Convert Highlighting

### Phase 3: Advanced Systems (2-3 hours)
1. Convert Face Detection
2. Convert LLM Integration
3. Convert Cat Face system

### Phase 4: Testing (1 hour)
1. Test all systems
2. Fix any issues
3. Optimize performance

**Total time**: ~6-8 hours

## Benefits of Godot

- ✅ **No crashes** - much more stable
- ✅ **Fast startup** - opens in seconds
- ✅ **Small download** - ~100MB vs Unity's GBs
- ✅ **Better Mac support** - works great on Mac
- ✅ **Open source** - free forever
- ✅ **Active community** - lots of help available

## Ready to Convert?

If you want, I can:
1. Create Godot project structure
2. Convert scripts to Godot C# or GDScript
3. Set up the scene structure
4. Get basic systems working

Just say the word! 🚀

