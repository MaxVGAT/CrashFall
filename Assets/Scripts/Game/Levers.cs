using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Tilemaps;
#endif

public class Levers : MonoBehaviour
{
    // ============================
    // ====== LEVER STATE =========
    // ============================
    [Header("Lever State")]
    [SerializeField] private GameObject Inactive_Lever;  // Lever visual when off
    [SerializeField] private GameObject Active_Lever;    // Lever visual when on

    // ============================
    // ======= FENCE ELEMENTS =====
    // ============================
    [Header("Fence")]
    [SerializeField] private Transform Left_Gate;   // Left gate transform
    [SerializeField] private Transform Right_Gate;  // Right gate transform

    // ============================
    // ======= FENCE STATE ========
    // ============================
    [Header("Fence State")]
    [SerializeField] private Vector3 leftClosedLocalPos;   // Closed position for left gate
    [SerializeField] private Vector3 rightClosedLocalPos;  // Closed position for right gate
    [SerializeField] private Vector3 leftOpenedLocalPos;   // Open position for left gate
    [SerializeField] private Vector3 rightOpenedLocalPos;  // Open position for right gate

    [SerializeField] private Collider2D leftGateCollider;  // Collider for left gate
    [SerializeField] private Collider2D rightGateCollider; // Collider for right gate

    // ============================
    // ===== INTERNAL STATE =======
    // ============================
    private bool isPlayerInside = false;  // Tracks if player is in interaction range
    private bool isLeverActive = false;   // Current lever state

    // ============================
    // ====== UNITY EVENTS ========
    // ============================
    private void Start()
    {
        // Initialize lever and gate states
        if (Active_Lever != null) Active_Lever.SetActive(false);
        if (Inactive_Lever != null) Inactive_Lever.SetActive(true);

        if (Left_Gate != null)
        {
            leftClosedLocalPos = Left_Gate.localPosition;
            leftGateCollider = Left_Gate.GetComponent<Collider2D>();
        }
        else
        {
            Debug.LogWarning("[Levers] Left_Gate is not assigned!");
        }

        if (Right_Gate != null)
        {
            rightClosedLocalPos = Right_Gate.localPosition;
            rightGateCollider = Right_Gate.GetComponent<Collider2D>();
        }
        else
        {
            Debug.LogWarning("[Levers] Right_Gate is not assigned!");
        }

        CloseFence();
    }

    private void Update()
    {
        // Handle lever interaction input
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLeverAndFence();
            Debug.Log("[Levers] Lever is: " + isLeverActive);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("[Levers] Player entered trigger.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("[Levers] Player exited trigger.");
        }
    }

    // ============================
    // ===== LEVER & FENCE CTRL ===
    // ============================
    public void SetLeverActive(bool active)
    {
        // Set lever state and update visuals
        isLeverActive = active;
        if (Active_Lever != null) Active_Lever.SetActive(isLeverActive);
        if (Inactive_Lever != null) Inactive_Lever.SetActive(!isLeverActive);
    }

    private void ToggleLeverAndFence()
    {
        // Toggle lever state and gate position
        isLeverActive = !isLeverActive;

        if (isLeverActive)
        {
            OpenFence();
        }
        else
        {
            CloseFence();
        }
    }

    // ============================
    // ======= GATE ANIMATION =====
    // ============================
    private void OpenFence()
    {
        // Open both gates with rotation and disable colliders
        if (Left_Gate != null)
        {
            Left_Gate.localPosition = leftOpenedLocalPos;
            Left_Gate.localRotation = Quaternion.Euler(0, 0, -90);
            if (leftGateCollider != null) leftGateCollider.enabled = false;
        }

        if (Right_Gate != null)
        {
            Right_Gate.localPosition = rightOpenedLocalPos;
            Right_Gate.localRotation = Quaternion.Euler(0, 0, 90);
            if (rightGateCollider != null) rightGateCollider.enabled = false;
        }
    }

    private void CloseFence()
    {
        // Close both gates and enable colliders
        if (Left_Gate != null)
        {
            Left_Gate.localPosition = leftClosedLocalPos;
            Left_Gate.localRotation = Quaternion.Euler(0, 0, 0);
            if (leftGateCollider != null) leftGateCollider.enabled = true;
        }

        if (Right_Gate != null)
        {
            Right_Gate.localPosition = rightClosedLocalPos;
            Right_Gate.localRotation = Quaternion.Euler(0, 0, 0);
            if (rightGateCollider != null) rightGateCollider.enabled = true;
        }
    }
}