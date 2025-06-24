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
        if (hoverEffect != null)
        {
            currentEffect = Instantiate(hoverEffect, transform.position, hoverEffect.transform.rotation, transform);
            currentEffect.Play();
        }

        if (audioSource != null && hoverSound != null)
        {
            SoundManager.Instance?.PlaySFX(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
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
        if (audioSource != null && openSound != null)
        {
            SoundManager.Instance?.PlaySFX(openSound);
        }
    }

    public void CloseSFX()
    {
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
        MenuToGame("InGame", "Tuto_Spawn_Point");
    }

    public void MenuToGame(string sceneName, string spawnPoint)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame(sceneName, spawnPoint);
        }
    }
}
