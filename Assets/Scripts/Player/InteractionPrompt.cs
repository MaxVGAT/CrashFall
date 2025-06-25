// ==================================================
// INTERACTION PROMPT WITH DEBUG LOGGING
// ==================================================
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
    [SerializeField] private float maxPulseSize = 1.2f;
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
        Debug.Log($"[InteractionPrompt] Awake called on: {gameObject.name}");

        if (Instance != null && Instance != this)
        {
            Debug.Log($"[InteractionPrompt] Duplicate instance detected. Destroying: {gameObject.name}");
            Destroy(this);
            return;
        }

        Instance = this;
        Debug.Log($"[InteractionPrompt] Singleton instance set on: {gameObject.name}");

        // Cache base scale and hide prompt if assigned
        if (prompt != null)
        {
            baseScale = prompt.transform.localScale;
            prompt.SetActive(false);
            Debug.Log($"[InteractionPrompt] Prompt initialized and hidden. Base scale: {baseScale}");
        }
        else
        {
            Debug.LogWarning("[InteractionPrompt] Prompt is NULL! Will try to find it in Start().");
        }

        mainCamera = Camera.main;
        Debug.Log($"[InteractionPrompt] Main camera found: {(mainCamera != null ? mainCamera.name : "NULL")}");
    }

    private void Start()
    {
        Debug.Log("[InteractionPrompt] Start() called");

        // Re-check player reference, try to find it if missing
        if (player == null)
        {
            Debug.LogWarning("[InteractionPrompt] Player reference is still NULL in Start()");
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log($"[InteractionPrompt] Player found via tag: {player.name}");
            }
            else
            {
                Debug.LogError("[InteractionPrompt] Could not find player with 'Player' tag");
            }
        }

        // Re-check prompt reference, attempt various find methods if missing
        if (prompt == null)
        {
            Debug.LogWarning("[InteractionPrompt] Prompt is still NULL in Start(). Trying to find it...");

            GameObject foundPrompt = null;

            // Try find by name
            foundPrompt = GameObject.Find("InteractionPrompt");
            if (foundPrompt != null)
                Debug.Log($"[InteractionPrompt] Found prompt by name: {foundPrompt.name}");

            // Try find by tag if not found by name
            if (foundPrompt == null)
            {
                try
                {
                    foundPrompt = GameObject.FindWithTag("InteractionPrompt");
                    if (foundPrompt != null)
                        Debug.Log($"[InteractionPrompt] Found prompt by tag: {foundPrompt.name}");
                }
                catch
                {
                    Debug.Log("[InteractionPrompt] InteractionPrompt tag doesn't exist");
                }
            }

            // Try searching in canvases if still not found
            if (foundPrompt == null)
            {
                Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
                foreach (Canvas canvas in canvases)
                {
                    string[] promptNames = { "InteractionPrompt", "Prompt", "UIPrompt", "E_Prompt", "InteractionUI" };

                    foreach (string promptName in promptNames)
                    {
                        Transform promptTransform = canvas.transform.Find(promptName);
                        if (promptTransform != null)
                        {
                            foundPrompt = promptTransform.gameObject;
                            break;
                        }
                    }

                    if (foundPrompt != null) break;
                }
            }

            // Fallback: find any object with 'prompt' in name (case insensitive)
            if (foundPrompt == null)
            {
                GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (GameObject obj in allObjects)
                {
                    if (obj.name.ToLower().Contains("prompt"))
                    {
                        foundPrompt = obj;
                        break;
                    }
                }
            }

            if (foundPrompt != null)
            {
                prompt = foundPrompt;
                baseScale = prompt.transform.localScale;
                prompt.SetActive(false);
            }
            else
            {
            }
        }

        // Re-check main camera reference if missing
        if (mainCamera == null)
        {
            Camera[] cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
            if (cameras.Length > 0)
            {
                mainCamera = cameras[0];
            }
            else
            {
            }
        }
    }

    private void Update()
    {
        if (prompt == null)
        {
            Debug.LogError("[InteractionPrompt] Update() - prompt is NULL!");
            return;
        }

        if (!prompt.activeSelf)
            return;

        if (player == null)
        {
            Debug.LogError("[InteractionPrompt] Update() - player is NULL!");
            return;
        }

        if (mainCamera == null)
        {
            Debug.LogError("[InteractionPrompt] Update() - mainCamera is NULL!");
            return;
        }

        // Position prompt above player and face the camera
        prompt.transform.position = player.position + promptOffset;
        prompt.transform.forward = mainCamera.transform.forward;

        // Pulse scale effect
        float normalizedSin = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float scale = Mathf.Lerp(minPulseSize, maxPulseSize, normalizedSin);
        prompt.transform.localScale = Vector3.one * scale;
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

        if (prompt == null)
        {
            Debug.LogError("[InteractionPrompt] Cannot show prompt. prompt is NULL.");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("[InteractionPrompt] Cannot show prompt. Player reference is NULL.");
            return;
        }

        if (!prompt.activeSelf)
        {
            prompt.SetActive(true);
            Debug.Log($"[InteractionPrompt] Prompt shown: {prompt.name}");
        }
        else
        {
            Debug.Log("[InteractionPrompt] Prompt is already active");
        }
    }

    public void HidePrompt()
    {

        if (prompt == null)
        {
            Debug.LogError("[InteractionPrompt] Cannot hide prompt. prompt is NULL.");
            return;
        }

        if (prompt.activeSelf)
        {
            prompt.SetActive(false);
            Debug.Log($"[InteractionPrompt] Prompt hidden: {prompt.name}");
        }
        else
        {
            Debug.Log("[InteractionPrompt] Prompt is already inactive");
        }
    }
}
// ==================================================
// END OF INTERACTION PROMPT
// ==================================================
