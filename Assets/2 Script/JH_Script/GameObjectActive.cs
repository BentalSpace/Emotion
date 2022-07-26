using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectActive : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objs;

    public float invokeTime;

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject == null)
                return;

            Invoke("Active", invokeTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject == null)
                return;

            for (int i = 0; i < objs.Length; i++)
            {
                // objs[i].SetActive(false);
                Destroy(objs[i]);
            }
        }
    }

    public void Active()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i] == null) 
                return;
                
            objs[i].SetActive(true);
        }
    }
}
