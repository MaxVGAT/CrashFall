using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    PlayerMove Player;

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
        GameObject spawnPoint = GameObject.Find(nextSpawn);
        if (spawnPoint != null)
        {
            Player.transform.position = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("Spawn point not found: " + nextSpawn);
        }
    }
}
