// ==================================================
// CAMERA FOLLOW PLAYER
// ==================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    // ==================================================
    // CAMERA SETTINGS: target to follow, speed (unused), and offset
    // ==================================================
    [Header("Camera")]
    [SerializeField] public Transform target;
    [SerializeField] public float cameraSpeed = 1f; // currently unused, could be for smoothing
    [SerializeField] public Vector3 cameraOffset = new Vector3(0f, 0f, -10f);

    // ==================================================
    // LATE UPDATE: position camera at target + offset every frame
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
