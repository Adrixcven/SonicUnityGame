using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float climbSpeed;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Vector3 backMoveDirection;
    Vector3 sideMoveDirection;

    Rigidbody rb;

    GameObject character;

    public AudioSource jumpSound;

    public Animator anim;
    public ParticleSystem particleSystem;

    public float rotationSpeed = 1000f;

    public MovementState state;
    public enum MovementState
    {
        climbing
    }
    public bool climbing;

    [Header ("SpawnPosition")]
    public Transform Position;

    [Header("Another Files")]
    public Checkpoint checkpoint;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        particleSystem.Stop();

        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {


        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.2f + 0.2f, whatIsGround);
        if (grounded)
            anim.SetBool("InGround", true);
        else
            anim.SetBool("InGround", false);


        MyInput();
        SpeedControl();

        anim.SetFloat("Speed", rb.velocity.magnitude);

        if(anim.GetFloat("Speed") > 0 && readyToJump && grounded)
            particleSystem.Play();
        else
            particleSystem.Stop();

        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void StateMachine()
    {
        if (climbing)
        {
            state = MovementState.climbing;
            moveSpeed = climbSpeed;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();

    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        

        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        
        


        // when to jump
        if (Input.GetKey(jumpkey) && readyToJump && grounded)
        {


            readyToJump = false;

            Jump();
            jumpSound.Play();
            anim.SetBool("Jumping", true);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        sideMoveDirection = orientation.forward * horizontalInput + orientation.right * verticalInput;
        backMoveDirection = orientation.forward * -verticalInput + orientation.right * -horizontalInput;
        

        // on ground
        if (grounded)
        {
            if(verticalInput < 0)
                rb.AddForce(backMoveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            else if(horizontalInput != 0 && verticalInput == 0)
            {
                if(horizontalInput >0)
                    rb.AddForce(sideMoveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
                else if(horizontalInput < 0)
                    rb.AddForce(-sideMoveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            
        }
        // in air
        else if (!grounded)
        if(verticalInput < 0)
                rb.AddForce(backMoveDirection.normalized * moveSpeed/2 * 10f, ForceMode.Force);
            else if(horizontalInput != 0 && verticalInput == 0)
            {
                if(horizontalInput >0)
                    rb.AddForce(sideMoveDirection.normalized * moveSpeed/2 * 10f, ForceMode.Force);
                else if(horizontalInput < 0)
                    rb.AddForce(-sideMoveDirection.normalized * moveSpeed/2 * 10f, ForceMode.Force);
            }
            else
                rb.AddForce(moveDirection.normalized * moveSpeed/2 * 10f, ForceMode.Force);
            
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        anim.SetBool("Jumping", false);
    }

   
    

}