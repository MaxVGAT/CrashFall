using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // ============================
    // ======= ENUMS ==============
    // ============================
    public enum PickupType { DoubleJump, Dash }

    // ============================
    // ======= SETTINGS ===========
    // ============================
    [Header("Settings")]
    [SerializeField] private GameObject player;      // Player reference
    [SerializeField] private GameObject DJCard;      // Double jump visual
    [SerializeField] private GameObject dashCard;    // Dash ability visual
    [SerializeField] private PickupType cardType;    // Type of ability to unlock

    // Reference to the player's movement script
    public PlayerMove Player;

    // ============================
    // ======= TRIGGERS ===========
    // ============================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Player == null)
        {
            Debug.LogWarning("[ItemPickup] Player component not found!");
            return;
        }

        // Grant appropriate ability based on pickup type
        switch (cardType)
        {
            case PickupType.DoubleJump:
                Player.UnlockDoubleJump();
                break;

            case PickupType.Dash:
                Player.UnlockDash();
                break;
        }

        // Disable pickup and play sound effect
        gameObject.SetActive(false);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PickUpSFX();
        }
        else
        {
            Debug.LogWarning("[ItemPickup] SoundManager.Instance is null!");
        }
    }
}