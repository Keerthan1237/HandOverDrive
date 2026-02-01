using System.Collections.Generic;
using UnityEngine;

public class RoadSegmentSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform car;
    public GameObject roadPrefab;

    [Header("Settings")]
    public float segmentLength = 30f;
    public int segmentsOnScreen = 7;

    private Queue<GameObject> segments = new Queue<GameObject>();
    private float spawnZ = 0f;

    void Start()
    {
        // spawn initial tiles toward -Z
        for (int i = 0; i < segmentsOnScreen; i++)
            Spawn();
    }

    void Update()
    {
        GameObject first = segments.Peek();

        // 🔥 reversed comparison for -Z movement
        if (first.transform.position.z - car.position.z >= segmentLength)
        {
            Spawn();
            DeleteFirst();
        }
    }

    void Spawn()
    {
        GameObject seg = Instantiate(
            roadPrefab,
            new Vector3(0, 0, spawnZ),
            Quaternion.identity
        );

        segments.Enqueue(seg);

        // 🔥 move backward instead of forward
        spawnZ -= segmentLength;
    }

    void DeleteFirst()
    {
        Destroy(segments.Dequeue());
    }
}