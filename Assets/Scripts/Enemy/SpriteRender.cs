using UnityEngine;

public class SpriteRender : MonoBehaviour
{
    public Sprite front;
    public Sprite back;
    public Sprite right;
    public Sprite left;

    private PlayerMovement playerMovementScript;
    private Transform playerTransform;
    private Transform enemyTransform;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        playerMovementScript = FindAnyObjectByType<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = playerMovementScript.transform;
        enemyTransform = transform.parent;
    }

    void LateUpdate()
    {
        //sprite always faces the player
        Vector3 yawRotate = playerTransform.position;
        yawRotate.y = transform.position.y;

        transform.LookAt(yawRotate);

        //enemy looking direction
        Vector3 enemyForward = enemyTransform.forward;
        enemyForward.y = 0f; //ignore height
        
        //direction from enemy to player
        Vector3 toPlayer = playerTransform.position - enemyTransform.position;
        toPlayer.y = 0f; //ignore height

        float directionAngle = Vector3.SignedAngle(enemyForward, toPlayer,Vector3.up);

        //change sprite texture
        if (Mathf.Abs(directionAngle) <= 45f)
        { spriteRenderer.sprite = front; }

        else if (Mathf.Abs(directionAngle) >= 135f)
        { spriteRenderer.sprite = back; }

        else if (directionAngle > 0f)
        { spriteRenderer.sprite = right; }
        
        else
        { spriteRenderer.sprite = left; }

    }
}
