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
    public CanvasGroup bugsGroup;
    public float fadeTime = 0.3f;

    [Header("Volume")]
    public Slider volumeSlider;

    // ----------------------------------------
    // UNITY EVENTS
    // ----------------------------------------
    private void Start()
    {
        // Initialize settings and credits groups to invisible and non-interactable
        if (settingsGroup != null)
        {
            settingsGroup.alpha = 0;
            settingsGroup.interactable = false;
            settingsGroup.blocksRaycasts = false;
        }

        if (creditsGroup != null)
        {
            creditsGroup.alpha = 0;
            creditsGroup.interactable = false;
            creditsGroup.blocksRaycasts = false;
        }

        if (bugsGroup != null)
        {
            bugsGroup.alpha = 0;
            bugsGroup.interactable = false;
            bugsGroup.blocksRaycasts = false;
        }
    }

    // ----------------------------------------
    // PANEL CONTROLS
    // ----------------------------------------
    public void ShowTutorial()
    {
        if (settingsGroup == null) return;

        settingsGroup.alpha = 1;
        settingsGroup.interactable = true;
        settingsGroup.blocksRaycasts = true;
    }

    public void HideTutorial()
    {
        if (settingsGroup == null) return;

        settingsGroup.alpha = 0;
        settingsGroup.interactable = false;
        settingsGroup.blocksRaycasts = false;
    }

    public void ShowSettings()
    {
        if (settingsGroup == null || mainMenuGroup == null) return;

        settingsGroup.alpha = 1;
        settingsGroup.interactable = true;
        settingsGroup.blocksRaycasts = true;

        mainMenuGroup.interactable = false;
    }

    public void HideSettings()
    {
        if (settingsGroup == null || mainMenuGroup == null) return;

        settingsGroup.alpha = 0;
        settingsGroup.interactable = false;
        settingsGroup.blocksRaycasts = false;

        mainMenuGroup.interactable = true;
    }

    public void ShowCredits()
    {
        if (creditsGroup == null || mainMenuGroup == null) return;

        creditsGroup.alpha = 1;
        creditsGroup.interactable = true;
        creditsGroup.blocksRaycasts = true;

        mainMenuGroup.interactable = false;
    }

    public void HideCredits()
    {
        if (creditsGroup == null || mainMenuGroup == null) return;

        creditsGroup.alpha = 0;
        creditsGroup.interactable = false;
        creditsGroup.blocksRaycasts = false;

        mainMenuGroup.interactable = true;
    }

    public void ShowBugs()
    {
        if (bugsGroup == null || mainMenuGroup == null) return;

        bugsGroup.alpha = 1;
        bugsGroup.interactable = true;
        bugsGroup.blocksRaycasts = true;

        mainMenuGroup.interactable = false;
    }

    public void HideBugs()
    {
        if (bugsGroup == null || mainMenuGroup == null) return;

        bugsGroup.alpha = 0;
        bugsGroup.interactable = false;
        bugsGroup.blocksRaycasts = false;

        mainMenuGroup.interactable = true;
    }

    // ----------------------------------------
    // VOLUME SETTINGS
    // ----------------------------------------
    public void SetVolume()
    {
        if (volumeSlider == null) return;

        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    public void SaveVolume()
    {
        if (volumeSlider == null) return;

        PlayerPrefs.SetFloat("soundVolume", volumeSlider.value);
    }

    public void LoadVolume()
    {
        if (volumeSlider == null) return;

        volumeSlider.value = PlayerPrefs.GetFloat("soundVolume", 1f);
    }
}
