using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Teleport;

public class Teleport : MonoBehaviour
{
    //==================================================
    // ENUMS
    //==================================================
    public enum TeleportType { ForestIntro, Forest, TutoJump, TutoPlatform, TutoLevel, Lobby, City, CastleIntro, Castle }

    //==================================================
    // REFERENCES
    //==================================================
    [Header("Player")]
    [SerializeField] private GameObject Player;

    [Header("Camera")]
    [SerializeField] private CameraFollowPlayer cameraFollow;

    //==================================================
    // TELEPORT STATE OBJECTS
    //==================================================
    [Header("TP States")]
    [SerializeField] private GameObject TP_Inactive_Forest;
    [SerializeField] private GameObject TP_Active_Forest;

    //==================================================
    // TELEPORT TYPE
    //==================================================
    [Header("Teleporters")]
    [SerializeField] private TeleportType TP_Type;

    //==================================================
    // STATE VARIABLES
    //==================================================
    private bool isPortalActive = false;
    public static Teleport currentTeleport;

    //==================================================
    // UNITY EVENTS
    //==================================================
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
            Debug.Log("TP_Active_Forest is now: " + TP_Active_Forest.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetPortalActive(false);
            Debug.Log(TP_Active_Forest.activeSelf);
        }

        if (Teleport.currentTeleport == this && Input.GetKeyDown(KeyCode.E))
        {
            switch (TP_Type)
            {
                case TeleportType.Forest:
                    TeleportToForestLevel();
                    break;

                case TeleportType.ForestIntro:
                    if (TP_Active_Forest.activeSelf)
                    {
                        TeleportToForestIntro();
                    }
                    break;

                case TeleportType.TutoJump:
                    TeleportToTutoJump();
                    break;

                case TeleportType.TutoPlatform:
                    TeleportToTutoPlatform();
                    break;

                case TeleportType.TutoLevel:
                    TeleportToTutoLevel();
                    break;

                case TeleportType.Lobby:
                    TeleportTutoToLobby();
                    break;

                case TeleportType.City:
                    TeleportToCityLevel();
                    break;

                case TeleportType.CastleIntro:
                    TeleportToCastleIntro();
                    break;

                case TeleportType.Castle:
                    TeleportToCastleLevel();
                    break;
            }
        }

        if (gameObject.CompareTag("NPC_Knight") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Talking to knight");
        }
    }

    //==================================================
    // PORTAL ACTIVATION
    //==================================================
    private void SetPortalActive(bool active)
    {
        isPortalActive = active;
        TP_Active_Forest.SetActive(isPortalActive);
        TP_Inactive_Forest.SetActive(!isPortalActive);
    }

    //==================================================
    // COLLISION HANDLERS
    //==================================================
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Debug.Log(gameObject.name + " | TP_Type: " + TP_Type);
        currentTeleport = this;
        InteractionPrompt.Instance.ShowPrompt();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (currentTeleport == this)
        {
            currentTeleport = null;
        }
        InteractionPrompt.Instance.HidePrompt();
    }

    //==================================================
    // TELEPORT DESTINATIONS
    //==================================================
    private void TeleportToForestIntro()
    {
        Player.transform.position = new Vector2(-67f, -12.3f);
    }

    private void TeleportToForestLevel()
    {
        SceneManager.LoadScene("ForestLevel");
    }

    private void TeleportToCastleIntro()
    {
        Player.transform.position = new Vector2(-50f, -12.3f);
    }

    private void TeleportToCastleLevel()
    {
        SceneManager.LoadScene("CastleLevel");
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
        Player.transform.position = new Vector2(3f, -3.3f);
    }

    private void TeleportToCityLevel()
    {
        SceneManager.LoadScene("CityLevel");
    }

    //==================================================
    // EXTERNAL CALLS
    //==================================================
    public void SpawnLobby()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame("InGame", "Lobby_Spawn_Point");
        }
    }
}
