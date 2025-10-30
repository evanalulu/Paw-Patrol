using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource Music;
    public AudioSource SoundFX;

    [Header("Audio Clips")]
    public AudioClip BackgroundMusic;
    public AudioClip AttackSound;
    public AudioClip CollisionSound;
    public AudioClip PetCollectSound;
    public AudioClip WoofSound;
    public AudioClip MeowSound;
		public AudioClip GameOverSound;

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
        // background music
        if (Music != null && BackgroundMusic != null)
        {
            Music.clip = BackgroundMusic;
            Music.loop = true;
            Music.Play();
        }
    }

    public void Play(AudioClip clip)
    {
        if (clip != null && SoundFX != null)
            SoundFX.PlayOneShot(clip);
    }

    // Randomly play Woof or Meow.
    public void PlayRandomPetSound()
    {
        AudioClip clipToPlay = Random.value > 0.5f ? WoofSound : MeowSound;
        Play(clipToPlay);
    }
}