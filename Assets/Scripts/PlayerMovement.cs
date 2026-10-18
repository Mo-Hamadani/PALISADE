using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //public variables
    public float camSensitivity = 1f;
    public float walkSpeed = 3f;
    public float runSpeed = 9f;
    public float acceleration = 20f;
    public float deceleration = 30f;


    //components
    Rigidbody rb;
    Camera camera;
    public Transform cameraTransform;

    //private variables
    float speed;
    float xRotation; 
    float yRotation;
    Vector2 movementInput; 
    bool running = false;
    float fov = 55;
    float runFov = 70f;
    float fovSpeed = 4f;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        camera =cameraTransform.GetComponent<Camera>();
    }

    void Update()
    {
        //Camera
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * camSensitivity/100;

        yRotation += mouseDelta.x;
        xRotation -= mouseDelta.y ;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
      
        //running
        if (Keyboard.current.shiftKey.isPressed)
        {
            speed = runSpeed;
            running = true;
            if (rb.linearVelocity == Vector3.zero) 
            {
             speed = walkSpeed;
             running = false;
            }
        }

        else
        {
            speed = walkSpeed;
            running = false;
        }
        
        //movement input
        movementInput = Vector2.zero;
        float sideSpeed = 1f;
        float backSpeed = 1f;
        float targetFov;
        if (running)
        {
            sideSpeed = 0.8f;
            backSpeed = .5f;
            targetFov = runFov;
        }
        else targetFov = fov; 

        if (Keyboard.current.wKey.isPressed) movementInput.y += 1f;
        if (Keyboard.current.sKey.isPressed) movementInput.y -= backSpeed;
        if (Keyboard.current.dKey.isPressed) movementInput.x += sideSpeed;
        if (Keyboard.current.aKey.isPressed) movementInput.x -= sideSpeed;
        
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFov, fovSpeed * Time.deltaTime);


        }

    
    private void FixedUpdate()
    //fixed update used for physics
    {
        //cam movement input
        Quaternion yaw = Quaternion.Euler(0f, yRotation, 0f);
        rb.MoveRotation(yaw);

        //keyboard movement input 
        Vector3 movementDirection = yaw * new Vector3(movementInput.x, 0f, movementInput.y);
        Vector3 targetVelocity = Vector3.ClampMagnitude(movementDirection, 1f) * speed;

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);


        float rate;
        if (movementInput != Vector2.zero)
        { rate = acceleration;}
        else 
        { rate = deceleration;}

        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, rate * Time.fixedDeltaTime);
        
        //vertical velocity
        rb.linearVelocity = new Vector3(horizontalVelocity.x,currentVelocity.y,horizontalVelocity.z);
        rb.AddForce(Vector3.down * 3.5f, ForceMode.Acceleration);



    }

}
