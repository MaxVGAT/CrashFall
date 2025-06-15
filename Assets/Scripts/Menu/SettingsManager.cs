using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideSettings : MonoBehaviour
{
    // ----------------------------------------
    // REFERENCES
    // ----------------------------------------
    [Header("References")]
    public CanvasGroup mainMenuGroup;
    public CanvasGroup settingsGroup;
    public CanvasGroup creditsGroup;
    public float fadeTime = 0.3f;

    [Header("Volume")]
    public Slider volumeSlider;


    // ----------------------------------------
    // UNITY EVENTS
    // ----------------------------------------
    private void Start()
    {
        if (settingsGroup != null)
        {
            settingsGroup.alpha = 1;
            settingsGroup.interactable = true;
            settingsGroup.blocksRaycasts = true;
        }

        if (creditsGroup != null)
        {
            creditsGroup.alpha = 1;
            creditsGroup.interactable = true;
            creditsGroup.blocksRaycasts = true;
        }

        settingsGroup.alpha = 0;
        settingsGroup.interactable = false;
        settingsGroup.blocksRaycasts = false;

        creditsGroup.alpha = 0;
        creditsGroup.interactable = false;
        creditsGroup.blocksRaycasts = false;
    }


    // ----------------------------------------
    // PANEL CONTROLS
    // ----------------------------------------
    public void ShowTutorial()
    {
        settingsGroup.alpha = 1;
        settingsGroup.interactable = true;
        settingsGroup.blocksRaycasts = true;
    }

    public void HideTutorial()
    {
        settingsGroup.alpha = 0;
        settingsGroup.interactable = false;
        settingsGroup.blocksRaycasts = false;
    }

    public void ShowSettings()
    {
        settingsGroup.alpha = 1;
        settingsGroup.interactable = true;
        settingsGroup.blocksRaycasts = true;

        mainMenuGroup.interactable = false;
    }

    public void HideSettings()
    {
        settingsGroup.alpha = 0;
        settingsGroup.interactable = false;
        settingsGroup.blocksRaycasts = false;

        mainMenuGroup.interactable = true;
    }

    public void ShowCredits()
    {
        creditsGroup.alpha = 1;
        creditsGroup.interactable = true;
        creditsGroup.blocksRaycasts = true;

        mainMenuGroup.interactable = false;
    }

    public void HideCredits()
    {
        creditsGroup.alpha = 0;
        creditsGroup.interactable = false;
        creditsGroup.blocksRaycasts = false;

        mainMenuGroup.interactable = true;
    }


    // ----------------------------------------
    // VOLUME SETTINGS
    // ----------------------------------------
    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("soundVolume", volumeSlider.value);
    }

    public void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("soundVolume");
    }
}
