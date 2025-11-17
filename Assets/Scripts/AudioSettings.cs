using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;

    private Slider masterSlider;
    private Slider musicSlider;
    private Slider sfxSlider;

    void OnEnable()
    {
        var ui = GetComponent<UIDocument>().rootVisualElement;

        masterSlider = ui.Q<Slider>("MasterSlider");
        musicSlider = ui.Q<Slider>("MusicSlider");
        sfxSlider = ui.Q<Slider>("SFXSlider");

        // Load saved values
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        // Assign listeners
        masterSlider.RegisterValueChangedCallback(v => SetVolume("MasterVolume", v.newValue));
        musicSlider.RegisterValueChangedCallback(v => SetVolume("MusicVolume", v.newValue));
        sfxSlider.RegisterValueChangedCallback(v => SetVolume("SFXVolume", v.newValue));
    }

    private void SetVolume(string exposedName, float value)
    {
        // Convert slider (0–1) to mixer dB scale
        float db = Mathf.Log10(Mathf.Max(value, 0.001f)) * 20f;
        mixer.SetFloat(exposedName, db);

        // Save preference
        PlayerPrefs.SetFloat(exposedName, value);
        PlayerPrefs.Save();
    }
}