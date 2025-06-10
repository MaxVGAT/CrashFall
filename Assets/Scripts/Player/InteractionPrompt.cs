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
        HidePrompt();
        baseScale = prompt.transform.localScale;
    }

    private void Update()
    {
        if (prompt.activeSelf)
        {
            prompt.transform.position = player.position + promptOffset;
            prompt.transform.forward = Camera.main.transform.forward;

            float normalizedSin = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            float scale = Mathf.Lerp(minPulseSize, maxPulseSize, normalizedSin);
            prompt.transform.localScale = baseScale * scale;
        }

    }

    public void ShowPrompt()
    {
        prompt.SetActive(true);
    }

    public void HidePrompt()
    {
        prompt.SetActive(false);
    }

}
