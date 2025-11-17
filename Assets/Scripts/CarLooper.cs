using UnityEngine;

public class CarLooper : MonoBehaviour
{
    [Header("Loop Settings")]
    [SerializeField] private float speed = 2f;      // Car movement speed
    [SerializeField] private float despawnZ = -10f; // When to disappear
    [SerializeField] private float respawnZ = 12f;  // Where to reappear

    private float[] lanes;

    void Start()
    {
        LevelConfig config = Resources.Load<LevelConfig>(PlayerPrefs.GetString("SelectedLevel", "NeighborhoodLevel"));
        lanes = config != null ? config.lanePositions : new float[] { -0.9f, 0f, 0.9f };
    }

    void Update()
    {
        // Move car downward (along Z)
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        if (transform.position.z < despawnZ)
        {
            float laneX = lanes[Random.Range(0, lanes.Length)];
            transform.position = new Vector3(laneX, 0.16f, respawnZ);
        }
    }
}