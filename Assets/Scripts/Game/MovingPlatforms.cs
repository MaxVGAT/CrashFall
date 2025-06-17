using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatforms : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform movingBox;
    [SerializeField] private Vector3 minX;
    [SerializeField] private Vector3 maxX;
    [SerializeField] private Vector3 minY;
    [SerializeField] private Vector3 maxY;
    [SerializeField] private float movingSpeed;
    [SerializeField] private bool isXMoving = false;
    [SerializeField] private bool isYMoving = false;

    private float distanceX;
    private float distanceY;


    

    private void Start()
    {
        minX = movingBox.position;
        maxX = minX + new Vector3(5f, 0f, 0f);
        distanceX = maxX.x - minX.x;

        minY = movingBox.position;
        maxY = minY + new Vector3(0f, 0f, 0f);
        distanceY = maxY.y - minY.y;
    }
    private void Update()
    {
        if (isXMoving && !isYMoving)
        {
            MoveX();
        }
        else if(isYMoving && !isXMoving)
        {
                MoveY();
        }
    }

    private void MoveX()
    {
        isXMoving = true;
        float newXPos = Mathf.PingPong(Time.time * movingSpeed, 5f) + minX.x;
        movingBox.position = new Vector3(newXPos, movingBox.position.y, movingBox.position.z);
    }

    private void MoveY()
    {
        isYMoving = true;
        float newYPos = maxY.y - Mathf.PingPong(Time.time * movingSpeed, 3f);
        movingBox.position = new Vector3(movingBox.position.x, newYPos, movingBox.position.z);
    }

}
