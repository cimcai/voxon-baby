using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

        private void Reset()
        {
            // Called when component is first added or reset in editor
            CreateCubeMesh();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
        }

#if UNITY_EDITOR
        private new void OnValidate()
        {
            // Ensure mesh exists in editor
            if (!Application.isPlaying)
            {
                MeshFilter mf = GetComponent<MeshFilter>();
                MeshRenderer mr = GetComponent<MeshRenderer>();
                if (mf == null || mr == null || mf.sharedMesh == null)
                {
                    CreateCubeMesh();
                }
            }
            base.OnValidate();
        }
        
        private void OnEnable()
        {
            // Ensure components exist when enabled in editor
            if (!Application.isPlaying)
            {
                if (GetComponent<MeshFilter>() == null || GetComponent<MeshRenderer>() == null)
                {
                    CreateCubeMesh();
                }
            }
        }
#endif

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

            Mesh mesh = new Mesh();
            mesh.name = "VoxelCube";

            float scaledX = dimensions.x * size * 0.5f;
            float scaledY = dimensions.y * size * 0.5f;
            float scaledZ = dimensions.z * size * 0.5f;

            // 8 vertices for a cube
            Vector3[] vertices = new Vector3[]
            {
                // Bottom face
                new Vector3(-scaledX, -scaledY, -scaledZ), // 0
                new Vector3(scaledX, -scaledY, -scaledZ),  // 1
                new Vector3(scaledX, -scaledY, scaledZ),   // 2
                new Vector3(-scaledX, -scaledY, scaledZ),   // 3
                // Top face
                new Vector3(-scaledX, scaledY, -scaledZ),   // 4
                new Vector3(scaledX, scaledY, -scaledZ),    // 5
                new Vector3(scaledX, scaledY, scaledZ),     // 6
                new Vector3(-scaledX, scaledY, scaledZ)     // 7
            };

            // 6 faces * 2 triangles * 3 vertices = 36 indices
            int[] triangles = new int[]
            {
                // Bottom face
                0, 2, 1,
                0, 3, 2,
                // Top face
                4, 5, 6,
                4, 6, 7,
                // Front face
                3, 7, 6,
                3, 6, 2,
                // Back face
                0, 1, 5,
                0, 5, 4,
                // Left face
                0, 4, 7,
                0, 7, 3,
                // Right face
                1, 2, 6,
                1, 6, 5
            };

            // Proper UV mapping for each face
            Vector2[] uvs = new Vector2[]
            {
                // Bottom face
                new Vector2(0, 0), // 0
                new Vector2(1, 0), // 1
                new Vector2(1, 1), // 2
                new Vector2(0, 1), // 3
                // Top face
                new Vector2(0, 0), // 4
                new Vector2(1, 0), // 5
                new Vector2(1, 1), // 6
                new Vector2(0, 1)  // 7
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();

            meshFilter.mesh = mesh;

            // Setup material
            if (meshRenderer.material == null)
            {
                meshRenderer.material = new Material(Shader.Find("Standard"));
            }
            meshRenderer.material.color = baseColor;
            
            // Position cube in front of camera if at origin (only in editor)
#if UNITY_EDITOR
            if (!Application.isPlaying && transform.position == Vector3.zero)
            {
                UnityEngine.Camera sceneViewCam = UnityEditor.SceneView.lastActiveSceneView?.camera;
                if (sceneViewCam != null)
                {
                    Vector3 forward = sceneViewCam.transform.forward;
                    forward.y = 0; // Keep on ground
                    forward.Normalize();
                    transform.position = forward * 3f;
                }
                else
                {
                    transform.position = new Vector3(0, 0, 3f);
                }
            }
#endif
        }

    }
}

