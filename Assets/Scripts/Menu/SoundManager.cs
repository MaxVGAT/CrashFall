using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Audio Clips")]
    public AudioClip lobbyMusic;
    public AudioClip titleMusic;

    [Header("Sound Settings")]
    public float sfxVolume = 0.3f;
    public float musicVolume = 0.1f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.1f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.3f);
        }
        else
        {
            Destroy(gameObject);
        }

        sfxSource.volume = sfxVolume;
        musicSource.volume = musicVolume;

    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (musicSource.clip == musicClip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = musicClip;
        musicSource.loop = loop;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }
    private void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = sfxVolume;

    }

    void OnEnable()
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
        }
    }
}
