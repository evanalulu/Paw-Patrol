using UnityEngine;

public class CarLooper : MonoBehaviour
{
    [Header("Loop Settings")]
    [SerializeField] private float speed = 2f;      // Car movement speed
    [SerializeField] private float despawnZ = -10f; // When to disappear
    [SerializeField] private float respawnZ = 12f;  // Where to reappear
    
    [Header("Collision Prevention")]
    [SerializeField] private float minCarSpacing = 3f; // Minimum distance between cars
    [SerializeField] private float maxRespawnZ = 15f;  // Maximum Z position for respawn
    
    [HideInInspector] public int currentLaneIndex;

    private float[] lanes;

    void Start()
    {
        LevelConfig config = Resources.Load<LevelConfig>("Levels/" + PlayerPrefs.GetString("SelectedLevel", "NeighborhoodLevel"));
        lanes = config != null ? config.lanePositions : new float[] { -0.9f, 0f, 0.9f };
        LockToLane();
    }

    void Update()
    {
        // Move car downward (along Z)
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
        LockToLane();

        if (transform.position.z < despawnZ)
        {
            RespawnSameLane();
        }
    }

    private void LockToLane()
    {
        Vector3 pos = transform.position;
        pos.x = lanes[Mathf.Clamp(currentLaneIndex, 0, lanes.Length - 1)];
        transform.position = pos;
    }

    private void RespawnSameLane()
    {
        Debug.Log($"♻️ Respawning {gameObject.name} in lane {currentLaneIndex}");

        // Find a safe position to respawn
        float safeZ = FindSafeRespawnPosition();
        
        transform.position = new Vector3(
            lanes[currentLaneIndex],
            0.16f,
            safeZ
        );
    }
    
    // Find a safe position to respawn without overlapping other cars
    private float FindSafeRespawnPosition()
    {
        // Try different positions within the respawn range
        const int maxAttempts = 10;
        float laneX = lanes[currentLaneIndex];
        
        for (int i = 0; i < maxAttempts; i++)
        {
            // Try a position between respawnZ and maxRespawnZ
            float testZ = Random.Range(respawnZ, maxRespawnZ);
            
            // Check if this position is safe (not too close to other cars)
            if (!IsPositionOccupied(testZ))
            {
                return testZ;
            }
        }
        
        // If we couldn't find a safe position, use a position far beyond the normal range
        // This is a fallback to ensure cars don't completely disappear
        return maxRespawnZ + 5f;
    }
    
    // Check if a position is too close to any existing car in the same lane
    private bool IsPositionOccupied(float zPosition)
    {
        // Find all cars in the scene
        CarLooper[] cars = FindObjectsOfType<CarLooper>();
        
        foreach (CarLooper car in cars)
        {
            // Skip checking against self
            if (car == this) continue;
            
            // Only check cars in the same lane
            if (car.currentLaneIndex == currentLaneIndex)
            {
                float distance = Mathf.Abs(car.transform.position.z - zPosition);
                
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