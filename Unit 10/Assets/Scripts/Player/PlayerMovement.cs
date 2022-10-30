using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float jumpForce;
    [Range(-1,1)]
    public float testAngle;
    public float maxVelocity;
    public float runSpeed;

    private Rigidbody2D rb;
    private Vector2 inputVelocity, desiredVelocity;
    private float tiltAngle;
    [SerializeField]
    private bool isGyroEnabled = false, jumpPress = false;
    public bool grounded;
    private Vector3 posCur, turnVector;
    private Quaternion rotCur;


    public BezierSpline spline;

	public float duration;

	public bool lookForward;

	public SplineWalkerMode mode;

	private float progress = 1f;
	private bool goingForward = true;


    private void Update()
    {
        TakeInput();
        ProcessAirtimeNormals();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(isGyroEnabled)
            Input.gyro.enabled = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
            SnapToGroundNormal(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
            grounded = false;
    }

    private void SnapToGroundNormal(Collision2D collision)
    {
        grounded = true;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.blue, 2f, false);
            rotCur = Quaternion.FromToRotation(transform.up, contact.normal) * transform.rotation;
            posCur = new Vector3(transform.position.x, contact.point.y + 0.9f, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, posCur, Time.deltaTime * 10);
            //transform.rotation = rotCur;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotCur, Time.deltaTime * 3);
        }
    }



    private void ProcessAirtimeNormals()
    {
        if(!grounded)
        {
            transform.rotation = Quaternion.Euler(Vector3.up);
        }
    }

    private void MovePlayer()
    {
        if(rb.velocity.magnitude < maxVelocity)
            rb.AddForce(inputVelocity * runSpeed);
    }

    private void TakeInput()
    {
        if(!isGyroEnabled)
        {
            //testAngle = Input.GetAxis("Horizontal");
            
            inputVelocity.x = testAngle;
            jumpPress = (Input.GetMouseButtonDown(0)) ? true : false;
        }

        if(isGyroEnabled)
        {
            tiltAngle = -Input.gyro.attitude.z;
            jumpPress = (Input.touchCount > 0) ? true : false;

        }

        if (jumpPress == true)
            Jump();
        
    }

    private void Jump()
    {
        if(grounded)
        {
            float jumpBonus = (inputVelocity.x * inputVelocity.x) / 10 + 1;
            rb.AddForce(Vector2.up * jumpForce * jumpBonus, ForceMode2D.Impulse);
        }
    }
}
