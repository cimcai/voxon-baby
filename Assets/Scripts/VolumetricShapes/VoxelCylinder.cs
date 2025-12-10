using UnityEngine;
using System.Collections.Generic;

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
        [SerializeField] private int segments = 16;

        protected override void Awake()
        {
            base.Awake();
            CreateCylinderMesh();
        }

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
            mesh.name = "Cylinder";

            // Create cylinder vertices
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            float scaledRadius = radius * size;
            float scaledHeight = height * size;

            // Bottom circle
            for (int i = 0; i < segments; i++)
            {
                float angle = (i / (float)segments) * Mathf.PI * 2f;
                vertices.Add(new Vector3(
                    Mathf.Cos(angle) * scaledRadius,
                    0,
                    Mathf.Sin(angle) * scaledRadius
                ));
            }
            vertices.Add(new Vector3(0, 0, 0)); // Bottom center

            // Top circle
            for (int i = 0; i < segments; i++)
            {
                float angle = (i / (float)segments) * Mathf.PI * 2f;
                vertices.Add(new Vector3(
                    Mathf.Cos(angle) * scaledRadius,
                    scaledHeight,
                    Mathf.Sin(angle) * scaledRadius
                ));
            }
            vertices.Add(new Vector3(0, scaledHeight, 0)); // Top center

            // Bottom face
            int bottomCenterIndex = segments;
            for (int i = 0; i < segments; i++)
            {
                int next = (i + 1) % segments;
                triangles.Add(bottomCenterIndex);
                triangles.Add(i);
                triangles.Add(next);
            }

            // Top face
            int topCenterIndex = segments * 2 + 1;
            int topStartIndex = segments + 1;
            for (int i = 0; i < segments; i++)
            {
                int current = topStartIndex + i;
                int next = topStartIndex + ((i + 1) % segments);
                triangles.Add(topCenterIndex);
                triangles.Add(next);
                triangles.Add(current);
            }

            // Side faces
            for (int i = 0; i < segments; i++)
            {
                int bottomCurrent = i;
                int bottomNext = (i + 1) % segments;
                int topCurrent = topStartIndex + i;
                int topNext = topStartIndex + ((i + 1) % segments);

                // First triangle
                triangles.Add(bottomCurrent);
                triangles.Add(topCurrent);
                triangles.Add(bottomNext);

                // Second triangle
                triangles.Add(bottomNext);
                triangles.Add(topCurrent);
                triangles.Add(topNext);
            }

            // UVs
            for (int i = 0; i < vertices.Count; i++)
            {
                uvs.Add(new Vector2(vertices[i].x / scaledRadius, vertices[i].y / scaledHeight));
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            meshFilter.mesh = mesh;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (Application.isPlaying && GetComponent<MeshFilter>() != null)
            {
                CreateCylinderMesh();
            }
        }
    }
}

