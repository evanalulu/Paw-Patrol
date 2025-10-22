using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Car Prefabs")]
    public GameObject[] carPrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;

    private readonly float[] lanes = { -0.9f, 0f, 0.9f };

    void Start()
    {
        InvokeRepeating(nameof(SpawnCar), 1f, spawnInterval);
    }

    void SpawnCar()
    {
        if (carPrefabs.Length == 0)
        {
            Debug.LogWarning("No car prefabs assigned to CarSpawner!");
            return;
        }

        // Choose a random prefab and lane
        int carIndex = Random.Range(0, carPrefabs.Length);
        float laneX = lanes[Random.Range(0, lanes.Length)];
        float randomZ = Random.Range(2f, 10f);

        Vector3 spawnPos = new Vector3(laneX, 0.16f, randomZ);
        Quaternion spawnRot = Quaternion.Euler(90f, 180f, 0f);

        Instantiate(carPrefabs[carIndex], spawnPos, spawnRot);
    }
}