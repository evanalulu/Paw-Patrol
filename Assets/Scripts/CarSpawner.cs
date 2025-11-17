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
    
    [Header("Collision Prevention")]
    public float minCarSpacing = 3f; // Minimum distance between cars in the same lane
    public LayerMask carLayerMask; // Layer mask for cars
    
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

        // Step 1: find all available lanes
        System.Collections.Generic.List<int> availableLanes = new System.Collections.Generic.List<int>();
        for (int i = 0; i < lanes.Length; i++)
        {
            if (currentTime >= nextSpawnTime[i])
            {
                availableLanes.Add(i);
            }
        }

        // If no lanes are available, return
        if (availableLanes.Count == 0) return;
        
        // Randomly select one of the available lanes
        int availableLane = availableLanes[Random.Range(0, availableLanes.Count)];

        // Step 2: vehicle pool
        bool spawnRescue = rescuePrefabs != null && rescuePrefabs.Length > 0 && Random.value < rescueChance;
        GameObject[] pool = spawnRescue ? rescuePrefabs : fullPrefabSet;

        // Step 3: choose prefab & lane
        int index = Random.Range(0, pool.Length);
        float laneX = lanes[availableLane];
        
        // Try different spawn positions until we find one without collision
        float spawnZ = FindSafeSpawnPosition(laneX, availableLane);
        
        // If no safe position found, skip this spawn
        if (spawnZ < 0)
        {
            Debug.Log("⚠️ No safe spawn position found in lane " + availableLane);
            return;
        }

        Vector3 spawnPos = new(laneX, 0.16f, spawnZ);
        Quaternion spawnRot = Quaternion.Euler(90f, 180f, 0f);

        GameObject spawnedCar = Instantiate(pool[index], spawnPos, spawnRot);

        // Step 4: tell CarLooper which lane it's in
        CarLooper looper = spawnedCar.GetComponent<CarLooper>();
        if (looper != null)
            looper.currentLaneIndex = availableLane;

        // Step 5: next cooldown
        nextSpawnTime[availableLane] = currentTime + config.spawnInterval;
    }
    
    // Find a safe position to spawn a car without overlapping existing cars
    private float FindSafeSpawnPosition(float laneX, int laneIndex)
    {
        // Try several positions within the spawn range
        const int maxAttempts = 10;
        
        for (int i = 0; i < maxAttempts; i++)
        {
            float testZ = Random.Range(carSpawnZMin, carSpawnZMax);
            Vector3 testPos = new Vector3(laneX, 0.16f, testZ);
            
            // Check if there's any car too close to this position
            if (!IsPositionOccupied(testPos, laneIndex))
            {
                return testZ;
            }
        }
        
        // No safe position found after all attempts
        return -1f;
    }
    
    // Check if a position is too close to any existing car
    private bool IsPositionOccupied(Vector3 position, int laneIndex)
    {
        // Find all cars in the scene
        CarLooper[] cars = FindObjectsOfType<CarLooper>();
        
        foreach (CarLooper car in cars)
        {
            // Only check cars in the same lane
            if (car.currentLaneIndex == laneIndex)
            {
                float distance = Mathf.Abs(car.transform.position.z - position.z);
                
                // If a car is too close, position is occupied
                if (distance < minCarSpacing)
                {
                    return true;
                }
            }
        }
        
        // No cars too close, position is safe
        return false;
    }
}