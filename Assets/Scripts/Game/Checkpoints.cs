using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public static Checkpoints Instance { get; private set; }

    public enum CheckpointType { City_CP, Forest_CP, Castle_CP }

    [Header("Checkpoints")]
    [SerializeField] private CheckpointType CPType;
    [SerializeField] private GameObject City_CP;
    //[SerializeField] private GameObject Forest_CP;
    //[SerializeField] private GameObject Castle_CP;

    [Header("Textures")]
    [SerializeField] private GameObject City_OFF;
    [SerializeField] private GameObject City_ON;
    //[SerializeField] private GameObject Forest_OFF;
    //[SerializeField] private GameObject Forest_ON;
    //[SerializeField] private GameObject Castle_OFF;
    //[SerializeField] private GameObject Castle_ON;

    [Header("Conditions")]
    [SerializeField] public bool isCityON = false;
    //[SerializeField] private bool isForestON = false;
    //[SerializeField] private bool isCastleON = false;

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

        City_ON.SetActive(false);
        City_OFF.SetActive(true);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        string currentCP = CPType.ToString();

        switch(currentCP)
        {
            case "City_CP": 
                if (!isCityON)
                {
                    isCityON = true;
                    City_OFF.SetActive(false);
                    City_ON.SetActive(true);
                }
                break;

            //case "Forest_CP":
            //    if (!isForestON)
            //    {
            //        isForestON = true;
            //        Forest_OFF.SetActive(false);
            //        Forest_ON.SetActive(true);
            //    }
            //    break;

            //case "Castle_CP":
            //    if (!isCastleON)
            //    {
            //        isCastleON = true;
            //        Castle_OFF.SetActive(false);
            //        Castle_ON.SetActive(true);
            //    }
            //    break;
        }
    }

}
