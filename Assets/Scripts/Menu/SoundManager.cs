using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
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

    [Header("Sound Settings")]
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 0.3f;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.1f;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Canvas>() != null)
            {
                Debug.LogWarning($"[SoundManager] Detaching unexpected Canvas child '{child.name}'");
                child.SetParent(null);
            }
        }

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", sfxVolume);

        ApplyVolumes();
    }

    private void OnTransformChildrenChanged()
    {
        if (transform.childCount > 0)
        {
            Debug.LogError($"SoundManager gained unexpected children:");
            foreach (Transform child in transform)
            {
                Debug.LogError($"- {child.name} (from scene: {child.gameObject.scene.name})");
                if (child.GetComponent<Canvas>() != null)
                {
                    Debug.LogWarning($"[SoundManager] Emergency detaching Canvas child '{child.name}'");
                    child.SetParent(null);
                }
            }
        }
    }

    private void Start()
    {
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

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

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
        Debug.Log($"SoundManager being destroyed. Children count: {transform.childCount}");
        foreach (Transform child in transform)
        {
            Debug.Log($"- Destroying child: {child.name}");
        }
    }

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

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    private void ApplyVolumes()
    {
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }
}
