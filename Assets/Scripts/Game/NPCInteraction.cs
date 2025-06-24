using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NPCInteraction : MonoBehaviour
{
    // ============================
    // ======= ENUMS ==============
    // ============================
    public enum InteractionType { OneSentence }

    // ============================
    // ======= SETTINGS ===========
    // ============================
    [Header("Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject player_Obj;
    [SerializeField] private InteractionType interactionType;

    // ============================
    // ======= ONE SENTENCE =======
    // ============================
    [Header("OneSentence")]
    [SerializeField] private GameObject oneSentencePanel;
    [SerializeField] private TMP_Text dialogueComponent;
    [SerializeField] private string dialogueLine;

    [SerializeField] private Animator animator;

    // ============================
    // ======= INTERNAL ===========
    // ============================
    private bool isPlayerNearNPC = false;

    // ============================
    // ======= UNITY EVENTS =======
    // ============================
    private void Start()
    {
        if (oneSentencePanel != null)
            oneSentencePanel.SetActive(false);
    }

    private void Update()
    {
        if (player == null) return;

        // Close dialogue panel on Escape
        if (oneSentencePanel != null && oneSentencePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            oneSentencePanel.SetActive(false);
        }

        // If dialogue panel is active, don't process new interactions
        if (oneSentencePanel != null && oneSentencePanel.activeSelf)
            return;

        // Open dialogue on E key if player near
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
