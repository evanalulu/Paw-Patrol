using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Car Prefabs")]
    public GameObject[] carPrefabs;
    public GameObject[] rescuePrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float rescueChance = 0.9f;
    public float laneCooldown = 3f;
    public float carSpawnZMin = 12f;
    public float carSpawnZMax = 15f;

    private readonly float[] lanes = { -0.9f, 0f, 0.9f };
    private float[] nextSpawnTime;

    void Start()
    {
        nextSpawnTime = new float[lanes.Length];
        InvokeRepeating(nameof(SpawnCar), 1f, spawnInterval);
    }

    void SpawnCar()
    {
        float currentTime = Time.time;

        // find lanes that are ready
        int availableLane = -1;
        for (int i = 0; i < lanes.Length; i++)
        {
            if (currentTime >= nextSpawnTime[i])
            {
                availableLane = i;
                break;
            }
        }

        // no lanes free
        if (availableLane == -1) return;

        // record cooldown for that lane
        nextSpawnTime[availableLane] = currentTime + laneCooldown;

        // 50% chance rescue vehicle
        bool spawnRescue = (rescuePrefabs != null && rescuePrefabs.Length > 0 && Random.value < rescueChance);
        GameObject[] pool = spawnRescue ? rescuePrefabs : carPrefabs;

        // choose prefab
        int index = Random.Range(0, pool.Length);
        float laneX = lanes[availableLane];
        float spawnZ = Random.Range(carSpawnZMin, carSpawnZMax);

        Vector3 spawnPos = new Vector3(laneX, 0.16f, spawnZ);
        Quaternion spawnRot = Quaternion.Euler(90f, 180f, 0f);

        Instantiate(pool[index], spawnPos, spawnRot);
    }
}