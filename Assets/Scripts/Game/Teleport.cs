using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static Teleport;

public class Teleport : MonoBehaviour
{

    public enum TeleportType { Forest, TutoJump, TutoPlatform, TutoLevel, Lobby }


    [Header("Player")]
    [SerializeField] private GameObject Player;
    //[SerializeField] private GameObject tpConfirmationPanel;

    [Header("Camera")]
    [SerializeField] private CameraFollowPlayer cameraFollow;

    [Header("TP States")]
    [SerializeField] private GameObject TP_Inactive_Forest;
    [SerializeField] private GameObject TP_Active_Forest;

    [Header("Teleporters")]
    [SerializeField] private TeleportType TP_Type;
    
    private bool isPortalActive = false;
    private bool isPlayerInside = false;

    private void Start()
    {
        cameraFollow.cameraOffset = new Vector3(0, 2f, -10f);
        TP_Active_Forest.SetActive(false);
        Debug.Log($"{gameObject.name} TP_Type is set to: {TP_Type}");
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

        if (isPlayerInside == true && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Teleporting using type: " + TP_Type);
            switch (TP_Type)
            {
               
                case TeleportType.Forest:
                    if (TP_Active_Forest == true)
                    {
                        TeleportToForest();
                    }
                    break;
                case TeleportType.TutoJump:
                    {
                        TeleportToTutoJump();
                        break;
                    }
                case TeleportType.TutoPlatform:
                    {
                        TeleportToTutoPlatform();
                        break;
                    }
                case TeleportType.TutoLevel:
                    {
                        TeleportToTutoLevel();
                        break;
                    }
                case TeleportType.Lobby:
                    {
                        TeleportTutoToLobby();
                        break;
                    }

            }
        }
    }

    private void SetPortalActive(bool active)
    {
        isPortalActive = active;
        TP_Active_Forest.SetActive(isPortalActive);
        TP_Inactive_Forest.SetActive(!isPortalActive);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            InteractionPrompt.Instance.ShowPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            InteractionPrompt.Instance.HidePrompt();
        }
    }

    private void TeleportToForest()
    {
        Player.transform.position = new Vector2(-68f, -5.3f);
    }

    private void TeleportToTutoJump()
    {
        Player.transform.position = new Vector2(2.6f, 13f);
    }

    private void TeleportToTutoPlatform()
    {
        Player.transform.position = new Vector2(21.7f, 18f);
    }

    private void TeleportToTutoLevel()
    {
        Player.transform.position = new Vector2(47f, 16.5f);
    }

    private void TeleportTutoToLobby()
    {
        cameraFollow.cameraOffset = new Vector3(0, 0f, -10f);
        Player.transform.position = new Vector2(3f, -3.3f);
    }

    public void SpawnLobby()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame("InGame", "Lobby_Spawn_Point");
        }
    }
}
