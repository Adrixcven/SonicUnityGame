using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the climbing methods for the Player.
/// </summary>
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
    private bool smallClimbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;

    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;
    private bool smallWallFront;

    /// <summary>
    /// Updates the character's behavior by performing wall checks, managing the state machine, and executing specific movements based on the current state.
    /// </summary>
    private void Update()
    {
        WallCheck();
        StateMachine();
        if (climbing) ClimbingMovement();
        if (smallClimbing) SmallClimbingMovement();
    }
    /// <summary>
    /// Controls the state machine for wall climbing behavior. Checks for input and wall conditions to initiate climbing, decrement climbing timer, and stop climbing when necessary.
    /// </summary>
    private void StateMachine()
    {
        if (wallFront && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && wallLookAngle < maxWallLookAngle)
        {
            if (!climbing && climbTimer > 0) StartClimbing();

            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer < 0) StopClimbing();
        }
        else if (smallWallFront && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && wallLookAngle < maxWallLookAngle)
        {
            if (!smallClimbing && climbTimer > 0) StartSmallClimbing();

            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer < 0) StopClimbing();
        }
        else
        {
            if (climbing) StopClimbing();
            if (smallClimbing) StopClimbing();
        }
    }

    /// <summary>
    /// Performs wall detection in the forward direction using sphere casting.
    /// </summary>
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

    /// <summary>
    /// Handles the initiation of climbing, setting relevant flags.
    /// </summary>
    private void StartClimbing()
    {
        climbing = true;
        pm.climbing = true;
    }
    /// <summary>
    /// Initiates a small climbing action, setting necessary flags.
    /// </summary>
    private void StartSmallClimbing()
    {
        smallClimbing = true;
        pm.climbing = true;
    }

    /// <summary>
    /// Manages climbing movement by updating the Rigidbody velocity.
    /// </summary>
    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }
    /// <summary>
    /// Manages small climbing movement by updating the Rigidbody velocity.
    /// </summary>
    private void SmallClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, smallClimbSpeed, rb.velocity.z);
    }
    /// <summary>
    /// Stops both regular and small climbing actions, resetting relevant flags.
    /// </summary>
    private void StopClimbing()
    {
        climbing = false;
        smallClimbing = false;
        pm.climbing = false;
    }
}
