using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideSettings : MonoBehaviour
{

    [Header("References")]
    public CanvasGroup mainMenuGroup;
    public CanvasGroup settingsGroup;
    public float fadeTime = 0.3f;


    //
    // PANEL SETTINGS
    //

    private void Start()
    {

        if (settingsGroup != null)
        {
            settingsGroup.alpha = 1;
            settingsGroup.interactable = true;
            settingsGroup.blocksRaycasts = true;
        }

        settingsGroup.alpha = 0;
        settingsGroup.interactable = false;
        settingsGroup.blocksRaycasts = false;


    }

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

    //
    // VOLUME SETTINGS
    //

    public Slider volumeSlider;

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

    public void ShowSettingsDATA3()
    {
        Debug.Log("SettingsGroup is null? " + (settingsGroup == null));
        if (settingsGroup != null)
        {
            settingsGroup.alpha = 1;
            settingsGroup.interactable = true;
            settingsGroup.blocksRaycasts = true;
        }
        else
        {
            Debug.LogError("SettingsGroup reference lost or destroyed!");
        }
    }

}
