using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    // ============================
    // ======== TARGET ============
    // ============================
    [Header("Target")]
    [SerializeField] private Transform movingBox;  // Platform to be moved

    [Header("Movement Boundaries")]
    [SerializeField] private Vector3 minX;  // Minimum X position
    [SerializeField] private Vector3 maxX;  // Maximum X position
    [SerializeField] private Vector3 minY;  // Minimum Y position
    [SerializeField] private Vector3 maxY;  // Maximum Y position

    [Header("Movement Settings")]
    [SerializeField] private float movingSpeed;  // Speed of platform movement
    [SerializeField] private bool isXMoving = false;  // Enable X-axis movement
    [SerializeField] private bool isYMoving = false;  // Enable Y-axis movement

    [Header("Ping Pong Settings")]
    [SerializeField] private float pingPongLengthX = 0f;  // X-axis movement range
    [SerializeField] private float pingPongLengthY = 0f;  // Y-axis movement range

    // ============================
    // ======== INTERNAL ==========
    // ============================
    private float distanceX;  // Calculated X distance
    private float distanceY;  // Calculated Y distance

    // ============================
    // ======= UNITY EVENTS =======
    // ============================
    private void Start()
    {
        if (movingBox == null)
        {
            Debug.LogWarning("[MovingPlatforms] movingBox is not assigned!");
            return;
        }

        // Initialize movement boundaries
        minX = movingBox.position;
        maxX = minX + new Vector3(5f, 0f, 0f);
        distanceX = maxX.x - minX.x;

        minY = movingBox.position;
        maxY = minY + new Vector3(0f, 3f, 0f);
        distanceY = maxY.y - minY.y;
    }

    private void FixedUpdate()
    {
        if (movingBox == null) return;

        // Handle platform movement based on enabled axes
        if (isXMoving && !isYMoving)
        {
            MoveX();
        }
        else if (isYMoving && !isXMoving)
        {
            MoveY();
        }
    }

    // ============================
    // ====== MOVEMENT LOGIC ======
    // ============================
    private void MoveX()
    {
        // PingPong movement along X-axis
        float newXPos = Mathf.PingPong(Time.time * movingSpeed, pingPongLengthX) + minX.x;
        movingBox.position = new Vector3(newXPos, movingBox.position.y, movingBox.position.z);
    }

    private void MoveY()
    {
        // PingPong movement along Y-axis
        float newYPos = minY.y - Mathf.PingPong(Time.time * movingSpeed, pingPongLengthY);
        movingBox.position = new Vector3(movingBox.position.x, newYPos, movingBox.position.z);
    }
}