using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //==================================================
    // SINGLETON INSTANCE
    //==================================================
    public static GameManager Instance { get; private set; }

    //==================================================
    // CURSOR SETTINGS
    //==================================================
    public Texture2D customCursor;
    public Vector2 hotspot = Vector2.zero;

    //==================================================
    // NEXT SCENE & SPAWN POINT (STATIC)
    //==================================================
    public static string nextScene = "InGame";
    public static string nextSpawn = "Tuto_Spawn_Point";

    //==================================================
    // SPAWN POINT REFERENCES
    //==================================================
    public GameObject LobbySpawn;
    public GameObject CitySpawn;
    public GameObject ForestSpawn;

    //==================================================
    // UI ELEMENTS
    //==================================================
    [Header("UI Elements")]
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] public TextMeshProUGUI finalTimeText;
    [SerializeField] public TextMeshProUGUI finalDeathText;
    [SerializeField] private GameObject pauseDimmerPanel;
    [SerializeField] private GameObject pauseText;

    //==================================================
    // GAME STATE FLAGS
    //==================================================
    public bool isCityCheckpointActive = false;
    public bool isForestCheckpointActive = false;

    public bool canDoubleJump = false;
    public bool hasUnlockedDash = false;

    [SerializeField] public int deathCounter;

    //==================================================
    // TIMER
    //==================================================
    private float timer = 0f;
    private bool isTimerRunning = false;
    public bool isPaused = false;
    public bool isResultShown = false;

    //==================================================
    // UNITY EVENTS
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

    private void Start()
    {
        Cursor.SetCursor(customCursor, hotspot, CursorMode.Auto);
        pauseCanvas.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu" && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        if (isTimerRunning)
        {
            timer += Time.deltaTime;
        }

        if (timerText != null)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
                timerText.text = "";
            else
                timerText.text = FormatTime(timer);
        }

        Debug.Log(isTimerRunning);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {
        Debug.Log("GameManager destroyed at " + Time.time);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isTimerRunning = true;

        if (!string.IsNullOrEmpty(nextSpawn))
        {
            GameObject player = GameObject.FindWithTag("Player"); // Assuming your player is tagged correctly
            GameObject spawnPoint = GameObject.Find(nextSpawn);

            if (player != null && spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;
                Debug.Log($"[GameManager] Player spawned at: {nextSpawn}");
            }
            else
            {
                Debug.LogWarning("[GameManager] Could not find Player or Spawn Point: " + nextSpawn);
            }
        }

        Debug.Log(nextSpawn);
    }


    //==================================================
    // TIMER FORMATTING
    //==================================================
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000f) % 1000f);
        return $"Timer: {minutes:00}:{seconds:00}.{milliseconds:000}";
    }

    //==================================================
    // GAME CONTROL METHODS
    //==================================================
    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame(string sceneName, string spawnPoint)
    {
        nextScene = sceneName;
        nextSpawn = spawnPoint;

        switch (sceneName)
        {
            case "CityLevel":
                isCityCheckpointActive = false;
                break;

            case "ForestLevel":
                isForestCheckpointActive = false;
                break;

            case "CastleLevel":
                isCityCheckpointActive = false;
                isForestCheckpointActive = false;
                isTimerRunning = false;
                break;
        }

        SceneManager.LoadScene(sceneName);
    }

    //==================================================
    // CHECKPOINTS
    //==================================================
    public void ActivateCityCheckpoint()
    {
        isCityCheckpointActive = true;
        Debug.Log("[GameManager] City checkpoint activated.");
    }

    public void ActivateForestCheckpoint()
    {
        isForestCheckpointActive = true;
        Debug.Log("[GameManager] Forest checkpoint activated.");
    }

    //==================================================
    // RESPAWN PLAYER
    //==================================================
    public void RespawnPlayer()
    {
        deathCounter++;

        string currentScene = SceneManager.GetActiveScene().name;

        SoundManager.Instance?.PlayDeathSFX();

        switch (currentScene)
        {
            case "InGame":
                PlayerMove.Instance.transform.position = LobbySpawn.transform.position + new Vector3(2.5f, -3f, 0f);
                break;

            case "CityLevel":
                PlayerMove.Instance.transform.position = isCityCheckpointActive
                    ? new Vector3(65f, 0.5f, 0f)
                    : new Vector3(1.5f, 1f, 0f);
                break;

            case "ForestLevel":
                PlayerMove.Instance.transform.position = isForestCheckpointActive
                    ? new Vector3(56f, 3.7f, 0f)
                    : new Vector3(5f, 0.45f, 0f);
                break;

            default:
                Debug.LogError("[GameManager] RespawnPlayer: Unknown scene!");
                break;
        }
    }

    //==================================================
    // PAUSE GAME
    //==================================================
    public void PauseGame()
    {
        isPaused = !isPaused;
        pauseCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        SoundManager.Instance?.PauseSFX();
    }

    //==================================================
    // SHOW RESULTS
    //==================================================
    public void ShowResults(int deaths, string time)
    {
        isResultShown = true;

        if (resultsPanel != null)
            resultsPanel.SetActive(true);

        if (finalDeathText != null)
            finalDeathText.text = "Deaths: " + deathCounter;

        if (finalTimeText != null)
            finalTimeText.text = "Time: " + FormatTime(timer);
    }

    //==================================================
    // GET FORMATTED TIME
    //==================================================
    public string GetFormattedTime()
    {
        return FormatTime(timer);
    }

    //==================================================
    // RETURN TO MENU ON RESULT SCREEN
    //==================================================
    public void ReturnToMenu()
    {
        if (isResultShown && Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
