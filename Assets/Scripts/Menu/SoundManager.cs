using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    // Singleton instance for global access
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Musics")]
    [SerializeField] private AudioClip titleMusic;
    [SerializeField] private AudioClip lobbyMusic;
    [SerializeField] private AudioClip cityMusic;
    [SerializeField] private AudioClip forestMusic;
    [SerializeField] private AudioClip castleMusic;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private AudioClip npcSound;
    [SerializeField] private AudioClip teleportSound;
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private AudioClip doorCloseSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip itemSound;
    [SerializeField] private AudioClip pauseSound;
    [SerializeField] private AudioClip endSound;

    [Header("Sound Settings")]
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 0.3f;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.1f;

    private void Awake()
    {
        // Detach any unexpected Canvas children (defensive)
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Canvas>() != null)
            {
                Debug.LogWarning($"[SoundManager] Detaching unexpected Canvas child '{child.name}'");
                child.SetParent(null);
            }
        }

        // Singleton pattern enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved volume settings
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", sfxVolume);

        ApplyVolumes();
    }

    private void Start()
    {
        // Initialize music for current scene
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Play appropriate music based on loaded scene
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "InGame":
                PlayMusic(lobbyMusic);
                break;
            case "MainMenu":
                PlayMusic(titleMusic);
                break;
            case "CityLevel":
                PlayMusic(cityMusic);
                break;
            case "ForestLevel":
                PlayMusic(forestMusic);
                break;
            case "CastleLevel":
                PlayMusic(castleMusic);
                break;
        }
    }

    // Play a sound effect once at specified volume
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // Play or switch music, avoid restarting same clip
    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (musicSource.clip == musicClip && musicSource.isPlaying) return;

        musicSource.clip = musicClip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    private void OnDestroy()
    {
        // Log destruction for debugging
        Debug.Log($"SoundManager being destroyed. Children count: {transform.childCount}");
        foreach (Transform child in transform)
        {
            Debug.Log($"- Destroying child: {child.name}");
        }
    }

    // Convenience methods for specific SFX
    public void PlayCheckpointSFX() => PlaySFX(checkpointSound);
    public void PlayNPCSFX() => PlaySFX(npcSound);
    public void PlayTeleportSFX() => PlaySFX(teleportSound);
    public void PlayDoorOpenSFX() => PlaySFX(doorOpenSound);
    public void PlayDoorCloseSFX() => PlaySFX(doorCloseSound);
    public void PlayDeathSFX() => PlaySFX(deathSound);
    public void PlayJumpSFX() => PlaySFX(jumpSound);
    public void PlayDashSFX() => PlaySFX(dashSound);
    public void PickUpSFX() => PlaySFX(itemSound);
    public void PauseSFX() => PlaySFX(pauseSound);
    public void ClearSFX() => PlaySFX(endSound);

    // Set music volume and save preference
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    // Set SFX volume and save preference
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    // Apply volume settings to audio sources
    private void ApplyVolumes()
    {
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }
}
