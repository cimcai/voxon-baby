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

            // Create a simple cube mesh
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Mesh cubeMesh = cube.GetComponent<MeshFilter>().sharedMesh;
            meshFilter.mesh = cubeMesh;
            
            // Copy material before destroying
            Material cubeMaterial = cube.GetComponent<MeshRenderer>().sharedMaterial;
            if (cubeMaterial != null)
            {
                meshRenderer.material = new Material(cubeMaterial);
            }
            else
            {
                // Create a default material if none exists
                meshRenderer.material = new Material(Shader.Find("Standard"));
                meshRenderer.material.color = baseColor;
            }
            
            DestroyImmediate(cube);

            // Apply size
            transform.localScale = dimensions * size;
            
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

