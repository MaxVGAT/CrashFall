// ==================================================
// CAMERA FOLLOW PLAYER
// ==================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    // ==================================================
    // CAMERA SETTINGS
    // ==================================================
    [Header("Camera")]
    [SerializeField] public Transform target;
    [SerializeField] public float cameraSpeed = 1f;
    [SerializeField] public Vector3 cameraOffset = new Vector3(0f, 0f, -10f);

    // ==================================================
    // LATE UPDATE: FOLLOW TARGET WITH OFFSET
    // ==================================================
    public void LateUpdate()
    {
        Vector3 targetPosition = target.position + cameraOffset;

        transform.position = targetPosition;
    }
}

// ==================================================
// END OF CAMERA FOLLOW PLAYER
// ==================================================
