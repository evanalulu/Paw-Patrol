using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    private LevelConfig config;
    private Renderer backgroundRenderer;
    private TextureScroller scroller;

    void Start()
    {
        // Load config based on saved level
        string selected = PlayerPrefs.GetString("SelectedLevel", "NeighborhoodLevel");
        config = Resources.Load<LevelConfig>($"Levels/{selected}");

        if (config == null)
        {
            Debug.LogError($"❌ No LevelConfig found for {selected}");
            return;
        }

        // Find background object
        backgroundRenderer = GameObject.Find("Background")?.GetComponent<Renderer>();
        scroller = GameObject.Find("Background")?.GetComponent<TextureScroller>();

        ApplyBackground();
    }

    private void ApplyBackground()
    {
        if (backgroundRenderer == null)
        {
            Debug.LogError("❌ No 'Background' object found in scene.");
            return;
        }

        if (config.backgroundMaterial == null)
        {
            Debug.LogWarning("⚠️ LevelConfig has no background material assigned.");
            return;
        }

        // Important: make unique instance so scrolling doesn't change the shared material
        Material instance = new Material(config.backgroundMaterial);

        backgroundRenderer.material = instance;

        // Also apply to scroller if exists
        if (scroller != null)
            scroller.GetComponent<Renderer>().material = instance;

        Debug.Log($"🖼 Applied BG from config: {config.name}");
    }
}