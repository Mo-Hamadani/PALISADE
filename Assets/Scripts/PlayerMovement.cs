using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //public variables
    public float camSensitivity = 1f;
    public float walkSpeed = 3f;

    
    //components
    Rigidbody rb;
    public Transform cameraTransform;

    //private variables
    float xRotation; 
    float yRotation;
    Vector2 movementInput; 


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Camera
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * camSensitivity/100;

        yRotation += mouseDelta.x;
        xRotation -= mouseDelta.y ;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        //movement input
        movementInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) movementInput.y += 1f;
        if (Keyboard.current.sKey.isPressed) movementInput.y -= 1f;
        if (Keyboard.current.dKey.isPressed) movementInput.x += 1f;
        if (Keyboard.current.aKey.isPressed) movementInput.x -= 1f;
    }

    
    private void FixedUpdate()
    //fixed update used for physics
    {
        //cam movement input
        Quaternion yaw = Quaternion.Euler(0f, yRotation, 0f);
        rb.MoveRotation(yaw);

        //keyboard movement input 
        Vector3 movementDirection = yaw * new Vector3(movementInput.x, 0f, movementInput.y);
        Vector3 velocity = movementDirection.normalized * walkSpeed;

        //preserve vertical velocity
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

}
