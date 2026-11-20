using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamaged : MonoBehaviour
{
    public bool IsDamaged = false;
    public GameObject ScreenEffect;
    GameObject ScreenClone;
    public Rigidbody PlayerBody;
    float InvincibilityTimer;
    public AudioClip HeartSound;
    AudioSource AudioPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioPlayer = GetComponent<AudioSource>(); //find audioplayer and set sound to heartbeat
        AudioPlayer.clip = HeartSound;
    }

    // Update is called once per frame
    void Update()
    {
        if(InvincibilityTimer > 0)
        {
            InvincibilityTimer -= Time.deltaTime; //decrease timer
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
                InvincibilityTimer = 2f; //start invincibility
                IsDamaged = true; //set damaged to true
                var Camera = GetComponentInChildren<CameraScript>(); //shake camera
                Camera.ScreenShake();
                ScreenClone = (GameObject)Instantiate(ScreenEffect); //create screen effect
                AudioPlayer.Play();
            }
        }

        if(collision.gameObject.layer == 30) //if collider is on healing layer
        {
            if (IsDamaged == true)
            {
                IsDamaged = false; //heals player
                Destroy(collision.gameObject); //destroys health item
                Destroy(ScreenClone); //removes screen effect
                AudioPlayer.Stop();
            }
        }
    }
}
