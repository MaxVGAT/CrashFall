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
    [SerializeField] private GameObject texture_OFF;  // Inactive state visual
    [SerializeField] private GameObject texture_ON;   // Active state visual

    private bool isActivated;

    // ============================
    // ========= START ============
    // ============================
    private void Start()
    {
        // Initialize checkpoint visuals
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

        // Update visuals and notify systems
        if (texture_OFF != null)
        {
            texture_OFF.SetActive(false);
        }

        if (texture_ON != null)
        {
            texture_ON.SetActive(true);
        }

        SoundManager.Instance?.PlayCheckpointSFX();

        // Handle checkpoint type-specific logic
        switch (checkpointType)
        {
            case CheckpointType.City:
                GameManager.Instance?.ActivateCityCheckpoint();
                break;
            case CheckpointType.Forest:
                GameManager.Instance?.ActivateForestCheckpoint();
                break;
        }
    }
}