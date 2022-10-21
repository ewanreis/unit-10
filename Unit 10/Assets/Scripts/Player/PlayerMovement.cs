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
        grounded = true;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.blue, 2f, false);
            rotCur = Quaternion.FromToRotation(transform.up, contact.normal) * transform.rotation;
            posCur = new Vector3(transform.position.x, contact.point.y + .3f, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, posCur, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotCur, Time.deltaTime * 5);
        }
        //if (collision.relativeVelocity.magnitude > 2)
        //    audioSource.Play();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        grounded = false;
    }

    private void ProcessAirtimeNormals()
    {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        //Debug.DrawRay(hit.point, contact.normal, Color.blue, 2f, false);

        if(!grounded)
        {
            /*// * if we are not grounded, make the object go straight down in world space (simulating gravity). the "1f" multiplier controls how fast we descend.
            transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up * 1f, Time.deltaTime * 5);
            // ! from memory, not sure why I added this... Looks like a fail safe to me. When the object is turned too much towards teh front or back, almost instantly (*1000) make it rotate to a better orientation for aligning.
            if(transform.eulerAngles.x > 15)
            {
                turnVector.x -= Time.deltaTime * 1000;
            }
            else if(transform.eulerAngles.x < 15)
            {
                turnVector.x += Time.deltaTime * 1000;
            }
            //if we are not grounded, make the vehicle's rotation "even out". Not completey reaslistic, but easy to work with.
            rotCur.eulerAngles = Vector3.zero;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotCur, Time.deltaTime);*/
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
            testAngle = Input.GetAxis("Horizontal");
            
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
        float jumpBonus = (inputVelocity.x * inputVelocity.x) / 10 + 1;
        rb.AddForce(Vector2.up * jumpForce * jumpBonus, ForceMode2D.Impulse);
    }
}
