using UnityEngine;

namespace Voxon.VolumetricShapes
{
    /// <summary>
    /// Sphere volumetric shape
    /// </summary>
    public class VoxelSphere : VolumetricShape
    {
        [Header("Sphere Properties")]
        [SerializeField] private int segments = 32;

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

            // Create a simple sphere mesh
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Mesh sphereMesh = sphere.GetComponent<MeshFilter>().sharedMesh;
            meshFilter.mesh = sphereMesh;
            DestroyImmediate(sphere);

            // Apply size
            transform.localScale = Vector3.one * size;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (Application.isPlaying && GetComponent<MeshFilter>() != null)
            {
                transform.localScale = Vector3.one * size;
            }
        }
    }
}

