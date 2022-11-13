using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Sun : MonoBehaviour
{
    // Vector3 target = new Vector3(3, 10, 0); // µµÂø ÁöÁ¡
    // Vector3 target = new Vector3(10, 10, 0); // µµÂø ÁöÁ¡

    [SerializeField]
    private float t;

    [SerializeField]
    private Light2D light2D;

    [SerializeField]
    private float intensity;

    [SerializeField]
    GameObject player;

    [SerializeField]
    PlayerRenewal playerRenewal;

    SpriteRenderer playerSpriteRenderer;

    [SerializeField]
    SpriteRenderer[] backgrounds;

    Vector2 playerNeedPos;

    void Awake() 
    {
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        //Vector3 targetVec = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f);
        //transform.RotateAround(targetVec, Vector3.back, 3);
        //transform.rotation = Quaternion.identity;
        Move();

    }
    void Move()
    {
        playerNeedPos = new Vector2(player.transform.position.x, 0);
        if(Input.GetKey(KeyCode.RightArrow))
        {
            if (playerSpriteRenderer.flipX)
            {
                transform.position = new Vector2(0, transform.position.y) + playerNeedPos;
            }

            else
            {
                transform.position = new Vector2(0, transform.position.y + Time.deltaTime / t) + playerNeedPos;

                if(transform.position.y > 4)
                {
                    t = 3.5f;
                }

                if (transform.position.y > 8)
                {
                    Destroy(gameObject);
                }
            }
            if(light2D.intensity <= 1.0f)
            {
                light2D.intensity += intensity;
            }
        }
    }
}
