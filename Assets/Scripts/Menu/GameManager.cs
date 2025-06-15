using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    public static string nextSpawn = "Tuto_Spawn_Point";
    public static string nextScene = "InGame";

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
            Destroy(gameObject); // Avoid duplicates
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
}
