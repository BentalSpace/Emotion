using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("stateChange", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("stateChange", false);
        }
    }
}
