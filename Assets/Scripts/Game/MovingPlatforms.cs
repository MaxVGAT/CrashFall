using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    // ============================
    // ======== TARGET ============
    // ============================
    [Header("Target")]
    [SerializeField] private Transform movingBox;

    [SerializeField] private Vector3 minX;
    [SerializeField] private Vector3 maxX;

    [SerializeField] private Vector3 minY;
    [SerializeField] private Vector3 maxY;

    [SerializeField] private float movingSpeed;

    [SerializeField] private bool isXMoving = false;
    [SerializeField] private bool isYMoving = false;

    [SerializeField] private float pingPongLengthX = 0f;
    [SerializeField] private float pingPongLengthY = 0f;

    // ============================
    // ======== INTERNAL ==========
    // ============================
    private float distanceX;
    private float distanceY;

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
        // You don't need to reassign isXMoving every frame
        float newXPos = Mathf.PingPong(Time.time * movingSpeed, pingPongLengthX) + minX.x;
        movingBox.position = new Vector3(newXPos, movingBox.position.y, movingBox.position.z);
    }

    private void MoveY()
    {
        // You don't need to reassign isYMoving every frame
        float newYPos = minY.y - Mathf.PingPong(Time.time * movingSpeed, pingPongLengthY);
        movingBox.position = new Vector3(movingBox.position.x, newYPos, movingBox.position.z);
    }
}
