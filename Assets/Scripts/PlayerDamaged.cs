using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamaged : MonoBehaviour
{
    //todo: add screen shake, add heartbeat sfx

    public bool IsDamaged = false;
    public GameObject ScreenEffect;
    public Rigidbody PlayerBody;
    bool Invincible = false;
    float InvincibilityTimer = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Invincible == true)
        {
            InvincibilityTimer -= Time.deltaTime; //decrease timer
            if(InvincibilityTimer <= 0f) //if timer has run out
            {
                InvincibilityTimer = 3f; //reset timer
                Invincible = false;
            }
        }

    }

    private void OnCollisionEnter(Collision collision) //on collision
    {
        if (collision.gameObject.layer == 31) //if collider is on enemy layer
        {
            if (IsDamaged == true) //if player is already damaged
            {
                //SceneManager.LoadScene(""); //load death screen
                Debug.Log("RIP");
            }
            else
            {
                Invincible = true;
                IsDamaged = true; //set damaged to true
                //apply screenshake
                GameObject ScreenClone = (GameObject)Instantiate(ScreenEffect); //create screen effect
            }
        }

        if(collision.gameObject.layer == 0) //change to layer for health items
        {
            if (IsDamaged == true)
            {
                IsDamaged = false; //heals player
                Destroy(collision.gameObject); //destroys health item
            }
        }
    }
}
