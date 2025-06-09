using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Texture2D customCursor;
    public Vector2 hotspot = Vector2.zero;

    public static string nextSpawn = "Tuto_Spawn_Point";
    public static string nextScene = "InGame";

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

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(customCursor, hotspot, CursorMode.Auto);
    }

    // Update is called once per frame
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
