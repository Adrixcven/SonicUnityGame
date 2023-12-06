using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public LayerMask whatIsWall;
    public LayerMask whatIsSmallWall;
    public PlayerMovement pm;

    [Header("Climbing")]
    public float climbSpeed;
    public float smallClimbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;

    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;
    private bool smallWallFront;

    private void Update()
    {
        WallCheck();
        StateMachine();
        if(climbing) ClimbingMovement();
    }

    private void StateMachine()
    {
        if(wallFront && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && wallLookAngle < maxWallLookAngle)
        {
            if(!climbing && climbTimer > 0) StartClimbing();

            if (climbTimer> 0) climbTimer -= Time.deltaTime;
            if(climbTimer < 0) StopClimbing();
        }
        else if(smallWallFront && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && wallLookAngle < maxWallLookAngle)
        {
            if(!climbing && climbTimer > 0) StartClimbing();

            if (climbTimer> 0) climbTimer -= Time.deltaTime;
            if(climbTimer < 0) StopClimbing();
        }
        else
        {
            if(climbing) StopClimbing();
        }
    }

    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        smallWallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsSmallWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        if (pm.grounded) 
        {
            climbTimer = maxClimbTime;
        }

    }
    

    private void StartClimbing()
    {
        climbing = true;
        pm.climbing = true;
    }

    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }

    private void SmallClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, smallClimbSpeed, rb.velocity.z);
    }

    private void StopClimbing()
    {
        climbing = false;
        pm.climbing = false;
    }
}
