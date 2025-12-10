using UnityEngine;
using Voxon.VolumetricShapes;

namespace Voxon.Utilities
{
    /// <summary>
    /// Utility for spawning volumetric shapes at runtime
    /// </summary>
    public class ShapeSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private KeyCode spawnCubeKey = KeyCode.Alpha1;
        [SerializeField] private KeyCode spawnSphereKey = KeyCode.Alpha2;
        [SerializeField] private KeyCode spawnPyramidKey = KeyCode.Alpha3;
        [SerializeField] private KeyCode clearShapesKey = KeyCode.C;
        [SerializeField] private Vector3 spawnPosition = new Vector3(0, 1, 5);
        [SerializeField] private float spawnRadius = 2f;

        private GameObject shapesParent;

        private void Start()
        {
            shapesParent = GameObject.Find("SpawnedShapes");
            if (shapesParent == null)
            {
                shapesParent = new GameObject("SpawnedShapes");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(spawnCubeKey))
            {
                SpawnShape<VoxelCube>("Cube");
            }
            else if (Input.GetKeyDown(spawnSphereKey))
            {
                SpawnShape<VoxelSphere>("Sphere");
            }
            else if (Input.GetKeyDown(spawnPyramidKey))
            {
                SpawnShape<VoxelPyramid>("Pyramid");
            }
            else if (Input.GetKeyDown(clearShapesKey))
            {
                ClearAllShapes();
            }
        }

        private void SpawnShape<T>(string shapeName) where T : VolumetricShape
        {
            GameObject shapeObject = new GameObject($"{shapeName}_{Time.time:F2}");
            shapeObject.transform.SetParent(shapesParent.transform);
            
            // Random position within spawn radius
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = Mathf.Abs(randomOffset.y); // Keep above ground
            shapeObject.transform.position = spawnPosition + randomOffset;

            // Add shape component
            T shape = shapeObject.AddComponent<T>();

            // Random color
            Renderer renderer = shapeObject.GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                renderer.material.color = new Color(
                    Random.Range(0.3f, 1f),
                    Random.Range(0.3f, 1f),
                    Random.Range(0.3f, 1f)
                );
            }

            Debug.Log($"Spawned {shapeName} at {shapeObject.transform.position}");
        }

        private void ClearAllShapes()
        {
            if (shapesParent != null)
            {
                int childCount = shapesParent.transform.childCount;
                for (int i = childCount - 1; i >= 0; i--)
                {
                    Destroy(shapesParent.transform.GetChild(i).gameObject);
                }
                Debug.Log($"Cleared {childCount} shapes");
            }
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 12;
            style.normal.textColor = Color.yellow;

            float yPos = Screen.height - 100;
            GUI.Label(new Rect(10, yPos, 300, 20), $"{spawnCubeKey}: Spawn Cube", style);
            GUI.Label(new Rect(10, yPos + 20, 300, 20), $"{spawnSphereKey}: Spawn Sphere", style);
            GUI.Label(new Rect(10, yPos + 40, 300, 20), $"{spawnPyramidKey}: Spawn Pyramid", style);
            GUI.Label(new Rect(10, yPos + 60, 300, 20), $"{clearShapesKey}: Clear All Shapes", style);
        }
    }
}

