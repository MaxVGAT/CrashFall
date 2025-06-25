using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deathCounterText;

    //==================================================
    // UNITY EVENTS
    //==================================================
    private void OnEnable()
    {
        // Register scene loaded callback
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unregister to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        UpdateDisplay(SceneManager.GetActiveScene().name);
        UpdateDeathCounter();
    }

    private void Update()
    {
        // Continuously update death count display
        UpdateDeathCounter();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update display when a new scene loads
        UpdateDisplay(scene.name);
        UpdateDeathCounter();
    }

    //==================================================
    // UI UPDATE METHODS
    //==================================================
    private void UpdateDisplay(string sceneName)
    {
        // Hide death counter in main menu
        bool isMainMenu = sceneName == "MainMenu";
        deathCounterText.gameObject.SetActive(!isMainMenu);
    }

    private void UpdateDeathCounter()
    {
        // Update counter text from GameManager
        if (GameManager.Instance != null)
        {
            deathCounterText.text = "Deaths: " + GameManager.Instance.deathCounter;
        }
        else
        {
            deathCounterText.text = "Deaths: 0";
            Debug.LogWarning("[DeathCounter] GameManager.Instance is null!");
        }
    }
}
