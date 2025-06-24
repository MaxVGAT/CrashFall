using UnityEngine;

public class CloseGame : MonoBehaviour
{

    private bool isPlayerInside = false;

    // =========================
    // ======== UPDATE =========
    // =========================
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInside)
        {
            Application.Quit();
        }
    }

    // ============================
    // === TRIGGER INTERACTIONS ===
    // ============================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInside = true;

        if (InteractionPrompt.Instance != null)
        {
            InteractionPrompt.Instance.ShowPrompt();
        }
        else
        {
            Debug.LogWarning("[CloseGame] InteractionPrompt.Instance is null on trigger enter.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInside = false;

        if (InteractionPrompt.Instance != null)
        {
            InteractionPrompt.Instance.HidePrompt();
        }
        else
        {
            Debug.LogWarning("[CloseGame] InteractionPrompt.Instance is null on trigger exit.");
        }
    }
}
