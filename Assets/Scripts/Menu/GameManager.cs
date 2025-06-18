using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    PlayerMove Player;
    Checkpoints Checkpoint;

    //==================================================
    // SINGLETON
    //==================================================
    public static GameManager Instance { get; private set; }

    //==================================================
    // CURSOR
    //==================================================
    public Texture2D customCursor;
    public Vector2 hotspot = Vector2.zero;

    //==================================================
    // SCENE & SPAWN CONTROL
    //==================================================
    public static string nextScene = "InGame";
    public static string nextSpawn = "Tuto_Spawn_Point";

    public GameObject LobbySpawn;
    public GameObject CitySpawn;
    //public GameObject ForestSpawn;
    //public GameObject CastleSpawn;

    public GameObject CityCPCheck;

    //==================================================
    // PLAYER POWER STATE
    //==================================================

    public bool canDoubleJump = false;
    public bool hasUnlockedDash = false;

    //==================================================
    // SCORE AND TIME
    //==================================================

    [SerializeField] public int deathCounter;

    //==================================================
    // LIFECYCLE
    //==================================================
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Cursor.SetCursor(customCursor, hotspot, CursorMode.Auto);
    }

    void OnDisable()
    {
        Debug.Log("Settings disabled at " + Time.time);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu" && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    void OnDestroy()
    {
        Debug.Log("Settings destroyed at " + Time.time);
    }

    //==================================================
    // GAME FLOW
    //==================================================
    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame(string sceneName, string spawnPoint)
    {
        nextScene = sceneName;
        nextSpawn = spawnPoint;
        SceneManager.LoadScene(sceneName);
    }

    //==================================================
    // PAUSE
    //==================================================

    public void PauseGame()
    {

    }

    //==================================================
    // DEATH CONTROL
    //==================================================

    public void RespawnPlayer()
    {
        deathCounter++;

        string diedInScene = SceneManager.GetActiveScene().name;

        switch(diedInScene)
        {
            case "InGame":
                Debug.Log("Spawning at " + LobbySpawn.transform.position);
                PlayerMove.Instance.transform.position = LobbySpawn.transform.position + new Vector3(2.5f, -3f, 0f);
                break;

            case "CityLevel":
                if (!Checkpoints.Instance.isCityON)
                {
                    PlayerMove.Instance.transform.position = CitySpawn.transform.position;
                }
                else
                {
                    PlayerMove.Instance.transform.position = CityCPCheck.transform.position;
                }
                break;
            //case "ForestLevel":
            //    Player.transform.position = ForestSpawn.transform.position;
            //    break;
            //case "CastleLevel":
            //    Player.transform.position = CastleSpawn.transform.position;
            //    break;
            default:
                Debug.LogError("No active scene!");
                break;
        }
    }

    //==================================================
    // SCORE CONTROL
    //==================================================


}
