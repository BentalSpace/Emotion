using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToObject : MonoBehaviour
{
    Animator cameraAnim;

    PlayerRenewal playerRenewal;
    DialogueManager dialogueManager;

    public GameObject plantDialouge;

    // Start is called before the first frame update
    void Start()
    {
        cameraAnim = GameObject.Find("Main Camera").GetComponent<Animator>();
        playerRenewal = GameObject.Find("player").GetComponent<PlayerRenewal>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }

    void Update()
    {
        transform.position = new Vector3(167.71f, 2.25f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueManager.ExitDialogue();
            StartCoroutine(CameraAnimMove());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine(CameraAnimMove());
        }
    }

    IEnumerator CameraAnimMove()
    { 
        playerRenewal.dontInput = true;
        yield return new WaitForSeconds(0.7f);
        cameraAnim.enabled = true;
        cameraAnim.SetBool("AnimStart", true);

        yield return new WaitForSeconds(5.0f);
        cameraAnim.SetBool("AnimStart", false);

        yield return new WaitForSeconds(3.0f);
        cameraAnim.enabled = false;
        playerRenewal.dontInput = false;
        plantDialouge.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        plantDialouge.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        this.gameObject.SetActive(false);
    }
}
