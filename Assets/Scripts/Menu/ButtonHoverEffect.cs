using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(hoverEffect != null)
        {
            currentEffect.Stop();
            Destroy(currentEffect.gameObject, currentEffect.main.duration);
        }
    }

    public void OpenSFX()
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }

    public void CloseSFX()
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }
}
