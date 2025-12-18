# How to Fix Cube Visibility

## Problem
The cube is created but not visible because it's missing components.

## Quick Fix Steps

### Step 1: Check What Components Are There
1. Select "VoxelCube" in Hierarchy
2. Look at Inspector - you should see:
   - ✅ Transform (you have this)
   - ❓ VoxelCube component (might be missing)
   - ❓ Mesh Filter (probably missing)
   - ❓ Mesh Renderer (probably missing)
   - ❓ Box Collider (probably missing)

### Step 2: Add Missing Components Manually

**Add VoxelCube Component:**
1. Click "Add Component" button
2. Type "VoxelCube" in search
3. Click on it to add

**Add Mesh Filter:**
1. Click "Add Component"
2. Type "Mesh Filter"
3. Add it

**Add Mesh Renderer:**
1. Click "Add Component"
2. Type "Mesh Renderer"
3. Add it

**Add Box Collider:**
1. Click "Add Component"
2. Type "Box Collider"
3. Add it

### Step 3: Set Up the Mesh

**Option A: Use Unity's Built-in Cube Mesh**
1. Select VoxelCube
2. In Mesh Filter component, click the circle icon next to "Mesh"
3. Search for "Cube" in the picker
4. Select "Cube" (the built-in Unity cube mesh)

**Option B: Create Mesh in Code (should happen automatically)**
- The VoxelCube component should create the mesh automatically
- If it doesn't, try: Right-click VoxelCube component → Reset

### Step 4: Set Up Material

1. Select VoxelCube
2. In Mesh Renderer component, check "Materials" section
3. If empty, click the circle icon
4. Search for "Default-Material" or create a new one
5. Or: Click "+" to add a material slot, then create a new material

**Quick Material Fix:**
1. In Project window, right-click → Create → Material
2. Name it "CubeMaterial"
3. Set its color to white or any color you want
4. Drag it onto the VoxelCube in Hierarchy

### Step 5: Position the Cube

The cube is at Z: -7 (behind camera). Move it:
1. In Transform, set Position to: X: 0, Y: 0, Z: 3
2. Or drag it in Scene view

### Step 6: Verify It's Visible

1. Look at Scene view
2. Press F while cube is selected (frames the selection)
3. You should see a white cube

## Alternative: Delete and Recreate

If manual setup is too complicated:

1. **Delete the cube:**
   - Select VoxelCube in Hierarchy
   - Press Delete key

2. **Recreate it:**
   - Voxon → Create → Voxel Cube
   - It should have all components automatically

3. **If it still doesn't work:**
   - Check Console for errors
   - The VoxelCube component should auto-create mesh in Awake()

## What Should Be Visible in Inspector

When working correctly, you should see:
- ✅ Transform
- ✅ VoxelCube (script component)
- ✅ Mesh Filter (with a mesh assigned)
- ✅ Mesh Renderer (with a material)
- ✅ Box Collider

## Still Not Working?

1. Check Console for errors
2. Try creating a regular Unity cube: GameObject → 3D Object → Cube
3. Compare what components it has vs your VoxelCube
4. Make sure VoxelCube component is actually there

