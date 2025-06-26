using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CloseGame : MonoBehaviour
{
    // ============================
    // ======== VARIABLES =========
    // ============================
    private bool isPlayerInside = false;

    // =========================
    // ======== UPDATE =========
    // =========================
    private void Update()
    {
        // Close game when player presses E while inside trigger
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInside)
        {
            StartCoroutine(SendScoreAndQuit());
        }
    }

    private IEnumerator SendScoreAndQuit()
    {

        int finalScore = Mathf.Max(0, 999999 - (GameManager.Instance.deathCounter * 1000 + Mathf.RoundToInt(GameManager.Instance.timer * 100)));

        yield return GameManager.Instance.PostScore(finalScore);

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.GameCleared();

    }

    // ============================
    // === TRIGGER INTERACTIONS ===
    // ============================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInside = true;

        // Show interaction prompt
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

        // Hide interaction prompt
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