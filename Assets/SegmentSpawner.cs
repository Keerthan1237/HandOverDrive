using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    public GameObject segmentPrefab;   // your prefab
    public Transform plane;             // your plane

    void Start()
    {
        SpawnSegments();
    }

    void SpawnSegments()
    {
        // Get plane length (Z direction)
        float planeLength = plane.localScale.z * 10000000f; // Unity plane is 10 units by default

        // Get prefab length
        float segmentLength = segmentPrefab.GetComponent<Renderer>().bounds.size.z;

        int numberOfSegments = Mathf.CeilToInt(planeLength / segmentLength);

        Vector3 startPos = plane.position;

        for (int i = 0; i < numberOfSegments; i++)
        {
            Vector3 spawnPos = startPos + new Vector3(0, 0, i * segmentLength);

            Instantiate(segmentPrefab, spawnPos, Quaternion.identity, transform);
        }
    }
}

