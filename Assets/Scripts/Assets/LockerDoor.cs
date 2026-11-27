using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class LockerDoor : MonoBehaviour
{



    public bool playerInside = false;

    private float rotate = 0f;
    private float speed = 5f;
    private bool open = false;
    private float defaultRotate = 0f;
    void Start()
    {
        defaultRotate = transform.localRotation.y;
    }

    private void Update()
    {

        //rotate out
        if (open)
        {
            rotate = Mathf.Lerp(rotate, defaultRotate + 90f, speed * Time.deltaTime);

        }
        else
        {
            rotate = Mathf.Lerp(rotate, defaultRotate, speed * Time.deltaTime);

        }
        //rotate in

        if (Keyboard.current.fKey.wasPressedThisFrame && playerInside)
        {
            open = !open;
        }

        transform.localRotation = Quaternion.Euler(0f,0f, rotate);
    }
}



