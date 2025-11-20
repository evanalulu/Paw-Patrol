using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class PauseOptions : MonoBehaviour
{
    [Header("Audio Mixer Reference")]
    public AudioMixer audioMixer;

    // UI Elements
    private VisualElement optionsPanel;
    private Button pauseButton;
    private Button exitButton;
    private Slider masterSlider;
    private Slider musicSlider;
    private Slider sfxSlider;

    private bool isOpen = false;

    // Volume keys (same as menu so values carry over)
    private const string MASTER_KEY = "Master";
    private const string MUSIC_KEY = "Music";
    private const string SFX_KEY = "SFX";

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // UI References
        pauseButton = root.Q<Button>("PauseBtn");
        exitButton = root.Q<Button>("Exit");
        optionsPanel = root.Q<VisualElement>("OptionsContainer");

        masterSlider = root.Q<Slider>("MasterSlider");
        musicSlider = root.Q<Slider>("MusicSlider");
        sfxSlider = root.Q<Slider>("SFXSlider");

        // Hide menu initially
        optionsPanel.style.display = DisplayStyle.None;

        // Load previous values
        LoadSetting(masterSlider, MASTER_KEY, "Master");
        LoadSetting(musicSlider, MUSIC_KEY, "Music");
        LoadSetting(sfxSlider, SFX_KEY, "SFX");

        // Button Listeners
        pauseButton.clicked += ToggleOptions;
        exitButton.clicked += ToggleOptions;

        // Slider Listeners
        masterSlider.RegisterValueChangedCallback(x => SaveSetting(x, MASTER_KEY, "Master"));
        musicSlider.RegisterValueChangedCallback(x => SaveSetting(x, MUSIC_KEY, "Music"));
        sfxSlider.RegisterValueChangedCallback(x => SaveSetting(x, SFX_KEY, "SFX"));
        
        // Add sound effects to buttons
        UIButtonSoundHandler.AddSoundEffectsToButtons(pauseButton, exitButton);
    }

    void ToggleOptions()
    {
        isOpen = !isOpen;

        optionsPanel.style.display = isOpen ? DisplayStyle.Flex : DisplayStyle.None;

        // Pause/Resume game
        Time.timeScale = isOpen ? 0f : 1f;
    }

    void SaveSetting(ChangeEvent<float> evt, string key, string exposed)
    {
        float mapped = Mathf.Lerp(-40f, 0f, evt.newValue / 100f);
        audioMixer.SetFloat(exposed, mapped);
        PlayerPrefs.SetFloat(key, evt.newValue);
    }

    void LoadSetting(Slider slider, string key, string exposed)
    {
        float savedValue = PlayerPrefs.GetFloat(key, 100f);
        slider.SetValueWithoutNotify(savedValue);

        float mapped = Mathf.Lerp(-40f, 0f, savedValue / 100f);
        audioMixer.SetFloat(exposed, mapped);
    }
}