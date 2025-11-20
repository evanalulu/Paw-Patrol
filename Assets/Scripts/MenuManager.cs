using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string levelSelectSceneName = "LevelSelect";

    [Header("Audio Mixer Reference")]
    public AudioMixer audioMixer;

    [Header("Background Textures (optional override)")]
    public Texture2D normalBackground;
    public Texture2D blurredBackground;

    // UI Elements
    private Button playButton;
    private Button optionsButton;
    private Button exitOptionsButton;

    private VisualElement background;
    private VisualElement blurOverlay;
    private VisualElement optionsPanel;
    private Slider masterSlider;
    private Slider musicSlider;
    private Slider sfxSlider;

    // PlayerPrefs keys
    private const string MASTER_KEY = "Master";
    private const string MUSIC_KEY = "Music";
    private const string SFX_KEY = "SFX";

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find UI
        background = root.Q<VisualElement>("Background");
        playButton = root.Q<Button>("PlayBtn");
        optionsButton = root.Q<Button>("OptionsBtn");
        exitOptionsButton = root.Q<Button>("Exit");
        optionsPanel = root.Q<VisualElement>("OptionsPanel");
        blurOverlay = root.Q<VisualElement>("BlurOverlay");

        masterSlider = root.Q<Slider>("MasterSlider");
        musicSlider = root.Q<Slider>("MusicSlider");
        sfxSlider = root.Q<Slider>("SFXSlider");

        // Assign events
        playButton.clicked += () => SceneManager.LoadScene(levelSelectSceneName);
        optionsButton.clicked += ShowOptions;
        exitOptionsButton.clicked += HideOptions;

        optionsPanel.style.display = DisplayStyle.None;

        LoadSlider(masterSlider, MASTER_KEY, "Master");
        LoadSlider(musicSlider, MUSIC_KEY, "Music");
        LoadSlider(sfxSlider, SFX_KEY, "SFX");

        // Slider listeners
        masterSlider.RegisterValueChangedCallback(evt => SaveVolume(evt, MASTER_KEY, "Master"));
        musicSlider.RegisterValueChangedCallback(evt => SaveVolume(evt, MUSIC_KEY, "Music"));
        sfxSlider.RegisterValueChangedCallback(evt => SaveVolume(evt, SFX_KEY, "SFX"));
        
        // Add sound effects to all buttons
        UIButtonSoundHandler.AddSoundEffectsToButtons(playButton, optionsButton, exitOptionsButton);

        // Background
        if (normalBackground == null)
            normalBackground = Resources.Load<Texture2D>("UI/StartScreen/background0");

        if (blurredBackground == null)
            blurredBackground = Resources.Load<Texture2D>("UI/StartScreen/UI Assets/optionBlur");

    }

    void ShowOptions()
    {
        optionsPanel.style.display = DisplayStyle.Flex;
        background.style.backgroundImage = new StyleBackground(blurredBackground);
    }

    void HideOptions()
    {
        optionsPanel.style.display = DisplayStyle.None;
        background.style.backgroundImage = new StyleBackground(normalBackground);
    }

    void SaveVolume(ChangeEvent<float> evt, string key, string exposedParam)
    {
        float volume = Mathf.Lerp(-40f, 0f, evt.newValue / 100f);
        audioMixer.SetFloat(exposedParam, volume);
        PlayerPrefs.SetFloat(key, evt.newValue);
    }

    void LoadSlider(Slider slider, string key, string exposedParam)
    {
        float savedValue = PlayerPrefs.GetFloat(key, 100f);
        slider.SetValueWithoutNotify(savedValue);

        float mappedVolume = Mathf.Lerp(-40f, 0f, savedValue / 100f);
        audioMixer.SetFloat(exposedParam, mappedVolume);
    }
}