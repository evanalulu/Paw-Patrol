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
}
// using UnityEngine;

// public class LevelConfig : MonoBehaviour
// {
//     [Header("Lane Setup")]
//     public float[] lanePositions;
//     public float carSpawnMinZ = 12f;
//     public float carSpawnMaxZ = 15f;
//     public float spawnInterval = 2f; 
//     public float rescueChance = 0.5f;

//     [Header("Background")]
//     public Material neighborhoodMaterial;
//     public Material downtownMaterial;

//     private Renderer backgroundRenderer;
//     private TextureScroller scroller;

//     void Start()
//     {
//         backgroundRenderer = GameObject.Find("Background")?.GetComponent<Renderer>();
//         scroller = GameObject.Find("Background")?.GetComponent<TextureScroller>();

//         string selectedLevel = PlayerPrefs.GetString("SelectedLevel", "NeighborhoodLevel");
// 				Debug.Log($"{selectedLevel}");
//         ApplyLevelSettings(selectedLevel);
//     }

//     public void ApplyLevelSettings(string level)
//     {
//         Debug.Log("Applying Level Settings: " + level);

//         Material chosenMat = null;

//         if (level == "NeighborhoodLevel")
//             chosenMat = neighborhoodMaterial;
//         else if (level == "DowntownLevel")
//             chosenMat = downtownMaterial;
//         else
//             Debug.LogWarning("⚠️ Unknown level name: " + level);

//         if (chosenMat != null)
//         {
//             // Assign a NEW instance of the material so scrolling stays independent
//             Material instance = new Material(chosenMat);
//             backgroundRenderer.material = instance;

//             if (scroller != null)
//                 scroller.GetComponent<Renderer>().material = instance;
//         }
//     }
// }