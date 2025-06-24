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
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject DJCard;
    [SerializeField] private GameObject dashCard;
    [SerializeField] private PickupType cardType;

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

        switch (cardType)
        {
            case PickupType.DoubleJump:
                Player.UnlockDoubleJump();
                break;

            case PickupType.Dash:
                Player.UnlockDash();
                break;
        }

        // Disable this pickup and play sound
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
