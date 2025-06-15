using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    // ----------------------------------------
    // SINGLETON INSTANCE
    // ----------------------------------------
    public static InteractionPrompt Instance { get; private set; }


    // ----------------------------------------
    // SERIALIZED FIELDS
    // ----------------------------------------
    [SerializeField] private Transform player;
    [SerializeField] private GameObject prompt;
    [SerializeField] private Vector3 promptOffset = new Vector3(0, 1.5f, 0);
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float maxPulseSize = 3f;
    [SerializeField] private float minPulseSize = 1f;


    // ----------------------------------------
    // PRIVATE FIELDS
    // ----------------------------------------
    private Vector3 baseScale;
    private Camera mainCamera;


    // ----------------------------------------
    // UNITY EVENTS
    // ----------------------------------------
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        if (prompt != null)
        {
            baseScale = prompt.transform.localScale;
            prompt.SetActive(false);
        }

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (prompt == null || !prompt.activeSelf || player == null || mainCamera == null)
            return;

        prompt.transform.position = player.position + promptOffset;
        prompt.transform.forward = mainCamera.transform.forward;

        float normalizedSin = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float scale = Mathf.Lerp(minPulseSize, maxPulseSize, normalizedSin);
        prompt.transform.localScale = baseScale * scale;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }


    // ----------------------------------------
    // PUBLIC METHODS
    // ----------------------------------------
    public void ShowPrompt()
    {
        if (prompt != null && !prompt.activeSelf)
        {
            prompt.SetActive(true);
        }
    }

    public void HidePrompt()
    {
        if (prompt != null && prompt.activeSelf)
        {
            prompt.SetActive(false);
        }
    }
}
