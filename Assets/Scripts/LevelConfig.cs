using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    public string levelName;
    public GameObject backgroundPrefab;

    [Header("Enemy Spawn Settings")]
    public float spawnInterval = 2f;
    public GameObject[] vehiclePrefabs;

    [Header("Difficulty Scaling")]
    public float minSpeed = 2f;
    public float maxSpeed = 5f;

    [Header("Rewards")]
    public int scorePerPet = 10;
}