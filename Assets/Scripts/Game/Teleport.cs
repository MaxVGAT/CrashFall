using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    [Header("Player")]
    [SerializeField] private GameObject Player;
    //[SerializeField] private GameObject tpConfirmationPanel;
    
    [Header("Disabled TP")]
    [SerializeField] private GameObject TP_Inactive;

    [Header("Active_TP")]
    [SerializeField] private GameObject TP_Active;

    private bool isPortalActive = false;
    private bool isPlayerInside = false;

    private void Start()
    {
        TP_Active.SetActive(false);
    }

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
    private void SetPortalActive(bool active)
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
