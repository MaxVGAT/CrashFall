using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayResults : MonoBehaviour
{
    private GameManager gameManager;

    private int deaths;
    private string timer;

    private bool seeResult = false;

    //==================================================
    // UNITY EVENTS
    //==================================================
    private void Start()
    {
        // Cache GameManager reference safely
        gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            deaths = gameManager.deathCounter;
            timer = gameManager.GetFormattedTime();
        }
        else
        {
            Debug.LogWarning("[DisplayResults] GameManager.Instance is null!");
            deaths = 0;
            timer = "00:00";
        }
    }

    private void Update()
    {
        if (seeResult && Input.GetKeyDown(KeyCode.E))
        {
            if (gameManager != null)
            {
                gameManager.ShowResults(deaths, timer);
            }
            else
            {
                Debug.LogWarning("[DisplayResults] Cannot show results, GameManager.Instance is null!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        seeResult = true;
        InteractionPrompt.Instance.ShowPrompt();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        seeResult = false;
        InteractionPrompt.Instance.HidePrompt();
    }
}
