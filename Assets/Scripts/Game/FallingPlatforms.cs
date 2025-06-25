using System.Collections;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    // ============================
    // ======== SETTINGS ==========
    // ============================
    [Header("Settings")]
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject crumblingBlock;  // The platform that will crumble
    [SerializeField] private float crumblingSpeed = 1f;   // How fast the crumbling effect occurs
    [SerializeField] private float respawnTimer = 5f;    // Time before platform reappears

    // ============================
    // ======== STATE =============
    // ============================
    private bool isCrumbling = false;
    private bool hasCrumbled = false;

    private float crumblingTimer = 0f;
    private float colorLerpProgress = 0f;

    private SpriteRenderer blockRenderer;
    private BoxCollider2D blockCollider;

    private Color originalColor;
    private readonly Color crumblingColor = Color.red;  // Visual indicator during crumbling

    // ============================
    // ========= START ============
    // ============================
    private void Start()
    {
        // Validate required components
        if (crumblingBlock == null)
        {
            Debug.LogError("[FallingPlatforms] crumblingBlock reference missing!");
            enabled = false;
            return;
        }

        blockRenderer = crumblingBlock.GetComponent<SpriteRenderer>();
        blockCollider = crumblingBlock.GetComponent<BoxCollider2D>();

        if (blockRenderer == null || blockCollider == null)
        {
            Debug.LogError("[FallingPlatforms] Missing SpriteRenderer or BoxCollider2D on crumblingBlock!");
            enabled = false;
            return;
        }

        originalColor = blockRenderer.color;
        crumblingTimer = 1f / crumblingSpeed;
    }

    // ============================
    // ========= UPDATE ===========
    // ============================
    private void Update()
    {
        if (!isCrumbling) return;

        // Handle crumbling visual effect and timing
        crumblingTimer -= Time.deltaTime;
        colorLerpProgress += Time.deltaTime * crumblingSpeed;

        blockRenderer.color = Color.Lerp(originalColor, crumblingColor, colorLerpProgress);

        if (crumblingTimer <= 0f && crumblingBlock.activeSelf)
        {
            // Disable platform when crumbling completes
            blockRenderer.enabled = false;
            blockCollider.enabled = false;

            hasCrumbled = true;
            isCrumbling = false;

            StartCoroutine(ResetBlock());
        }
    }

    // ============================
    // ======== COLLISIONS ========
    // ============================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Start crumbling when player lands on platform
        if (collision.gameObject.CompareTag("Player") && !isCrumbling && !hasCrumbled)
        {
            isCrumbling = true;
            colorLerpProgress = 0f;
            crumblingTimer = 1f / crumblingSpeed;
        }
    }

    // ============================
    // ======= COROUTINES =========
    // ============================
    private IEnumerator ResetBlock()
    {
        yield return new WaitForSeconds(respawnTimer);

        // Reset platform to original state
        blockRenderer.color = originalColor;
        blockRenderer.enabled = true;
        blockCollider.enabled = true;

        crumblingTimer = 1f / crumblingSpeed;
        colorLerpProgress = 0f;
        hasCrumbled = false;
    }
}