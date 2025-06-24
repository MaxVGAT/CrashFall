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

        // Debug serialized field assignments
        Debug.Log($"[InteractionPrompt] Player reference: {(player != null ? player.name : "NULL")}");
        Debug.Log($"[InteractionPrompt] Prompt reference: {(prompt != null ? prompt.name : "NULL")}");

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

        // Re-check references in Start (some objects might not be available in Awake)
        if (player == null)
        {
            Debug.LogWarning("[InteractionPrompt] Player reference is still NULL in Start()");
            // Try to find player
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

        if (prompt == null)
        {
            Debug.LogWarning("[InteractionPrompt] Prompt is still NULL in Start(). Trying to find it...");

            // Try multiple methods to find the prompt
            GameObject foundPrompt = null;

            // Method 1: Find by name
            foundPrompt = GameObject.Find("InteractionPrompt");
            if (foundPrompt != null)
            {
                Debug.Log($"[InteractionPrompt] Found prompt by name: {foundPrompt.name}");
            }

            // Method 2: Find by tag (if you set up a tag)
            if (foundPrompt == null)
            {
                try
                {
                    foundPrompt = GameObject.FindWithTag("InteractionPrompt");
                    if (foundPrompt != null)
                    {
                        Debug.Log($"[InteractionPrompt] Found prompt by tag: {foundPrompt.name}");
                    }
                }
                catch
                {
                    Debug.Log("[InteractionPrompt] InteractionPrompt tag doesn't exist");
                }
            }

            // Method 3: Look in UI Canvas
            if (foundPrompt == null)
            {
                Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
                foreach (Canvas canvas in canvases)
                {
                    // Try common prompt names
                    string[] promptNames = { "InteractionPrompt", "Prompt", "UIPrompt", "E_Prompt", "InteractionUI" };

                    foreach (string promptName in promptNames)
                    {
                        Transform promptTransform = canvas.transform.Find(promptName);
                        if (promptTransform != null)
                        {
                            foundPrompt = promptTransform.gameObject;
                            Debug.Log($"[InteractionPrompt] Found prompt in canvas {canvas.name}: {foundPrompt.name}");
                            break;
                        }
                    }

                    if (foundPrompt != null) break;
                }
            }

            // Method 4: Look for any GameObject with "prompt" in the name (case insensitive)
            if (foundPrompt == null)
            {
                GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (GameObject obj in allObjects)
                {
                    if (obj.name.ToLower().Contains("prompt"))
                    {
                        foundPrompt = obj;
                        Debug.Log($"[InteractionPrompt] Found object with 'prompt' in name: {foundPrompt.name}");
                        break;
                    }
                }
            }

            if (foundPrompt != null)
            {
                prompt = foundPrompt;
                baseScale = prompt.transform.localScale;
                prompt.SetActive(false);
                Debug.Log($"[InteractionPrompt] Prompt successfully found and assigned: {prompt.name}");
            }
            else
            {
                Debug.LogError("[InteractionPrompt] Could not find prompt GameObject in scene! Make sure there's a GameObject named 'InteractionPrompt' or assign it manually in the inspector.");
            }
        }

        if (mainCamera == null)
        {
            Debug.LogWarning("[InteractionPrompt] Main camera is NULL in Start()");
            Camera[] cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
            if (cameras.Length > 0)
            {
                mainCamera = cameras[0];
                Debug.Log($"[InteractionPrompt] Camera found via FindObjectsByType: {mainCamera.name}");
            }
            else
            {
                Debug.LogError("[InteractionPrompt] No cameras found in scene!");
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

        prompt.transform.position = player.position + promptOffset;
        prompt.transform.forward = mainCamera.transform.forward;
        float normalizedSin = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float scale = Mathf.Lerp(minPulseSize, maxPulseSize, normalizedSin);
        prompt.transform.localScale = Vector3.one * scale;
    }

    private void OnDestroy()
    {
        Debug.Log($"[InteractionPrompt] OnDestroy() called on: {gameObject.name}");
        if (Instance == this)
        {
            Instance = null;
            Debug.Log("[InteractionPrompt] Singleton instance cleared");
        }
    }

    // ----------------------------------------
    // PUBLIC METHODS
    // ----------------------------------------
    public void ShowPrompt()
    {
        Debug.Log("[InteractionPrompt] ShowPrompt() called");

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
        Debug.Log("[InteractionPrompt] HidePrompt() called");

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

    // ----------------------------------------
    // DEBUG METHODS (Remove in production)
    // ----------------------------------------
    public void DebugCurrentState()
    {
        Debug.Log("=== INTERACTION PROMPT DEBUG STATE ===");
        Debug.Log($"Instance exists: {Instance != null}");
        Debug.Log($"Player reference: {(player != null ? player.name : "NULL")}");
        Debug.Log($"Prompt reference: {(prompt != null ? prompt.name : "NULL")}");
        Debug.Log($"Prompt active: {(prompt != null ? prompt.activeSelf.ToString() : "N/A")}");
        Debug.Log($"Main camera: {(mainCamera != null ? mainCamera.name : "NULL")}");
        Debug.Log("=====================================");
    }
}
// ==================================================
// END OF INTERACTION PROMPT
// ==================================================