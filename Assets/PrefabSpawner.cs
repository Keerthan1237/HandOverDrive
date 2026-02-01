using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // Drag your prefab here
    public GameObject plane;         // Drag your plane here
    public int count = 10;           // Number of items to spawn
    public float offset = 0f;        // Optional spacing between items

    [ContextMenu("Spawn Objects")] // Adds a menu item to the component
    void Spawn()
    {
        if (objectToSpawn == null || plane == null) return;

        // Get the length of the plane along the Z-axis
        float planeLength = plane.GetComponent<Renderer>().bounds.size.z;
        Vector3 startPos = plane.GetComponent<Collider>().bounds.min;

        for (int i = 0; i < count; i++)
        {
            // Calculate position along the length
            Vector3 spawnPos = startPos + new Vector3(
                plane.GetComponent<Renderer>().bounds.size.x / 2, // Center on X
                0, // Y position (adjust if needed)
                (planeLength / count) * i + (planeLength / (count * 2)) // Distributed on Z
            );

            Instantiate(objectToSpawn, spawnPos, Quaternion.identity, plane.transform);
        }
    }
}
