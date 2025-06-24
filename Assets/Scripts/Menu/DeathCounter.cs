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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        UpdateDisplay(SceneManager.GetActiveScene().name);
        UpdateDeathCounter();
    }

    private void Update()
    {
        UpdateDeathCounter();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateDisplay(scene.name);
        UpdateDeathCounter();
    }

    //==================================================
    // UI UPDATE METHODS
    //==================================================
    private void UpdateDisplay(string sceneName)
    {
        bool isMainMenu = sceneName == "MainMenu";
        deathCounterText.gameObject.SetActive(!isMainMenu);
    }

    private void UpdateDeathCounter()
    {
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
