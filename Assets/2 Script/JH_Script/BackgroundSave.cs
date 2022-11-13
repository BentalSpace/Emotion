using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BackgroundSave : MonoBehaviour
{
    [SerializeField]
    GameObject background;

    [SerializeField]
    PlayerRenewal player;

    [SerializeField]
    GameObject sun;

    [SerializeField]
    Light2D light2D;

    void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // transform.position = player.transform.position; 
            if(background.transform.position.y > 20.9)
            {
                background.transform.Translate(0, 36f, 0);
            }
            else if(background.transform.position.y < 0)
            {
                background.transform.Translate(0, 38f, 0);
            }
            light2D.intensity = 1;
        }
        Destroy(sun);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this);
        }
    }
}
