using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    public static InteractionPrompt Instance;

    public Transform player;
    public GameObject prompt;
    public Vector3 promptOffset = new Vector3(0, 1.5f, 0);
    public float pulseSpeed = 2f;
    public float maxPulseSize = 3f;
    public float minPulseSize = 1f;

    private Vector3 baseScale;

    void Awake()
    {
        Instance = this;
        baseScale = prompt.transform.localScale;
    }

    private void Update()
    {
        if (prompt.activeSelf)
        {
            Debug.Log("Update running - prompt is active"); 
            prompt.transform.position = player.position + promptOffset;
            prompt.transform.forward = Camera.main.transform.forward;

            float normalizedSin = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            float scale = Mathf.Lerp(minPulseSize, maxPulseSize, normalizedSin);
            prompt.transform.localScale = baseScale * scale;
        }

    }

    public void ShowPrompt()
    {
        Debug.Log("ShowPrompt called");
        Debug.Log("Prompt object: " + (prompt != null));
        Debug.Log("Prompt active before: " + prompt.activeSelf);

        prompt.SetActive(true);

        Debug.Log("Prompt active after: " + prompt.activeSelf);
        Debug.Log("Prompt position: " + prompt.transform.position);
        Debug.Log("Player position: " + player.position);
    }

    public void HidePrompt()
    {
        Debug.Log("HidePrompt called from: " + System.Environment.StackTrace);
        prompt.SetActive(false);
    }

}
