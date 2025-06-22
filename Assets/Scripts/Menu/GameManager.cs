using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Texture2D customCursor;
    public Vector2 hotspot = Vector2.zero;

    public static string nextScene = "InGame";
    public static string nextSpawn = "Tuto_Spawn_Point";

    public GameObject LobbySpawn;
    public GameObject CitySpawn;
    public GameObject ForestSpawn;

    [Header("UI Elements")]
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] public TextMeshProUGUI finalTimeText;
    [SerializeField] public TextMeshProUGUI finalDeathText;
    [SerializeField] private GameObject pauseDimmerPanel;
    [SerializeField] private GameObject pauseText;

    public bool isCityCheckpointActive = false;
    public bool isForestCheckpointActive = false;

    public bool canDoubleJump = false;
    public bool hasUnlockedDash = false;

    [SerializeField] public int deathCounter;


    private float timer = 0f;
    private bool isTimerRunning = false;
    public bool isPaused = false;
    public bool isResultShown = false;

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000f) % 1000f);
        return $"Timer: {minutes:00}:{seconds:00}.{milliseconds:000}";
    }

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

        Debug.Log(isPaused);
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
            {
                timerText.text = "";
            }
            else
            {
                timerText.text = FormatTime(timer);
            }
        }
    }

    void OnDestroy()
    {
        Debug.Log("Settings destroyed at " + Time.time);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        Debug.Log("Settings disabled at " + Time.time);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset UI references on scene load to avoid stale references
        timerText = null;
        finalTimeText = null;
        finalDeathText = null;

        StartCoroutine(DelayedSceneInit(scene));
    }

    private IEnumerator DelayedSceneInit(Scene scene)
    {
        yield return null; // Wait a frame for scene to fully load and objects to initialize

        if (scene.name == "InGame")
        {
            timer = 0f;
            isTimerRunning = true;
        }
        else if (scene.name == "CastleLevel")
        {
            isTimerRunning = false;
            Debug.Log("[GameManager] Timer stopped. Final time: " + FormatTime(timer));
        }

        if (scene.name == "CastleLevel")
        {
            GameObject finalTimeObject = GameObject.Find("FinalTimeText");
            GameObject finalDeathObject = GameObject.Find("FinalDeathText");

            if (finalTimeObject != null)
            {
                finalTimeText = finalTimeObject.GetComponent<TextMeshProUGUI>();
                finalTimeText.text = "Time: " + FormatTime(timer);
            }

            if (finalDeathObject != null)
            {
                finalDeathText = finalDeathObject.GetComponent<TextMeshProUGUI>();
                finalDeathText.text = "Deaths: " + deathCounter;
            }

            Debug.Log("[GameManager] Final results displayed - Time: " + FormatTime(timer) + ", Deaths: " + deathCounter);
        }
        else
        {
            if (timerText == null)
            {
                GameObject timerObject = GameObject.Find("TimerText");
                if (timerObject != null)
                {
                    timerText = timerObject.GetComponent<TextMeshProUGUI>();
                    Debug.Log("[GameManager] Timer text found in new scene.");
                }
            }
        }

        if (scene.name != "CastleLevel")
        {
            yield return null; // Give PlayerMove one more frame to initialize if needed

            GameObject spawnPoint = GameObject.Find(nextSpawn);

            if (spawnPoint == null)
            {
                Debug.LogError($"[GameManager] Couldn't find spawn point named '{nextSpawn}'");
                yield break;
            }

            if (PlayerMove.Instance == null)
            {
                Debug.LogError("[GameManager] PlayerMove.Instance is null!");
                yield break;
            }

            PlayerMove.Instance.transform.position = spawnPoint.transform.position;
            Debug.Log($"[GameManager] Player moved to spawn point '{nextSpawn}' at position {spawnPoint.transform.position}");
        }
        else
        {
            Debug.Log("[GameManager] Results scene loaded — no player spawn logic executed.");
        }
    }

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

        if (sceneName == "CityLevel")
        {
            isCityCheckpointActive = false;
        }
        else if (sceneName == "ForestLevel")
        {
            isForestCheckpointActive = false;
        }
        else if (sceneName == "CastleLevel")
        {
            isCityCheckpointActive = false;
            isForestCheckpointActive = false;
            isTimerRunning = false;
        }

        SceneManager.LoadScene(sceneName);
    }

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

    public void RespawnPlayer()
    {
        deathCounter++;

        string currentScene = SceneManager.GetActiveScene().name;
        SoundManager.Instance.PlayDeathSFX();

        switch (currentScene)
        {
            case "InGame":
                PlayerMove.Instance.transform.position = LobbySpawn.transform.position + new Vector3(2.5f, -3f, 0f);
                break;

            case "CityLevel":
                PlayerMove.Instance.transform.position = isCityCheckpointActive ? new Vector3(65f, 0.5f, 0f) : new Vector3(1.5f, 1f, 0f);
                break;

            case "ForestLevel":
                PlayerMove.Instance.transform.position = isForestCheckpointActive ? new Vector3(56f, 3.7f, 0f) : new Vector3(5f, 0.45f, 0f);
                break;

            default:
                Debug.LogError("No active scene!");
                break;
        }
    }

    public void PauseGame()
    {
            isPaused = !isPaused;

            Time.timeScale = isPaused ? 0f : 1f;

            SoundManager.Instance.PauseSFX();
    }

    public void ShowResults(int deaths, string time)
    {
        isResultShown = true;
        resultsPanel.SetActive(true);

        finalDeathText.text = "Deaths: " + deathCounter;
        finalTimeText.text = "Time: " + FormatTime(timer);
    }

    public string GetFormattedTime()
    {
        return FormatTime(timer);
    }

    public void ReturnToMenu()
    {
        if(isResultShown && Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
