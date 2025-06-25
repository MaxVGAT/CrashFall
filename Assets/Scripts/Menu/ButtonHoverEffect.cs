using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //==================================================
    // REFERENCES
    //==================================================
    public ParticleSystem hoverEffect;
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip openSound;
    public AudioClip closeSound;

    private ParticleSystem currentEffect;

    //==================================================
    // UI HOVER EFFECT
    //==================================================
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Spawn and play hover VFX on pointer enter
        if (hoverEffect != null)
        {
            currentEffect = Instantiate(hoverEffect, transform.position, hoverEffect.transform.rotation, transform);
            currentEffect.Play();
        }

        // Play hover SFX
        if (audioSource != null && hoverSound != null)
        {
            SoundManager.Instance?.PlaySFX(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Stop and clean up the hover effect
        if (currentEffect != null)
        {
            currentEffect.Stop();
            Destroy(currentEffect.gameObject, currentEffect.main.duration);
            currentEffect = null;
        }
    }

    //==================================================
    // SFX HANDLERS
    //==================================================
    public void OpenSFX()
    {
        // Play open sound (e.g., for opening menus)
        if (audioSource != null && openSound != null)
        {
            SoundManager.Instance?.PlaySFX(openSound);
        }
    }

    public void CloseSFX()
    {
        // Play close sound (e.g., for closing menus)
        if (audioSource != null && closeSound != null)
        {
            SoundManager.Instance?.PlaySFX(closeSound);
        }
    }

    //==================================================
    // SCENE TRANSITION
    //==================================================
    public void StartGameAtTuto()
    {
        // Shortcut to load the tutorial scene
        MenuToGame("InGame", "Tuto_Spawn_Point");
    }

    public void MenuToGame(string sceneName, string spawnPoint)
    {
        // Start a new scene using GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame(sceneName, spawnPoint);
        }
    }
}
