using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDialouge : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;

    private ObjectDialougeManager theDM;

    PlayerRenewal playerRenewal;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<ObjectDialougeManager>();
        playerRenewal = GameObject.Find("player").GetComponent<PlayerRenewal>();
    }

    private void OnMouseDown()
    {
        //dialogueON = true;
        //if (dialogueON)
        //{
        //    theDM.ShowDialogue(dialogue);
        //}

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (ObjectDialougeManager.instance.dialogueGroup.alpha == 0)
            {
                theDM.ShowDialogue(dialogue);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            theDM.ExitDialogue();
            //gameObject.SetActive(false);
            Destroy(this);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        Debug.Log("대화창 활성화");
    //        theDM.ShowDialogue(dialogue);
    //        playerRenewal.dontInput = true;
    //        playerRenewal.animator.SetBool("isWalk", false);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        Debug.Log("대화창 비활성화");
    //        theDM.ExitDialogue();
    //    }
    //}

    public void ButtonClick()
    {
        theDM.ExitDialogue();
    }
}
