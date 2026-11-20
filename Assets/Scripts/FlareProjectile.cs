using UnityEngine;

public class FlareProjectile : MonoBehaviour
{
    SphereCollider flareCollider;
    Rigidbody rb;
    void Start()
    {
        flareCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, 10f);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //rb.linearVelocity = Vector3.zero;
        //rb.mass = 0;

    }

}
