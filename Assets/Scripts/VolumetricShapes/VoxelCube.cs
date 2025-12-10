using UnityEngine;

namespace Voxon.VolumetricShapes
{
    /// <summary>
    /// Cube volumetric shape
    /// </summary>
    public class VoxelCube : VolumetricShape
    {
        [Header("Cube Properties")]
        [SerializeField] private Vector3 dimensions = Vector3.one;

        protected override void Awake()
        {
            base.Awake();
            CreateCubeMesh();
        }

        private void CreateCubeMesh()
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

            // Create a simple cube mesh
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Mesh cubeMesh = cube.GetComponent<MeshFilter>().sharedMesh;
            meshFilter.mesh = cubeMesh;
            DestroyImmediate(cube);

            // Apply size
            transform.localScale = dimensions * size;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (Application.isPlaying && GetComponent<MeshFilter>() != null)
            {
                transform.localScale = dimensions * size;
            }
        }
    }
}

