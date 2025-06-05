using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    [Header("Player")]
    [SerializeField] private GameObject Player;
    //[SerializeField] private GameObject tpConfirmationPanel;
    private bool isPlayerInside = false;

    [Header("Disabled TP")]
    [SerializeField] private GameObject TP_Inactive;

    [Header("Active_TP")]
    [SerializeField] private GameObject TP_Active;

    bool isPortalActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPortalActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetPortalActive(false);
        }

        if (isPlayerInside == true && isPortalActive == true && Input.GetKeyDown(KeyCode.E))
        {
            TeleportPlayer();
        }
    }
    public void SetPortalActive(bool active)
    {
        isPortalActive = active;
        TP_Active.SetActive(isPortalActive);
        TP_Inactive.SetActive(!isPortalActive);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log(isPlayerInside);
            //tpConfirmationPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log(isPlayerInside);
            //tpConfirmationPanel.SetActive(false);
        }
    }

    void TeleportPlayer()
    {
        Player.transform.position = new Vector2(-68f, -5.3f);
    }
}
