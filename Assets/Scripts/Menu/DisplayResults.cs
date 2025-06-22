using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayResults : MonoBehaviour
{
    GameManager gameManager;

    int deaths = GameManager.Instance.deathCounter;
    string timer = GameManager.Instance.GetFormattedTime();

    private bool seeResult = false;

    private void Update()
    {
        if(seeResult && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.ShowResults(deaths, timer);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        seeResult = true;

        GameManager.Instance.ShowResults(deaths, timer);
    }
}
