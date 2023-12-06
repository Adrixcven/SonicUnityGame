using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 input;
    private CharacterController characterController;
    private Vector3 direction;

    private bool hold = false;
    private float currentSpeed;
    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float velocity;
    [SerializeField] private float smoothTime = 0.05f;
    private float currentVelocity;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpPower;

    
    private void Awake()
    {
        var targetAngle = Mathf.Atan2(0.0f, 0.0f) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
           
    }
    private void ApplyGravity()
    {
        if (isGrounded() && velocity < 0.0f)
        {
            if(hold){
                speed = currentSpeed;
                speed += acceleration;
                if(speed > maxSpeed) speed=maxSpeed;
                currentSpeed = speed;
            }
            velocity = -1.0f;
        }
        else{
            speed = 1.0f;
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        
        direction.y = velocity;
    }
    private void ApplyRotation()
    {
    if (input.sqrMagnitude == 0) return;
        var targetAngle = Mathf.Atan2(-direction.z, direction.x) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void ApplyMovement()
    {
        characterController.Move(direction * speed * Time.deltaTime);
    }
    public void Move(InputAction.CallbackContext context)
   {
    hold = true;
    if (context.canceled)
    {
        hold = false;
        speed = 1.0f;
        currentSpeed = 1.0f;
    }

    input = context.ReadValue<Vector2>();
    direction = new Vector3(input.y, 0.0f, -input.x);
   }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isGrounded()) return;
        
        velocity += jumpPower;
        
    }
    void OnCollisionEnter(Collision collision)
    {
        // Verifica si la colisión es con una pared
        if (collision.gameObject.tag == "Wall")
        {
            // Alinea el personaje con la normal de la pared para caminar en ella
            transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
        }
    }

    private bool isGrounded() => characterController.isGrounded;
}

