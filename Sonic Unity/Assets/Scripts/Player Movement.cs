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
    public LayerMask whatIsPlatform;
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

    [Header("SpawnPosition")]
    public Transform Position;

    [Header("Another Files")]
    public Checkpoint checkpoint;

    /// <summary>
    /// Asigna Rigidbody, Animator y detiene el sistema de partículas. Establece valores iniciales para Rigidbody y salto.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        particleSystem.Stop();

        rb.freezeRotation = true;

        readyToJump = true;
    }

    /// <summary>
    /// Llamado en cada frame. Maneja la entrada del jugador, actualiza los parámetros de la animación y controla la velocidad de movimiento.
    /// Además, gestiona el sistema de partículas según el movimiento del jugador.
    /// </summary>
    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.2f + 0.2f, whatIsGround | whatIsPlatform);
        if (grounded)
            anim.SetBool("InGround", true);
        else
            anim.SetBool("InGround", false);


        MyInput();
        SpeedControl();

        anim.SetFloat("Speed", rb.velocity.magnitude);

        if (anim.GetFloat("Speed") > 0 && readyToJump && grounded)
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
    /// <summary>
    /// Establece el estado de movimiento según ciertas condiciones.
    /// </summary>
    private void StateMachine()
    {
        if (climbing)
        {
            state = MovementState.climbing;
            moveSpeed = climbSpeed;
        }
    }
    /// <summary>
    /// Llamado a una tasa fija en cada actualización de físicas. Mueve al jugador según la entrada.
    /// </summary>
    private void FixedUpdate()
    {
        MovePlayer();

    }
    /// <summary>
    /// Lee la entrada del jugador para el movimiento y el salto.
    /// </summary>
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 rotationEulerAngles = transform.rotation.eulerAngles;

        // Obtener el valor de rotación a lo largo del eje x
        float rotationX = rotationEulerAngles.x;
        float rotationZ = rotationEulerAngles.z;

        // Calcular la dirección de entrada para la rotación
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Rotar al jugador según la dirección de entrada
        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);

            transform.rotation = Quaternion.Euler(rotationX, angle, rotationZ);
        }

        // Manejar salto
        if (Input.GetKey(jumpkey) && readyToJump && grounded)
        {


            readyToJump = false;

            Jump();
            jumpSound.Play();
            anim.SetBool("Jumping", true);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    /// <summary>
    /// Mueve al jugador según la dirección de entrada y el estado en el suelo.
    /// </summary>
    private void MovePlayer()
    {
        // Calcular la dirección de movimiento
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        sideMoveDirection = orientation.forward * horizontalInput + orientation.right * verticalInput;
        backMoveDirection = orientation.forward * -verticalInput + orientation.right * -horizontalInput;


        // Aplicar fuerzas según el estado de movimiento (en el suelo o en el aire)
        if (grounded)
        {
            if (verticalInput < 0)
                rb.AddForce(backMoveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            else if (horizontalInput != 0 && verticalInput == 0)
            {
                if (horizontalInput > 0)
                    rb.AddForce(sideMoveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
                else if (horizontalInput < 0)
                    rb.AddForce(-sideMoveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        }
        // En el aire
        else if (!grounded)
            if (verticalInput < 0)
                rb.AddForce(backMoveDirection.normalized * moveSpeed / 2 * 10f, ForceMode.Force);
            else if (horizontalInput != 0 && verticalInput == 0)
            {
                if (horizontalInput > 0)
                    rb.AddForce(sideMoveDirection.normalized * moveSpeed / 2 * 10f, ForceMode.Force);
                else if (horizontalInput < 0)
                    rb.AddForce(-sideMoveDirection.normalized * moveSpeed / 2 * 10f, ForceMode.Force);
            }
            else
                rb.AddForce(moveDirection.normalized * moveSpeed / 2 * 10f, ForceMode.Force);

    }
    /// <summary>
    /// Controla la velocidad de movimiento del jugador, limitándola si es necesario.
    /// </summary>
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limitar la velocidad si es necesario
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }
    /// <summary>
    /// Hace que el jugador salte aplicando una fuerza hacia arriba.
    /// </summary>
    private void Jump()
    {
        // Reiniciar la velocidad en y
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    /// <summary>
    /// Restablece la capacidad de saltar después de un período de enfriamiento.
    /// </summary>
    private void ResetJump()
    {
        readyToJump = true;
        anim.SetBool("Jumping", false);
    }
}
