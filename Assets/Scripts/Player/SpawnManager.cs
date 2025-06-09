using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance != null)
        {
            string spawnName = GameManager.nextSpawn;
            GameObject spawnPoint = GameObject.Find(spawnName);

            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
                transform.rotation = spawnPoint.transform.rotation;
            }
            else
            {
                Debug.LogWarning($"Spawn point '{spawnName}' not found!");
            }
        }
    }
}
