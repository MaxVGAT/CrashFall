using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public static UnlockDoor Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private GameObject City_Door;
    [SerializeField] private Animator animator;

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

        animator.SetBool("isPlayerNear", true);
        InteractionPrompt.Instance.ShowPrompt();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        animator.SetBool("isPlayerNear", false);
        InteractionPrompt.Instance.HidePrompt();
    }

}
