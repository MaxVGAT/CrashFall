using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    // ============================
    // ======= ENUM & FIELDS ======
    // ============================
    public enum CheckpointType { City, Forest /* TODO: Castle */ }

    [Header("Checkpoint Settings")]
    [SerializeField] private CheckpointType checkpointType;

    [Header("Visuals")]
    [SerializeField] private GameObject texture_OFF;
    [SerializeField] private GameObject texture_ON;

    private bool isActivated;

    // ============================
    // ========= START ============
    // ============================
    private void Start()
    {
        if (texture_OFF != null) texture_OFF.SetActive(true);
        if (texture_ON != null) texture_ON.SetActive(false);
    }

    // ============================
    // ======= TRIGGER EVENT ======
    // ============================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated || !collision.CompareTag("Player")) return;

        isActivated = true;

        // Visual feedback toggle
        if (texture_OFF != null) texture_OFF.SetActive(false);
        if (texture_ON != null) texture_ON.SetActive(true);

        // Play checkpoint sound if available
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayCheckpointSFX();
        }
        else
        {
            Debug.LogWarning("[Checkpoint] SoundManager.Instance is null.");
        }

        // Inform GameManager about checkpoint activation
        if (GameManager.Instance != null)
        {
            switch (checkpointType)
            {
                case CheckpointType.City:
                    GameManager.Instance.ActivateCityCheckpoint();
                    break;

                case CheckpointType.Forest:
                    GameManager.Instance.ActivateForestCheckpoint();
                    break;

                    // TODO: Add Castle or others here
            }

            Debug.Log($"[Checkpoint] Activated {checkpointType} checkpoint.");
        }
        else
        {
            Debug.LogWarning("[Checkpoint] GameManager.Instance is null!");
        }
    }
}
