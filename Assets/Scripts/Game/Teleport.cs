using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    //==================================================
    // ENUMS
    //==================================================
    public enum TeleportType { ForestIntro, Forest, TutoJump, TutoPlatform, TutoLevel, Lobby, LobbyScene, CityIntro, City, CastleIntro, Castle }

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
    [SerializeField] private GameObject TP_Inactive_Castle;
    [SerializeField] private GameObject TP_Active_Castle;

    //==================================================
    // TELEPORT TYPE
    //==================================================
    [Header("Teleporters")]
    [SerializeField] private TeleportType TP_Type;

    //==================================================
    // STATE VARIABLES
    //==================================================
    private bool isForestPortalActive = false;
    private bool isCastlePortalActive = false;
    public static Teleport currentTeleport;

    //==================================================
    // UNITY EVENTS
    //==================================================
    private void Start()
    {
        TP_Active_Forest.SetActive(false);
        TP_Active_Castle.SetActive(false);

        TP_Inactive_Forest.SetActive(true);
        TP_Inactive_Castle.SetActive(true);
    }

    private void Update()
    {
        // Adjust camera offset every frame — might be better elsewhere, but unchanged per your request
        cameraFollow.cameraOffset = new Vector3(0, 2f, -10f);

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.canDoubleJump)
                SetForestPortalActive(true);

            if (GameManager.Instance.hasUnlockedDash)
                SetCastlePortalActive(true);
        }

        if (currentTeleport == this && Input.GetKeyDown(KeyCode.E))
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayTeleportSFX();

            switch (TP_Type)
            {
                case TeleportType.Forest:
                    TeleportToForestLevel();
                    break;

                case TeleportType.ForestIntro:
                    if (TP_Active_Forest.activeSelf)
                        TeleportToForestIntro();
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

                case TeleportType.LobbyScene:
                    TeleportToLobbyScene();
                    break;

                case TeleportType.CityIntro:
                    TeleportToCityIntro();
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
    }

    //==================================================
    // PORTAL ACTIVATION
    //==================================================
    private void SetForestPortalActive(bool active)
    {
        isForestPortalActive = active;
        TP_Active_Forest.SetActive(isForestPortalActive);
        TP_Inactive_Forest.SetActive(!isForestPortalActive);
    }

    private void SetCastlePortalActive(bool active)
    {
        isCastlePortalActive = active;
        TP_Active_Castle.SetActive(isCastlePortalActive);
        TP_Inactive_Castle.SetActive(!isCastlePortalActive);
    }

    //==================================================
    // COLLISION HANDLERS
    //==================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        currentTeleport = this;
        InteractionPrompt.Instance?.ShowPrompt();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (currentTeleport == this)
            currentTeleport = null;

        InteractionPrompt.Instance?.HidePrompt();
    }

    //==================================================
    // TELEPORT DESTINATIONS
    //==================================================
    private void TeleportToCityIntro()
    {
        Player.transform.position = new Vector3(46f, -3.3f);
    }

    private void TeleportToCityLevel()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.isForestCheckpointActive = false;

        GameManager.Instance?.StartGame("CityLevel", "City_Spawn");
    }

    private void TeleportToForestIntro()
    {
        Player.transform.position = new Vector2(-67f, -12.3f);
    }

    private void TeleportToForestLevel()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isCityCheckpointActive = false;
            GameManager.Instance.isForestCheckpointActive = false;
        }

        GameManager.Instance?.StartGame("ForestLevel", "Forest_Spawn");
    }

    private void TeleportToCastleIntro()
    {
        Player.transform.position = new Vector2(47f, -22.3f);
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
        Player.transform.position = new Vector2(21.7f, 18f);
    }

    private void TeleportToTutoLevel()
    {
        Player.transform.position = new Vector2(47f, 16.5f);
    }

    private void TeleportTutoToLobby()
    {
        if (GameManager.Instance?.LobbySpawn != null)
        {
            Player.transform.position = GameManager.Instance.LobbySpawn.transform.position + new Vector3(2.5f, -3f, 0f);
        }
    }

    private void TeleportToLobbyScene()
    {
        Debug.Log("TeleportToTutoScene() called - should spawn at: Lobby_Spawn");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.isCityCheckpointActive = false;
            GameManager.Instance.isForestCheckpointActive = false;
            GameManager.Instance.StartGame("InGame", "LobbySpawn");
        }
    }
}
