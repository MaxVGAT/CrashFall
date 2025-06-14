using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using static UnityEditor.Timeline.TimelinePlaybackControls;
#endif

public class PlayerMove : MonoBehaviour
{
    //==================================================
    // REFERENCES
    //==================================================
    public Rigidbody2D rb;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    private float horizontal;
    private bool isFacingRight = true;

    [Header("Player")]
    [SerializeField] public Transform player;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Collider2D playerCollider;

    [Header("Camera")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    [Header("Sound")]
    [SerializeField] private AudioSource walkAudioSource;
    [SerializeField] private AudioClip walkSFX;
    private Coroutine fadeCoroutine;

    //==================================================
    // SMOKE
    //==================================================
    [Header("Run Smoke Effect")]
    [SerializeField] private GameObject runSmokeTexture;
    [SerializeField] private Transform smokeSpawnPoint;
    [SerializeField] private float smokeSpawnCooldown = 0.15f;
    private float lastSmokeTime;

    //==================================================
    // GRAVITY
    //==================================================
    [Header("Gravity")]
    [SerializeField] private float baseGravity = 2f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] private float fallSpeedMultiplier = 2f;

    //==================================================
    // JUMP
    //==================================================
    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 15f;
    int jumpsRemaining;
    private int maxJump;

    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 boxSize = new Vector2(0.8f, 0.4f);
    bool isOnPlatform;
    bool grounded;

    //==================================================
    // DASH
    //==================================================
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 1f;
    [SerializeField] private float dashDuration = 0.1f;

    private bool canDash = false;
    private bool isDashing = false;

    //==================================================
    // POWER-UPS
    //==================================================
    [Header("Power-Ups")]
    [SerializeField] public bool canDoubleJump = false;
    [SerializeField] public bool hasUnlockedDash = false;

    //==================================================
    // GENERAL
    //==================================================
    private float deathCounter;

    private void Start()
    {
        if (!string.IsNullOrEmpty(GameManager.nextSpawn))
        {
            GameObject spawn = GameObject.Find(GameManager.nextSpawn);
            if (spawn != null)
            {
                transform.position = spawn.transform.position;
            }
            else
            {
                Debug.LogWarning("Spawn point not found: " + GameManager.nextSpawn);
            }
        }

        playerCollider = GetComponent<Collider2D>();
        walkAudioSource.clip = walkSFX;

        maxJump = canDoubleJump ? 2 : 1;
    }

    private void Update()
    {
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        grounded = GroundCheck();
        Gravity();

        bool isRunning = animator.GetCurrentAnimatorStateInfo(0).IsName("player_run") && grounded && Mathf.Abs(horizontal) > 0.1f;

        if (Input.GetKeyDown(KeyCode.LeftControl) && canDash && !isDashing && Mathf.Abs(horizontal) > 0.01f)
        {
            UseDash();
        }

        if (isRunning)
        {
            SpawnSmoke();

            if (!walkAudioSource.isPlaying)
            {
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                    fadeCoroutine = null;
                }
                walkAudioSource.volume = 0.05f;
                walkAudioSource.loop = true;
                walkAudioSource.Play();
            }
        }
        else
        {
            if (walkAudioSource.isPlaying && fadeCoroutine == null)
            {
                fadeCoroutine = StartCoroutine(FadeOut(walkAudioSource, 0.35f));
            }
        }

        if (horizontal > 0 && !isFacingRight)
            Flip();
        else if (horizontal < 0 && isFacingRight)
            Flip();

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator.SetBool("isGrounded", grounded);
        animator.SetFloat("verticalVelocity", rb.velocity.y);
    }

    //==================================================
    // INPUT HANDLERS
    //==================================================

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x * moveSpeed;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                jumpsRemaining = canDoubleJump ? 1 : 0;
            }
            else if (jumpsRemaining > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                jumpsRemaining--;
            }

            if (context.canceled && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.performed && grounded && isOnPlatform)
        {
            StartCoroutine(TemporarilyIgnorePlatforms(0.20f));
        }
    }

    //==================================================
    // POWER-UP UNLOCKS
    //==================================================

    public void UnlockDoubleJump()
    {
        canDoubleJump = true;
        if (grounded)
        {
            jumpsRemaining = 1;
        }
    }

    public void UnlockDash()
    {
        hasUnlockedDash = true;
    }

    //==================================================
    // COLLISION
    //==================================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = true;
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            player.transform.position = new Vector2(3f, -3.3f);
            deathCounter++;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = false;
        }
    }

    //==================================================
    // HELPERS
    //==================================================

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !isFacingRight;
    }

    private void Gravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private bool GroundCheck()
    {
        bool OnGround = Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, groundLayer);
        bool onPlatform = Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, platformLayer);

        isOnPlatform = onPlatform;
        bool grounded = OnGround || onPlatform;

        if (grounded)
        {
            jumpsRemaining = canDoubleJump ? 1 : 0;
        }

        if (!grounded && !isDashing)
        {
            canDash = true;
        }
        return grounded;
    }

    private void SpawnSmoke()
    {
        if (Time.time - lastSmokeTime > smokeSpawnCooldown)
        {
            Vector3 spawnPos = smokeSpawnPoint.position;

            if (isFacingRight)
            {
                spawnPos.x = transform.position.x + Mathf.Abs(smokeSpawnPoint.localPosition.x);
            }
            else
            {
                spawnPos.x = transform.position.x - Mathf.Abs(smokeSpawnPoint.localPosition.x);
            }

            GameObject smoke = Instantiate(runSmokeTexture, smokeSpawnPoint.position, Quaternion.identity);
            Destroy(smoke, 0.25f);
            lastSmokeTime = Time.time;
        }
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;

        while (source.volume > 0f)
        {
            source.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
        fadeCoroutine = null;
    }

    private IEnumerator TemporarilyIgnorePlatforms(float duration)
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = platformLayer;
        contactFilter.useLayerMask = true;

        List<Collider2D> results = new List<Collider2D>();
        Physics2D.OverlapCollider(playerCollider, contactFilter, results);

        foreach (var platform in results)
        {
            Physics2D.IgnoreCollision(playerCollider, platform, true);
        }

        yield return new WaitForSeconds(duration);

        foreach (var platform in results)
        {
            Physics2D.IgnoreCollision(playerCollider, platform, false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, boxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(smokeSpawnPoint.position, Vector3.one * 0.1f);
    }

    //==================================================
    // DASH LOGIC
    //==================================================

    private void UseDash()
    {
        if (hasUnlockedDash)
        {
            float direction = isFacingRight ? 1f : -1f;
            StartCoroutine(DashCoroutine(direction));
        }
    }

    private IEnumerator DashCoroutine(float direction)
    {
        isDashing = true;
        animator.SetBool("isDashing", true);

        canDash = false;

        float dashTime = 0f;

        while (dashTime < dashDuration)
        {
            rb.velocity = new Vector2(direction * dashSpeed, rb.velocity.y);
            dashTime += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        animator.SetBool("isDashing", false);
    }
}
