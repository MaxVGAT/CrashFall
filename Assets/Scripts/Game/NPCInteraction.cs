using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NPCInteraction : MonoBehaviour
{
    // ============================
    // ======= ENUMS ==============
    // ============================
    public enum InteractionType { OneSentence }  // Type of NPC interaction

    // ============================
    // ======= SETTINGS ===========
    // ============================
    [Header("Settings")]
    [SerializeField] private Transform player;  // Player transform reference
    [SerializeField] private GameObject player_Obj;  // Player gameobject
    [SerializeField] private InteractionType interactionType;  // Interaction mode

    // ============================
    // ======= ONE SENTENCE =======
    // ============================
    [Header("OneSentence")]
    [SerializeField] private GameObject oneSentencePanel;  // Dialogue UI panel
    [SerializeField] private TMP_Text dialogueComponent;  // Text display component
    [SerializeField] private string dialogueLine;  // NPC's dialogue text

    [SerializeField] private Animator animator;  // Optional NPC animations

    // ============================
    // ======= INTERNAL ===========
    // ============================
    private bool isPlayerNearNPC = false;  // Tracks player proximity

    // ============================
    // ======= UNITY EVENTS =======
    // ============================
    private void Start()
    {
        // Initialize UI state
        if (oneSentencePanel != null)
            oneSentencePanel.SetActive(false);
    }

    private void Update()
    {
        if (player == null) return;

        // Handle dialogue closing
        if (oneSentencePanel != null && oneSentencePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            oneSentencePanel.SetActive(false);
            return;
        }

        // Prevent new interactions while dialogue is active
        if (oneSentencePanel != null && oneSentencePanel.activeSelf)
            return;

        // Process interaction input
        if (isPlayerNearNPC && Input.GetKeyDown(KeyCode.E))
        {
            switch (interactionType)
            {
                case InteractionType.OneSentence:
                    if (SoundManager.Instance != null)
                        SoundManager.Instance.PlayNPCSFX();

                    if (dialogueComponent != null)
                        dialogueComponent.text = dialogueLine;

                    if (oneSentencePanel != null)
                        oneSentencePanel.SetActive(true);
                    break;
            }
        }

        // Manage interaction prompt visibility
        if (oneSentencePanel != null && oneSentencePanel.activeSelf)
        {
            InteractionPrompt.Instance?.HidePrompt();
        }
        else if (isPlayerNearNPC)
        {
            InteractionPrompt.Instance?.ShowPrompt();
        }
    }

    // ============================
    // ======= TRIGGER EVENTS =====
    // ============================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerNearNPC = true;
        InteractionPrompt.Instance?.ShowPrompt();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerNearNPC = false;
        InteractionPrompt.Instance?.HidePrompt();

        if (oneSentencePanel != null)
            oneSentencePanel.SetActive(false);
    }
}