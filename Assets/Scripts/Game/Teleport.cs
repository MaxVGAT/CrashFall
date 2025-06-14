using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Teleport;

public class Teleport : MonoBehaviour
{

    public enum TeleportType { Forest, TutoJump, TutoPlatform, TutoLevel, Lobby, City }


    [Header("Player")]
    [SerializeField] private GameObject Player;

    [Header("Camera")]
    [SerializeField] private CameraFollowPlayer cameraFollow;

    [Header("TP States")]
    [SerializeField] private GameObject TP_Inactive_Forest;
    [SerializeField] private GameObject TP_Active_Forest;

    [Header("Teleporters")]
    [SerializeField] private TeleportType TP_Type;
    
    private bool isPortalActive = false;
    public static Teleport currentTeleport;

    private void Start()
    {
        TP_Active_Forest.SetActive(false);
    }

    private void Update()
    {
        cameraFollow.cameraOffset = new Vector3(0, 2f, -10f);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPortalActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetPortalActive(false);
        }

        if (Teleport.currentTeleport == this && Input.GetKeyDown(KeyCode.E))
        {
            switch (TP_Type)
            {
               
                case TeleportType.Forest:
                    if (TP_Active_Forest.activeSelf)
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
                case TeleportType.City:
                    {
                        TeleportToCityLevel();
                        break;
                    }

            }
        }

        if(gameObject.CompareTag("NPC_Knight") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Talking to knight");
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
        if (!collision.CompareTag("Player")) return;

        currentTeleport = this;
        InteractionPrompt.Instance.ShowPrompt();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if(currentTeleport == this)
        {
            currentTeleport = null;
        }
        InteractionPrompt.Instance.HidePrompt();
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
        Vector2 newPos = new Vector2(21.7f, 18f);
        Debug.Log($"ENTERED Teleporter | Type: {TP_Type} | Object: {gameObject.name}");
        Player.transform.position = newPos;
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

    private void TeleportToCityLevel()
    {
        SceneManager.LoadScene("CityLevel");
    }

    public void SpawnLobby()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame("InGame", "Lobby_Spawn_Point");
        }
    }
}
