using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Car Prefabs")]
    public GameObject[] carPrefabs;
    public GameObject[] rescuePrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public float rescueChance = 0.2f;

    private readonly float[] lanes = { -0.9f, 0f, 0.9f };

    void Start()
    {
        InvokeRepeating(nameof(SpawnCar), 1f, spawnInterval);
    }

    void SpawnCar()
  {
      // 20% chance to spawn a rescue vehicle instead of a normal car
      bool spawnRescue = (rescuePrefabs != null && rescuePrefabs.Length > 0 && Random.value < 0.2f);

      GameObject[] pool = spawnRescue ? rescuePrefabs : carPrefabs;

      // Choose a random prefab and lane
      int index = Random.Range(0, pool.Length);
      float laneX = lanes[Random.Range(0, lanes.Length)];
      float randomZ = Random.Range(2f, 10f);

      Vector3 spawnPos = new Vector3(laneX, 0.16f, randomZ);
      Quaternion spawnRot = Quaternion.Euler(90f, 180f, 0f);

      // Spawn vehicle
      Instantiate(pool[index], spawnPos, spawnRot);
  }
}