using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDialogue : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("��ȭâ Ȱ��ȭ");
            theDM.ShowDialogue(dialogue);
            playerRenewal.dontInput = true;
            playerRenewal.animator.SetBool("isWalk", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("��ȭâ ��Ȱ��ȭ");
            // theDM.ExitDialogue();
            // Destroy(gameObject);
            // gameObject.SetActive(false);
        }
    }

    public void ButtonClick()
    {
        theDM.ExitDialogue();
    }
}
