using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource Music;
    public AudioSource SoundFX;

    [Header("Audio Clips")]
    public AudioClip BackgroundMusic;
    public AudioClip LobbyMusic;
    public AudioClip AttackSound;
    public AudioClip CollisionSound;
    public AudioClip PetCollectSound;
    public AudioClip WoofSound;
    public AudioClip MeowSound;
    public AudioClip GameOverSound;
    public AudioClip RolloverSound;
    public AudioClip ClickSound;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UpdateMusicForScene(SceneManager.GetActiveScene().name);

        SceneManager.activeSceneChanged += (_, newScene) =>
        {
            UpdateMusicForScene(newScene.name);
        };
    }

    private void UpdateMusicForScene(string sceneName)
    {
        if (sceneName.Contains("NeighborhoodStreet"))
            PlayMusic(BackgroundMusic);
        else
            PlayMusic(LobbyMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || Music == null) return;

        if (Music.clip == clip) return; // already playing → do nothing

        Music.clip = clip;
        Music.loop = true;
        Music.Play();
    }

    public void Play(AudioClip clip)
    {
        if (clip != null && SoundFX != null)
            SoundFX.PlayOneShot(clip);
    }
    
    // Play pet collect sound at a higher volume
    public void PlayPetCollectSound()
    {
        if (PetCollectSound != null && SoundFX != null)
            SoundFX.PlayOneShot(PetCollectSound, 1.5f); // Increased volume (1.0f is normal)
    }

    // Play button rollover sound
    public void PlayRolloverSound()
    {
        if (RolloverSound != null && SoundFX != null)
            SoundFX.PlayOneShot(RolloverSound, 1.5f); // Increased volume from 0.8f to 1.5f
    }

    // Play button click sound
    public void PlayClickSound()
    {
        if (ClickSound != null && SoundFX != null)
        {
            // Stop any currently playing sounds to ensure click is heard clearly
            SoundFX.Stop();
            // Play at much higher volume (2.5x)
            SoundFX.PlayOneShot(ClickSound, 2.5f);
        }
    }

    // Randomly play Woof or Meow.
    public void PlayRandomPetSound()
    {
        AudioClip clipToPlay = Random.value > 0.5f ? WoofSound : MeowSound;
        Play(clipToPlay);
    }
}