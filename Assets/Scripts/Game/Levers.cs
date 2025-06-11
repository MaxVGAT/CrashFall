using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Tilemaps;
#endif

public class Levers : MonoBehaviour
{
    [Header("Lever State")]
    [SerializeField] private GameObject Inactive_Lever;
    [SerializeField] private GameObject Active_Lever;

    [Header("Fence")]
    [SerializeField] private Transform Left_Gate;
    [SerializeField] private Transform Right_Gate;

    [Header("Fence State")]
    [SerializeField] private Vector3 leftClosedLocalPos;
    [SerializeField] private Vector3 rightClosedLocalPos;
    [SerializeField] private Vector3 leftOpenedLocalPos;
    [SerializeField] private Vector3 rightOpenedLocalPos;

    [SerializeField] private Collider2D leftGateCollider;
    [SerializeField] private Collider2D rightGateCollider;
    
    private bool isPlayerInside = false;
    private bool isLeverActive = false;

    private void Start()
    {
        Active_Lever.SetActive(false);

        leftClosedLocalPos = Left_Gate.localPosition;
        rightClosedLocalPos = Right_Gate.localPosition;
        leftGateCollider = Left_Gate.GetComponent<Collider2D>();
        rightGateCollider = Right_Gate.GetComponent<Collider2D>();

        CloseFence();
        Debug.Log(isLeverActive);
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLeverAndFence();
            Debug.Log("Lever is :" + isLeverActive);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log(isPlayerInside);
            //tpConfirmationPanel.SetActive(true);
        }
    }

    public void SetLeverActive(bool active)
    {
        isLeverActive = active;
        Active_Lever.SetActive(isLeverActive);
        Inactive_Lever.SetActive(!isLeverActive);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log(isPlayerInside);
            //tpConfirmationPanel.SetActive(false);
        }
    }

    private void ToggleLeverAndFence()
    {
        isLeverActive = !isLeverActive;

        if (isLeverActive)
        {
            Active_Lever.SetActive(true);
            Inactive_Lever.SetActive(false);
            OpenFence();
        }
        else
        {
            Active_Lever.SetActive(false);
            Inactive_Lever.SetActive(true);
            CloseFence();
        }
    }

    private void OpenFence()
    {
        Left_Gate.localPosition = leftOpenedLocalPos;
        Right_Gate.localPosition = rightOpenedLocalPos;
        Left_Gate.localRotation = Quaternion.Euler(0, 0, -90);
        Right_Gate.localRotation = Quaternion.Euler(0, 0, 90);

        leftGateCollider.enabled = false;
        rightGateCollider.enabled = false;
    }

    private void CloseFence()
    {
        Left_Gate.localPosition = leftClosedLocalPos;
        Right_Gate.localPosition = rightClosedLocalPos;
        Left_Gate.localRotation = Quaternion.Euler(0, 0, 0);
        Right_Gate.localRotation = Quaternion.Euler(0, 0, 0);

        leftGateCollider.enabled = true;
        rightGateCollider.enabled = true;
    }
}
