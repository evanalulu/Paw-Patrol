using UnityEngine;

public class CarLooper : MonoBehaviour
{
    [Header("Loop Settings")]
    [SerializeField] private float speed = 2f;      // Car movement speed
    [SerializeField] private float despawnZ = -10f; // When to disappear
    [SerializeField] private float respawnZ = 12f;  // Where to reappear
    
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

        transform.position = new Vector3(
            lanes[currentLaneIndex], 
            0.16f, 
            respawnZ
        );
    }
}