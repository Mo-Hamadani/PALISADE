using UnityEngine;
using UnityEngine.UIElements;

public class FlareRenderer : MonoBehaviour
{
    public Sprite flare;

    private PlayerMovement playerMovementScript;
    private Transform playerTransform;
    private Transform flareTransform;
    float noiseOffset;

    void Start()
    {
        playerMovementScript = FindAnyObjectByType<PlayerMovement>();
        playerTransform = playerMovementScript.transform;
        flareTransform = transform.parent;
    }

    void LateUpdate()
    {
        //sprite always faces the player
        Vector3 yawRotate = playerTransform.position;
        yawRotate.y = transform.position.y;

        transform.LookAt(yawRotate);

        float noise = Mathf.PerlinNoise(Time.time * 20f, noiseOffset);
        float rotationOffset =  3f * (0.1f + noise * 3f);
        //Rotate sprite
    }
}
