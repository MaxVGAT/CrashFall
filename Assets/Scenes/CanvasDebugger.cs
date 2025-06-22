using UnityEngine;

public class CanvasRecreator : MonoBehaviour
{
    [SerializeField] private GameObject canvasPrefab; // Drag your canvas prefab here

    void Update()
    {
        // If ResultCanvas is missing, recreate it
        if (GameObject.Find("ResultCanvas") == null)
        {
            Debug.Log("ResultCanvas destroyed - recreating it");

            if (canvasPrefab != null)
            {
                GameObject newCanvas = Instantiate(canvasPrefab);
                newCanvas.name = "ResultCanvas"; // Remove the (Clone) part
            }
            else
            {
                Debug.LogError("Canvas prefab not assigned!");
            }
        }
    }
}