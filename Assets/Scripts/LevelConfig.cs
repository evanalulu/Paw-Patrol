using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Lane Setup")]
    public float[] lanePositions;
    public float carSpawnMinZ = 12f;
    public float carSpawnMaxZ = 15f;
    public float spawnInterval = 2f; 
    public float rescueChance = 0.5f;

    [Header("Background")]
    public Material backgroundMaterial;

    [Header("Special Vehicles (Spawn Only in This Level)")]
    public GameObject[] exclusivePrefabs;

    [Range(0f, 1f)]
    public float megaSpawnChance = 0.15f;
}
