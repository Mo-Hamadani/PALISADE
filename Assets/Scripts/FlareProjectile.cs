using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlareProjectile : MonoBehaviour
{
    private bool stuck = false;
    float noiseOffset;
    SphereCollider flareCollider;
    Rigidbody rb;
    PlayerMovement playerMovementScript;
    Light flareLight;

    CapsuleCollider playerCapsule;
    void Awake()
    {
        flareCollider = GetComponent<SphereCollider>();
        flareLight = GetComponentInChildren<Light>();
        rb = GetComponent<Rigidbody>();
        playerMovementScript = FindAnyObjectByType<PlayerMovement>();
        playerCapsule = playerMovementScript.capsule;

        Destroy(gameObject, 10f);
        Physics.IgnoreCollision(flareCollider, playerCapsule);
        rb.isKinematic = false;
        noiseOffset = Random.Range(0f, 100);
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * 20f, noiseOffset);
        flareLight.intensity = 3f * (0.1f + noise * 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!stuck)
        {
            stuck = true;
            flareCollider.enabled = false;
            Destroy(rb);
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;

            if (collision.collider.CompareTag("Enemy"))
            {

                transform.SetParent(collision.collider.transform, true);
                Debug.Log("ENEMY HIT");
            }
            
            else if (collision.collider.CompareTag("Panel"))
            {

                transform.SetParent(collision.collider.transform, true);
                Debug.Log("PANEL HIT");
            }

            else
            {
                transform.SetParent(collision.transform, true);
            }

        }
    }

}
