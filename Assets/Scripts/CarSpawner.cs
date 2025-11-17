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
    
    private GameObject[] fullPrefabSet;
    private LevelConfig config;
    private float[] lanes;
    private float[] nextSpawnTime;

    void Start()
    {
        string selected = PlayerPrefs.GetString("SelectedLevel", "NeighborhoodLevel");

        config = Resources.Load<LevelConfig>("Levels/" + PlayerPrefs.GetString("SelectedLevel", "NeighborhoodLevel"));
        if (config == null)
        {
            Debug.LogError("❌ LevelConfig missing. Using backup lane layout.");
            lanes = new float[] { -0.9f, 0f, 0.9f };
            fullPrefabSet = carPrefabs; 
        }
        else
        {
            lanes = config.lanePositions;
            fullPrefabSet = CombineVehicleLists(carPrefabs, config.exclusivePrefabs);
            spawnInterval = config.spawnInterval;
            rescueChance = config.rescueChance;
            carSpawnZMin = config.carSpawnMinZ;
            carSpawnZMax = config.carSpawnMaxZ;
        }

        nextSpawnTime = new float[lanes.Length];
        InvokeRepeating(nameof(SpawnCar), 1f, spawnInterval);
    }

    GameObject[] CombineVehicleLists(GameObject[] baseCars, GameObject[] extra)
    {
        if (extra == null || extra.Length == 0)
            return baseCars;

        GameObject[] result = new GameObject[baseCars.Length + extra.Length];
        baseCars.CopyTo(result, 0);
        extra.CopyTo(result, baseCars.Length);

        return result;
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
        nextSpawnTime[availableLane] = currentTime + config.spawnInterval;

        // 50% chance rescue vehicle
        bool spawnRescue = (rescuePrefabs != null && rescuePrefabs.Length > 0 && Random.value < rescueChance);
        GameObject[] pool = spawnRescue ? rescuePrefabs : carPrefabs;

        if (config.exclusivePrefabs != null && config.exclusivePrefabs.Length > 0)
        {
            System.Array.Resize(ref pool, pool.Length + config.exclusivePrefabs.Length);
            config.exclusivePrefabs.CopyTo(pool, pool.Length - config.exclusivePrefabs.Length);
        }

        // choose prefab
        int index = Random.Range(0, pool.Length);
        float laneX = lanes[availableLane];
        float spawnZ = Random.Range(carSpawnZMin, carSpawnZMax);

        Vector3 spawnPos = new Vector3(laneX, 0.16f, spawnZ);
        Quaternion spawnRot = Quaternion.Euler(90f, 180f, 0f);

        Instantiate(pool[index], spawnPos, spawnRot);
    }
}