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

    public SpriteRenderer sr;

    private Rigidbody2D rb;
    private Vector2 inputVelocity, desiredVelocity;
    private float tiltAngle;
    [SerializeField]
    private bool isGyroEnabled = false, jumpPress = false;
    public bool grounded;
    private Vector3 posCur, turnVector;
    private Quaternion rotCur;

    public bool isJumping, isRunning;

    public Animator anim;


	public bool lookForward = true;



    private void Update()
    {
        TakeInput();
        ProcessAirtimeNormals();
    }

    private void FixedUpdate()
    {
        if(grounded)
            MovePlayer();
        ProcessAnimations();
        
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(isGyroEnabled)
            Input.gyro.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
            StartCoroutine(LandPlayer());
    }

    private IEnumerator LandPlayer()
    {
        yield return new WaitForSeconds(0.1f);

        isJumping = false;
        yield return null;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
            SnapToGroundNormal(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }

    private void ProcessAnimations()
    {
        sr.flipX = !lookForward;

        if(isJumping || !grounded)
            anim.Play("SonicJump");

        else if(isRunning && rb.velocity.magnitude <= 5)
            anim.Play("SonicRun");

        else if(isRunning && rb.velocity.magnitude > 5)
            anim.Play("SonicRunFast");

        else if(!isRunning && !isJumping)
            anim.Play("SonicIdle");
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
            transform.rotation = Quaternion.Lerp(transform.rotation, rotCur, Time.deltaTime * 5);
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
        {
            rb.AddForce(inputVelocity * runSpeed);
            isRunning = true;
        }
        if (inputVelocity.x == 0)
            isRunning = false;
        if(grounded && inputVelocity.x == 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
        
        if(inputVelocity.x > 0)
            lookForward = true;
        else if(inputVelocity.x < 0)
            lookForward = false;
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
            //inputVelocity.x = -Input.gyro.attitude.z;
            inputVelocity.x = testAngle;
            jumpPress = (Input.touchCount > 0) ? true : false;

        }

        if (jumpPress == true)
            Jump();
        
    }

    private void Jump()
    {
        if(grounded && !isJumping)
        {
            isJumping = true;
            float jumpBonus = Mathf.Clamp((rb.velocity.magnitude / 20), 0.7f, 0.8f);
            print(jumpBonus);
            rb.AddForce(Vector2.up * jumpForce * jumpBonus, ForceMode2D.Impulse);
        }
    }
}
