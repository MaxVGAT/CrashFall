using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ParticleSystem hoverEffect;
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip openSound;
    public AudioClip closeSound;

    private ParticleSystem currentEffect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverEffect != null)
        {
            currentEffect = Instantiate(hoverEffect, transform.position, hoverEffect.transform.rotation, transform);
            currentEffect.Play();
        }

        if (audioSource != null && hoverSound != null)
        {
            SoundManager.Instance.PlaySFX(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverEffect != null)
        {
            currentEffect.Stop();
            Destroy(currentEffect.gameObject, currentEffect.main.duration);
        }
    }

    public void OpenSFX()
    {
        if (audioSource != null)
        {
            SoundManager.Instance.PlaySFX(openSound);
        }
    }

    public void CloseSFX()
    {
        if (audioSource != null)
        {
            SoundManager.Instance.PlaySFX(closeSound);
        }
    }

    public void MenuToGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame("InGame", "Tuto_Spawn_Point");
        }
    }


}
