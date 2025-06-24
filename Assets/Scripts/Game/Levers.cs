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
    [SerializeField] private GameObject Inactive_Lever;
    [SerializeField] private GameObject Active_Lever;

    // ============================
    // ======= FENCE ELEMENTS =====
    // ============================
    [Header("Fence")]
    [SerializeField] private Transform Left_Gate;
    [SerializeField] private Transform Right_Gate;

    // ============================
    // ======= FENCE STATE ========
    // ============================
    [Header("Fence State")]
    [SerializeField] private Vector3 leftClosedLocalPos;
    [SerializeField] private Vector3 rightClosedLocalPos;
    [SerializeField] private Vector3 leftOpenedLocalPos;
    [SerializeField] private Vector3 rightOpenedLocalPos;

    [SerializeField] private Collider2D leftGateCollider;
    [SerializeField] private Collider2D rightGateCollider;

    // ============================
    // ===== INTERNAL STATE =======
    // ============================
    private bool isPlayerInside = false;
    private bool isLeverActive = false;

    // ============================
    // ====== UNITY EVENTS ========
    // ============================
    private void Start()
    {
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
            //tpConfirmationPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("[Levers] Player exited trigger.");
            //tpConfirmationPanel.SetActive(false);
        }
    }

    // ============================
    // ===== LEVER & FENCE CTRL ===
    // ============================
    public void SetLeverActive(bool active)
    {
        isLeverActive = active;
        if (Active_Lever != null) Active_Lever.SetActive(isLeverActive);
        if (Inactive_Lever != null) Inactive_Lever.SetActive(!isLeverActive);
    }

    private void ToggleLeverAndFence()
    {
        isLeverActive = !isLeverActive;

        if (isLeverActive)
        {
            if (Active_Lever != null) Active_Lever.SetActive(true);
            if (Inactive_Lever != null) Inactive_Lever.SetActive(false);
            OpenFence();
        }
        else
        {
            if (Active_Lever != null) Active_Lever.SetActive(false);
            if (Inactive_Lever != null) Inactive_Lever.SetActive(true);
            CloseFence();
        }
    }

    // ============================
    // ======= GATE ANIMATION =====
    // ============================
    private void OpenFence()
    {
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
