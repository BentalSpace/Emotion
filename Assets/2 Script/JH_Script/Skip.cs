using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : MonoBehaviour
{
    private DialogueManager theDM;

    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
    }

    public void ButtonClick()
    {
        Debug.Log("½ºÅµ");
        theDM.ExitDialogue();
    }
}
