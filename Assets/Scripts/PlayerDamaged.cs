using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamaged : MonoBehaviour
{
    public bool IsDamaged = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(0==1)//player is hit
        {
            if (IsDamaged == true)
            {
                SceneManager.LoadScene(""); //load death screen
            }
            else
            {
                IsDamaged = true;
            }
        }

        if(IsDamaged == true)
        {
            //apply screen effects
        }
    }
}
