using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGyroEnabled = false;
    private Vector2 inputVelocity, desiredVelocity;

    private float tiltAngle;

    [Range(-1,1)]
    public float testAngle;

    private void FixedUpdate()
    {
        TakeInput();
        MovePlayer();
        if(isGyroEnabled)
            tiltAngle = -Input.gyro.attitude.z;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(isGyroEnabled)
            Input.gyro.enabled = true;
    }
    private void MovePlayer()
    {
        rb.AddForce(inputVelocity * 10);
    }

    private void TakeInput()
    {
        if(!isGyroEnabled)
        {
            inputVelocity.x = testAngle;
        }
    }
}
