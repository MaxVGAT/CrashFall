using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deathCounterText;

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

    private void UpdateDisplay(string sceneName)
    {
        bool isMainMenu = sceneName == "MainMenu";
        deathCounterText.gameObject.SetActive(!isMainMenu);
    }

    private void UpdateDeathCounter()
    {
        deathCounterText.text = "Deaths: " + GameManager.Instance.deathCounter;
    }
}
