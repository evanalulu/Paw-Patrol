using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public float spawnInterval = 2f;
    public float carSpeed = 2f;

    private readonly float[] lanes = { -0.9f, 0f, 0.9f };
    private float despawnZ = -10f;
    private float respawnZ = 12f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCar), 1f, spawnInterval);
    }

    void SpawnCar()
    {
        // Spawn car of
        int carIndex = Random.Range(0, carPrefabs.Length); // rand car type
        float laneX = lanes[Random.Range(0, lanes.Length)]; // at rand x
        float randomZ = Random.Range(2f, 10f); // at rand z

        Vector3 spawnPos = new Vector3(laneX, 0.16f, randomZ);

        GameObject car = Instantiate(
            carPrefabs[carIndex],
            spawnPos,
            Quaternion.Euler(90f, 180f, 0f)
        );

        var looper = car.AddComponent<CarLooper>();
        looper.Init(carSpeed, despawnZ, respawnZ);
    }
}