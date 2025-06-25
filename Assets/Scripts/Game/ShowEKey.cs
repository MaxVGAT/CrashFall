using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Show prompt when player enters
        if (collision.CompareTag("Player"))
        {
            Debug.Log("[InteractionTrigger] Player entered trigger zone.");
            InteractionPrompt.Instance?.ShowPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Hide prompt when player exits
        if (collision.CompareTag("Player"))
        {
            Debug.Log("[InteractionTrigger] Player exited trigger zone.");
            InteractionPrompt.Instance?.HidePrompt();
        }
    }
}