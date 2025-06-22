using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public enum CheckpointType { City, Forest /*, Castle if needed */ }

    [Header("Checkpoint Settings")]
    [SerializeField] private CheckpointType checkpointType;

    [Header("Visuals")]
    [SerializeField] private GameObject texture_OFF;
    [SerializeField] private GameObject texture_ON;

    private bool isActivated = false;

    private void Start()
    {
        // Start with OFF active, ON inactive
        texture_OFF.SetActive(true);
        texture_ON.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated) return; // Already activated

        if (!collision.CompareTag("Player")) return;

        isActivated = true;

        // Swap textures
        texture_OFF.SetActive(false);
        texture_ON.SetActive(true);

        // Play sound
        SoundManager.Instance.PlayCheckpointSFX();

        // Tell GameManager to activate checkpoint flag
        switch (checkpointType)
        {
            case CheckpointType.City:
                GameManager.Instance.ActivateCityCheckpoint();
                break;

            case CheckpointType.Forest:
                GameManager.Instance.ActivateForestCheckpoint();
                break;

                // Add Castle or others if needed here
        }

        Debug.Log($"[Checkpoint] Activated {checkpointType} checkpoint.");
    }
}
