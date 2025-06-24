using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    //==================================================
    // SINGLETON INSTANCE
    //==================================================
    public static UnlockDoor Instance { get; private set; }

    //==================================================
    // SETTINGS
    //==================================================
    [Header("Settings")]
    [SerializeField] private GameObject City_Door;
    [SerializeField] private Animator animator;

    //==================================================
    // UNITY EVENTS
    //==================================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        SoundManager.Instance?.PlayDoorOpenSFX();
        if (animator != null)
            animator.SetBool("isPlayerNear", true);

        InteractionPrompt.Instance?.ShowPrompt();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        SoundManager.Instance?.PlayDoorCloseSFX();
        if (animator != null)
            animator.SetBool("isPlayerNear", false);

        InteractionPrompt.Instance?.HidePrompt();
    }
}
