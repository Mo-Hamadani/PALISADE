using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //public variables
    public float camSensitivity = 1f;
    public float walkSpeed = 3f;
    public float runSpeed = 9f;
    public float crouchSpeed = 9f;
    public float acceleration = 20f;
    public float deceleration = 30f;
    public bool running = false;
    public bool crouching = false;
    public float crouchHeight = 0.6f;
    public float jumpHeight = 1f;

    //components
    Rigidbody rb;
    Camera camera;
    CapsuleCollider capsule;
    public Transform cameraTransform;

    //private variables
    float speed;
    float xRotation; 
    float yRotation;
    Vector2 movementInput; 
    float fov = 55;
    float runFov = 65f;
    float fovSpeed = 5f;
    float standHeight = 0f;
    Vector3 lastPos = Vector3.zero;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        camera =cameraTransform.GetComponent<Camera>();
        capsule = GetComponent<CapsuleCollider>();
        standHeight = capsule.height;
    }

    void Update()
    {
        //Camera
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * camSensitivity / 100;

        yRotation += mouseDelta.x;
        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        


        //running
        if (Keyboard.current.shiftKey.isPressed)
        {
            running = true;
            
            if (rb.linearVelocity == Vector3.zero)
            {
                running = false;
            }

            if (crouching)
            {
                if (canStandUp())
                {
                    crouching = false;
                    capsule.height = standHeight;

                }
                else
                {
                    crouching = true;
                    capsule.height = crouchHeight;
                    running = false;
                } 
            }
        }
        
        if (Keyboard.current.shiftKey.wasReleasedThisFrame)
        {
            running = false;
        }

        //crouching
        if (Keyboard.current.cKey.wasPressedThisFrame ||
            (crouching && Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded()))
        { 
            //uncrouch
            if (crouching && canStandUp())
            {
                crouching = false;
                capsule.height = standHeight;
                rb.position += Vector3.up * 0.2f;

                
            }
            //crouch
            else
            {
                crouching = true;
                capsule.height = crouchHeight;
            }
        }

        //set speeds
        if (crouching) {speed = crouchSpeed; }
        else if (running) {speed = runSpeed; } 
        else {speed = walkSpeed; }

        //movement input
        float targetFov;
        movementInput = Vector2.zero;
        if (running)
        {
            if (Keyboard.current.wKey.isPressed) movementInput.y += 1f;
            if (Keyboard.current.sKey.isPressed) movementInput.y -= 0.5f;
            if (Keyboard.current.dKey.isPressed) movementInput.x += 0.8f;
            if (Keyboard.current.aKey.isPressed) movementInput.x -= 0.8f;

            targetFov = runFov;

        }
        else
        {

        }
        {
            if (Keyboard.current.wKey.isPressed) movementInput.y += 1f;
            if (Keyboard.current.sKey.isPressed) movementInput.y -= 1f;
            if (Keyboard.current.dKey.isPressed) movementInput.x += 1f;
            if (Keyboard.current.aKey.isPressed) movementInput.x -= 1f;

            targetFov = fov;
        }

        //Jump input
        
        if (Keyboard.current.spaceKey.isPressed && canStandUp() && !crouching && isGrounded())
        {
            Vector3 jumpUp = Vector3.up * jumpHeight * 10;
            rb.AddForce(jumpUp);
        }
        
        lastPos = transform.position;

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

    bool canStandUp()
    {
        return !Physics.Raycast(capsule.bounds.center, Vector3.up, (standHeight / 2f) + 0.5f, ~0, QueryTriggerInteraction.Ignore);
    }
    
    //isgrounded
    public bool isGrounded()
    {
        return (Physics.Raycast(capsule.bounds.center, Vector3.down, capsule.bounds.extents.y + 0.1f,
                ~0, QueryTriggerInteraction.Ignore));
    }

}
