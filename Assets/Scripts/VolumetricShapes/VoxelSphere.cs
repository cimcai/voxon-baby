using UnityEngine;
using System.Collections.Generic;

namespace Voxon.VolumetricShapes
{
    /// <summary>
    /// Sphere volumetric shape
    /// </summary>
    public class VoxelSphere : VolumetricShape
    {
        [Header("Sphere Properties")]
        [SerializeField] [Range(8, 128)] private int segments = 32;
        [SerializeField] [Range(8, 64)] private int rings = 16;

        protected override void Awake()
        {
            base.Awake();
            CreateSphereMesh();
        }

        private void CreateSphereMesh()
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
            mesh.name = "VoxelSphere";

            float scaledRadius = size * 0.5f;
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();

            // Generate vertices
            for (int ring = 0; ring <= rings; ring++)
            {
                float phi = Mathf.PI * ring / rings; // 0 to PI
                float sinPhi = Mathf.Sin(phi);
                float cosPhi = Mathf.Cos(phi);

                for (int seg = 0; seg <= segments; seg++)
                {
                    float theta = 2f * Mathf.PI * seg / segments; // 0 to 2PI
                    float sinTheta = Mathf.Sin(theta);
                    float cosTheta = Mathf.Cos(theta);

                    Vector3 position = new Vector3(
                        cosTheta * sinPhi * scaledRadius,
                        cosPhi * scaledRadius,
                        sinTheta * sinPhi * scaledRadius
                    );

                    vertices.Add(position);
                    normals.Add(position.normalized);
                    uvs.Add(new Vector2((float)seg / segments, (float)ring / rings));
                }
            }

            // Generate triangles
            for (int ring = 0; ring < rings; ring++)
            {
                for (int seg = 0; seg < segments; seg++)
                {
                    int current = ring * (segments + 1) + seg;
                    int next = current + segments + 1;

                    // First triangle
                    triangles.Add(current);
                    triangles.Add(next);
                    triangles.Add(current + 1);

                    // Second triangle
                    triangles.Add(current + 1);
                    triangles.Add(next);
                    triangles.Add(next + 1);
                }
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
                CreateSphereMesh();
            }
#if UNITY_EDITOR
            else if (!Application.isPlaying)
            {
                CreateSphereMesh();
            }
#endif
        }
    }
}

