using UnityEngine;

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
            mesh.name = "Pyramid";

            // Create pyramid vertices
            Vector3[] vertices = new Vector3[]
            {
                // Base vertices
                new Vector3(-baseSize, 0, -baseSize) * size,
                new Vector3(baseSize, 0, -baseSize) * size,
                new Vector3(baseSize, 0, baseSize) * size,
                new Vector3(-baseSize, 0, baseSize) * size,
                // Apex
                new Vector3(0, height * size, 0)
            };

            // Create triangles
            int[] triangles = new int[]
            {
                // Base
                0, 2, 1,
                0, 3, 2,
                // Sides
                0, 1, 4,
                1, 2, 4,
                2, 3, 4,
                3, 0, 4
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            meshFilter.mesh = mesh;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (Application.isPlaying && GetComponent<MeshFilter>() != null)
            {
                CreatePyramidMesh();
            }
        }
    }
}

