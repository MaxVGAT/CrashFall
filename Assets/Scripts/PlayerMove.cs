using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{

    public Rigidbody2D rb;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    private float horizontal;
    private bool isFacingRight = true;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Collider2D playerCollider;

    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private int maxJump = 2;
    int jumpsRemaining;

    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    bool isOnPlatform;
    bool grounded;

    [Header("Gravity")]
    [SerializeField] private float baseGravity = 2f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] private float fallSpeedMultiplier = 2f;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    [Header("Camera")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        grounded = GroundCheck();
        Gravity();

        if (horizontal > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && isFacingRight)
        {
            Flip();
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator.SetBool("isGrounded", grounded);
        animator.SetFloat("verticalVelocity", rb.velocity.y);
    }

    private void Gravity()
    {
        if(rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {
            if (context.performed)
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

    private bool GroundCheck()
    {
        bool OnGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        bool onPlatform = Physics2D.OverlapCircle(groundCheck.position, 0.2f, platformLayer);

        isOnPlatform = onPlatform;
        bool grounded = OnGround || onPlatform;

        if (grounded)
        {
            jumpsRemaining = maxJump;
        }
        return grounded;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !isFacingRight;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x * moveSpeed;
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.performed && grounded && isOnPlatform && playerCollider.enabled)
        {
            StartCoroutine(DisablePlayerCollider(0.20f));
        }
    }

    private IEnumerator DisablePlayerCollider(float disableTime)
    {
        playerCollider.enabled = false;
        yield return new WaitForSeconds(disableTime);
        playerCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius); // Now shows a circle
    }
}
