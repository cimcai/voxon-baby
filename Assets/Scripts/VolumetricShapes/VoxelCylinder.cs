using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voxon.VolumetricShapes
{
    /// <summary>
    /// Cylinder volumetric shape
    /// </summary>
    public class VoxelCylinder : VolumetricShape
    {
        [Header("Cylinder Properties")]
        [SerializeField] private float radius = 0.5f;
        [SerializeField] private float height = 1f;
        [SerializeField] [Range(3, 64)] private int segments = 16;

        protected override void Awake()
        {
            base.Awake();
            CreateCylinderMesh();
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            // Ensure mesh exists when enabled in editor
            if (!Application.isPlaying)
            {
                if (GetComponent<MeshFilter>() == null || GetComponent<MeshRenderer>() == null)
                {
                    CreateCylinderMesh();
                }
            }
        }
#endif

        private void CreateCylinderMesh()
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
            mesh.name = "VoxelCylinder";

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();

            float scaledRadius = radius * size;
            float scaledHeight = height * size;

            // Bottom circle vertices
            for (int i = 0; i < segments; i++)
            {
                float angle = (i / (float)segments) * Mathf.PI * 2f;
                float x = Mathf.Cos(angle) * scaledRadius;
                float z = Mathf.Sin(angle) * scaledRadius;
                vertices.Add(new Vector3(x, 0, z));
                normals.Add(Vector3.down);
                uvs.Add(new Vector2((float)i / segments, 0));
            }
            int bottomCenterIndex = vertices.Count;
            vertices.Add(new Vector3(0, 0, 0)); // Bottom center
            normals.Add(Vector3.down);
            uvs.Add(new Vector2(0.5f, 0.5f));

            // Top circle vertices
            int topStartIndex = vertices.Count;
            for (int i = 0; i < segments; i++)
            {
                float angle = (i / (float)segments) * Mathf.PI * 2f;
                float x = Mathf.Cos(angle) * scaledRadius;
                float z = Mathf.Sin(angle) * scaledRadius;
                vertices.Add(new Vector3(x, scaledHeight, z));
                normals.Add(Vector3.up);
                uvs.Add(new Vector2((float)i / segments, 1));
            }
            int topCenterIndex = vertices.Count;
            vertices.Add(new Vector3(0, scaledHeight, 0)); // Top center
            normals.Add(Vector3.up);
            uvs.Add(new Vector2(0.5f, 0.5f));

            // Bottom face triangles
            for (int i = 0; i < segments; i++)
            {
                int next = (i + 1) % segments;
                triangles.Add(bottomCenterIndex);
                triangles.Add(next);
                triangles.Add(i);
            }

            // Top face triangles
            for (int i = 0; i < segments; i++)
            {
                int current = topStartIndex + i;
                int next = topStartIndex + ((i + 1) % segments);
                triangles.Add(topCenterIndex);
                triangles.Add(current);
                triangles.Add(next);
            }

            // Side faces - need to duplicate vertices for proper normals and UVs
            int sideStartIndex = vertices.Count;
            for (int i = 0; i <= segments; i++)
            {
                int segIndex = i % segments;
                float angle = (segIndex / (float)segments) * Mathf.PI * 2f;
                float x = Mathf.Cos(angle) * scaledRadius;
                float z = Mathf.Sin(angle) * scaledRadius;
                
                // Bottom vertex
                vertices.Add(new Vector3(x, 0, z));
                normals.Add(new Vector3(x / scaledRadius, 0, z / scaledRadius));
                uvs.Add(new Vector2((float)i / segments, 0));
                
                // Top vertex
                vertices.Add(new Vector3(x, scaledHeight, z));
                normals.Add(new Vector3(x / scaledRadius, 0, z / scaledRadius));
                uvs.Add(new Vector2((float)i / segments, 1));
            }

            // Side face triangles
            for (int i = 0; i < segments; i++)
            {
                int baseIndex = sideStartIndex + i * 2;
                
                // First triangle
                triangles.Add(baseIndex);
                triangles.Add(baseIndex + 1);
                triangles.Add(baseIndex + 2);
                
                // Second triangle
                triangles.Add(baseIndex + 2);
                triangles.Add(baseIndex + 1);
                triangles.Add(baseIndex + 3);
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.normals = normals.ToArray();
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
                CreateCylinderMesh();
            }
#if UNITY_EDITOR
            else if (!Application.isPlaying)
            {
                CreateCylinderMesh();
            }
#endif
        }
    }
}

