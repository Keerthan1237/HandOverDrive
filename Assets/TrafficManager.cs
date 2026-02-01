using UnityEngine;
using System.Collections.Generic;

public class SimpleTrafficSpawner : MonoBehaviour
{
    [Header("Traffic Car Prefab")]
    [SerializeField] private GameObject trafficCarPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 5f; 
    [SerializeField] private float spawnDistance = 150f;

    [Header("Lane Settings")]
    [SerializeField] private float laneWidth = 3f;
    [SerializeField] private int totalLanes = 4;

    [Header("Player Reference")]
    [SerializeField] private Transform playerCar;

    [Header("Auto Cleanup")]
    [SerializeField] private float deleteDistance = 100f;

    private float nextSpawnTime;
    private List<GameObject> spawnedCars = new List<GameObject>();

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCar();
            nextSpawnTime = Time.time + spawnInterval;
        }

        CleanupOldCars();
    }

    void SpawnCar()
    {
        if (!trafficCarPrefab || !playerCar) return;

        int randomLane = Random.Range(0, 4);

        float xPos = GetLaneXPosition(randomLane);
        float zPos = playerCar.position.z - spawnDistance;

        Vector3 spawnPos = new Vector3(
            xPos,
            trafficCarPrefab.transform.position.y,
            zPos
        );

        GameObject car = Instantiate(trafficCarPrefab, spawnPos, Quaternion.identity);

        
        car.transform.rotation = Quaternion.LookRotation(-Vector3.forward);

        spawnedCars.Add(car);
    }

    void CleanupOldCars()
    {
        for (int i = spawnedCars.Count - 1; i >= 0; i--)
        {
            if (spawnedCars[i] == null)
            {
                spawnedCars.RemoveAt(i);
                continue;
            }

            if (spawnedCars[i].transform.position.z < playerCar.position.z - deleteDistance)
            {
                Destroy(spawnedCars[i]);
                spawnedCars.RemoveAt(i);
            }
        }
    }

    float GetLaneXPosition(int laneIndex)
    {
        float totalWidth = (totalLanes - 1) * laneWidth;
        return -totalWidth / 2f + laneIndex * laneWidth;
    }
}
