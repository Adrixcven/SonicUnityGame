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
    /// Actualiza el comportamiento del personaje realizando comprobaciones de pared, gestionando la máquina de estados y ejecutando movimientos específicos basados en el estado actual.
    /// </summary>
    private void Update()
    {
        WallCheck();
        StateMachine();
        if (climbing) ClimbingMovement();
        if (smallClimbing) SmallClimbingMovement();
    }
    /// <summary>
    /// Controla la máquina de estados para el comportamiento de escalada en la pared. 
    /// Verifica la entrada y las condiciones de la pared para iniciar la escalada, decrementar el temporizador de escalada y detener la escalada cuando sea necesario.
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
    /// Realiza la detección de pared en la dirección frontal utilizando un lanzamiento de esfera.
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
    /// Maneja el inicio de la escalada, estableciendo banderas relevantes.
    /// </summary>
    private void StartClimbing()
    {
        climbing = true;
        pm.climbing = true;
    }
    /// <summary>
    /// Inicia una acción de escalada pequeña, estableciendo las banderas necesarias.
    /// </summary>
    private void StartSmallClimbing()
    {
        smallClimbing = true;
        pm.climbing = true;
    }

    /// <summary>
    /// Gestiona el movimiento de escalada actualizando la velocidad del Rigidbody.
    /// </summary>
    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }
    /// <summary>
    /// Gestiona el movimiento de escalada pequeña actualizando la velocidad del Rigidbody.
    /// </summary>
    private void SmallClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, smallClimbSpeed, rb.velocity.z);
    }
    /// <summary>
    /// Detiene tanto las acciones regulares como las de escalada pequeña, restableciendo las banderas relevantes.
    /// </summary>
    private void StopClimbing()
    {
        climbing = false;
        smallClimbing = false;
        pm.climbing = false;
    }
}
