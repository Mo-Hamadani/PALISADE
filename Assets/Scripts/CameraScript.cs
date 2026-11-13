using UnityEngine;


public class CameraScript : MonoBehaviour
{
    public float bobAmount = 0.01f;
    public float bobSpeedWalk = 10;
    public float bobSpeedRun = 15;
    public float bobSmoothing = 10f;

    private float defaultY = 0f;
    private float targetY = 0f;
    private float crouchHeight = 0f;
    private float CamHeight = 0f;
    private float CamTargetHeight = 0f;


    Rigidbody playerRb;
    CapsuleCollider playerCapsule;
    PlayerMovement playerMovementScript;
    void Start()
    {
        defaultY = transform.localPosition.y;
        CamHeight = defaultY;
        playerRb = GetComponentInParent<Rigidbody>();
        playerCapsule = GetComponentInParent<CapsuleCollider>();
        playerMovementScript = GetComponentInParent<PlayerMovement>();
        crouchHeight = playerMovementScript.crouchHeight;

    }

    void Update()
    {

        //crouch height
        if (playerMovementScript.crouching)
        { CamTargetHeight = crouchHeight - 0.5f; }
        else
        { CamTargetHeight = defaultY; }

        CamHeight = Mathf.Lerp(CamHeight, CamTargetHeight, 25 * Time.deltaTime);

        //Headbob
        Vector3 currentPos = transform.localPosition;
        
        Vector3 velocity = playerRb.linearVelocity;
        velocity.y = 0;
        float bobHeight = 0f;

        if (playerMovementScript.crouching) {bobHeight = velocity.magnitude * bobAmount * .7f ; }
        else { bobHeight = velocity.magnitude * bobAmount; }


        if (playerMovementScript.running)
            { bobHeight = bobHeight * Mathf.Sin(Time.time * bobSpeedRun);}
        else
            { bobHeight = bobHeight * Mathf.Sin(Time.time * bobSpeedWalk);}


        targetY = CamHeight + bobHeight;
        if (playerMovementScript.isGrounded())
        {
            currentPos.y = Mathf.Lerp(currentPos.y, targetY, bobSmoothing * Time.deltaTime);
        }
        else
        {
            currentPos.y = Mathf.Lerp(currentPos.y, currentPos.y, bobSmoothing * Time.deltaTime);
        }

        transform.localPosition = currentPos;


    }
}
