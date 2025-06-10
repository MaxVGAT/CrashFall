using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    [Header("Camera")]
    [SerializeField] public Transform target;
    [SerializeField] public float cameraSpeed = 1f;
    [SerializeField] public Vector3 cameraOffset = new Vector3(0f, 0f, -10f);

    public void LateUpdate()
    {
        Vector3 targetPosition = target.position + cameraOffset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
    }
}
