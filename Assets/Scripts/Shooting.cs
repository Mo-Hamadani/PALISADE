using UnityEngine;
using UnityEngine.InputSystem;


public class Shooting : MonoBehaviour
{
    public float strength = 1;
    public GameObject FlareProjectile;
    Camera cam;
    


    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 spawnLocation = (0.08f * Vector3.down) + (0.5f * cam.transform.forward) + cam.transform.position;
            
            GameObject flare = Instantiate(FlareProjectile, spawnLocation, cam.transform.rotation);
            Rigidbody flareRb = flare.GetComponent <Rigidbody>();
            flareRb.linearVelocity = cam.transform.forward * strength;

        }
    }
}
