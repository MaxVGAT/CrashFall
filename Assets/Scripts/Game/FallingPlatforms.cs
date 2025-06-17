using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject crumblingBlock;
    [SerializeField] private float crumblingSpeed = 1f;
    [SerializeField] private float crumblingTimer = 0f;
    [SerializeField] private float respawnTimer = 5f;

    private bool isCrumbling = false;
    private bool hasCrumbled = false;
    private BoxCollider2D blockCollider;


    private float colorLerpProgress = 0f;

    private SpriteRenderer blockRenderer;
    private Color originalColor;
    private Color crumblingColor = Color.red;

    private void Start()
    {
        blockRenderer = crumblingBlock.GetComponent<SpriteRenderer>();
        blockCollider = crumblingBlock.GetComponent<BoxCollider2D>();
        blockRenderer = crumblingBlock.GetComponent<SpriteRenderer>();
        originalColor = blockRenderer.color;
    }

    private void Update()
    {
        if(isCrumbling)
        {
            crumblingTimer -= Time.deltaTime;
            colorLerpProgress += Time.deltaTime * crumblingSpeed;

            blockRenderer.color = Color.Lerp(originalColor, crumblingColor, colorLerpProgress);

            if (crumblingTimer <= 0f && crumblingBlock.activeSelf)
            {
                crumblingBlock.GetComponent<SpriteRenderer>().enabled = false;
                crumblingBlock.GetComponent<BoxCollider2D>().enabled = false;
                hasCrumbled = true;
                isCrumbling = false;
                StartCoroutine(ResetBlock());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isCrumbling && !hasCrumbled)
        {
            isCrumbling = true;
            crumblingTimer = 1f;
            colorLerpProgress = 0f;
        }
    }

    IEnumerator ResetBlock()
    {
        yield return new WaitForSeconds(respawnTimer);

        blockRenderer.color = originalColor;
        crumblingBlock.GetComponent<SpriteRenderer>().enabled = true;
        crumblingBlock.GetComponent<BoxCollider2D>().enabled = true;
        hasCrumbled = false;
    }
}
