using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    SpriteRenderer house;

    [SerializeField]
    Sprite houseOpne;

    [SerializeField]
    GameObject warp;

    [SerializeField]
    float warpTime;

    // Start is called before the first frame update
    void Start()
    {
        house = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("¹®");
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                house.sprite = houseOpne;
                StartCoroutine(Warp());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            
        }
    }

    IEnumerator Warp()
    {
        yield return new WaitForSeconds(warpTime);
        warp.SetActive(true);
        Debug.Log("¿öÇÁ");
    }
}
