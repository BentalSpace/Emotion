using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable : MonoBehaviour
{
    [SerializeField]
    GameObject[] backgrounds;

    void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            backgrounds[0].GetComponent<Background>().enabled = true;
            backgrounds[1].GetComponent<Background>().enabled = true;
            backgrounds[2].GetComponent<Background>().enabled = true;
            Debug.Log("스크립트 활성화");
        }
    }
}
