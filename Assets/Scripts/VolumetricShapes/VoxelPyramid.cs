using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voxon.VolumetricShapes
{
    /// <summary>
    /// Pyramid volumetric shape
    /// </summary>
    public class VoxelPyramid : VolumetricShape
    {
        [Header("Pyramid Properties")]
        [SerializeField] private float baseSize = 1f;
        [SerializeField] private float height = 1f;

        protected override void Awake()
        {
            base.Awake();
            CreatePyramidMesh();
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            // Ensure mesh exists when enabled in editor
            if (!Application.isPlaying)
            {
                if (GetComponent<MeshFilter>() == null || GetComponent<MeshRenderer>() == null)
                {
                    CreatePyramidMesh();
                }
            }
        }
#endif

        private void CreatePyramidMesh()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }

            Mesh mesh = new Mesh();
            mesh.name = "VoxelPyramid";

            float scaledBase = baseSize * size * 0.5f;
            float scaledHeight = height * size;

            // Create pyramid vertices (5 vertices total)
            Vector3[] vertices = new Vector3[]
            {
                // Base vertices (counter-clockwise when viewed from above)
                new Vector3(-scaledBase, 0, -scaledBase), // 0
                new Vector3(scaledBase, 0, -scaledBase),  // 1
                new Vector3(scaledBase, 0, scaledBase),    // 2
                new Vector3(-scaledBase, 0, scaledBase),   // 3
                // Apex
                new Vector3(0, scaledHeight, 0)            // 4
            };

            // Create triangles with proper winding order
            int[] triangles = new int[]
            {
                // Base (two triangles, facing down)
                0, 3, 2,
                0, 2, 1,
                // Side 1 (front)
                0, 1, 4,
                // Side 2 (right)
                1, 2, 4,
                // Side 3 (back)
                2, 3, 4,
                // Side 4 (left)
                3, 0, 4
            };

            // Proper UV mapping
            Vector2[] uvs = new Vector2[]
            {
                // Base vertices
                new Vector2(0, 0), // 0
                new Vector2(1, 0), // 1
                new Vector2(1, 1), // 2
                new Vector2(0, 1), // 3
                // Apex (centered UV)
                new Vector2(0.5f, 0.5f) // 4
            };

            // Calculate normals manually for better quality
            Vector3[] normals = new Vector3[]
            {
                // Base normals (all pointing down)
                Vector3.down, // 0
                Vector3.down, // 1
                Vector3.down, // 2
                Vector3.down, // 3
                // Apex normal (average of side normals)
                Vector3.up // 4 (will be recalculated properly)
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.normals = normals;
            mesh.RecalculateNormals(); // Recalculate for proper smooth normals
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();

            meshFilter.mesh = mesh;

            // Setup material
            if (meshRenderer.material == null)
            {
                meshRenderer.material = new Material(Shader.Find("Standard"));
            }
            meshRenderer.material.color = baseColor;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (Application.isPlaying && GetComponent<MeshFilter>() != null)
            {
                CreatePyramidMesh();
            }
#if UNITY_EDITOR
            else if (!Application.isPlaying)
            {
                CreatePyramidMesh();
            }
#endif
        }
    }
}

