using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    [Header("Camera")]
    [SerializeField] private Transform target;
    [SerializeField] private float cameraSpeed = 1f;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 0f, -10f);


    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + cameraOffset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
    }
}
