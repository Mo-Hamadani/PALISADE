using UnityEngine;

public class locker : MonoBehaviour
{
    BoxCollider triggerBox;
    PlayerMovement PlayerMovementScript;
    LockerDoor LockerDoorScript;
    CapsuleCollider playerCollider;

    void Start()
    {
        PlayerMovementScript = FindAnyObjectByType<PlayerMovement>();
        playerCollider = PlayerMovementScript.GetComponent<CapsuleCollider>();
        LockerDoorScript = GetComponentInChildren<LockerDoor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
        {
            LockerDoorScript.playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == playerCollider)
        {
            LockerDoorScript.playerInside = false;
        }
    }

}

