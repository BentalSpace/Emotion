using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialouge : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;

    private DialogueManager theDM;

    PlayerRenewal playerRenewal;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        playerRenewal = GameObject.Find("player").GetComponent<PlayerRenewal>();
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            theDM.ExitDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("대화창 활성화");
            theDM.ShowDialogue(dialogue);
            playerRenewal.dontInput = true;
            playerRenewal.animator.SetBool("isWalk", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("대화창 비활성화");
            // theDM.ExitDialogue();
            Destroy(gameObject);
        }
    }

    public void ButtonClick()
    {
        theDM.ExitDialogue();
    }
}
