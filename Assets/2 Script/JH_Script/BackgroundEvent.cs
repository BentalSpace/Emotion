using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BackgroundEvent : MonoBehaviour
{
    Animator backgroundAnim;

    PlayerRenewal playerRenewal;

    // Start is called before the first frame update
    void Start()
    {
        backgroundAnim = GameObject.Find("Angry_Happy_Back").GetComponent<Animator>();
        playerRenewal = GameObject.Find("player").GetComponent<PlayerRenewal>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(BackgroundAnim());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine(BackgroundAnim());
        }
    }

    IEnumerator BackgroundAnim()
    {
        backgroundAnim.enabled = true;
        backgroundAnim.SetBool("AnimStart", true);
        yield return new WaitForSeconds(1.0f);

        yield return new WaitForSeconds(1.5f);
        backgroundAnim.SetBool("AnimStart", false);
        backgroundAnim.enabled = false;
    }
}
