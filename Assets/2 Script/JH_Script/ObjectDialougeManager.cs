using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDialougeManager : MonoBehaviour
{
    public static ObjectDialougeManager instance;

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    public Text text;
    // public SpriteRenderer rendererDialogueWindow;

    private List<string> listSentences;
    // private List<Sprite> listSprites;
    // private List<Sprite> listDialogueWindows;

    private int count;

    // public Animator animDialogueWindow;
    public CanvasGroup dialogueGroup;

    public bool talking = false;
    private bool keyActivated = false;

    PlayerRenewal playerRenewal;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        text.text = "";
        listSentences = new List<string>();
        // listSprites = new List<Sprite>();
        // listDialogueWindows = new List<Sprite>();
        playerRenewal = GameObject.Find("player").GetComponent<PlayerRenewal>();
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        talking = true;
        dialogueGroup.alpha = 1;

        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            listSentences.Add(dialogue.sentences[i]);
            // listDialogueWindows.Add(dialogue.dialogueWindows[i]);
        }

        // animDialogueWindow.SetBool("Appear", true);
        StartCoroutine(StartDialougeCoroutine());
    }

    public void ExitDialogue()
    {
        text.text = "";
        count = 0;
        listSentences.Clear();
        // listSprites.Clear();
        // listDialogueWindows.Clear();
        // animDialogueWindow.SetBool("Appear", false);
        talking = false;
        playerRenewal.dontInput = false;
        dialogueGroup.alpha = 0;
    }

    IEnumerator StartDialougeCoroutine()
    {
        //if (count > 0)
        //{
        //    if (listDialogueWindows[count] != listDialogueWindows[count - 1])
        //    {
        //        animDialogueWindow.SetBool("Appear", false);
        //        yield return new WaitForSeconds(0.2f);
        //        rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
        //        animDialogueWindow.SetBool("Appear", true);
        //    }
        //    else
        //    {
        //        if (listSprites[count] != listSprites[count - 1])
        //        {
        //            yield return new WaitForSeconds(0.1f);
        //        }
        //        else
        //        {
        //            yield return new WaitForSeconds(0.05f);
        //        }
        //    }
        //}

        //else
        //{
        //    rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
        //}

        keyActivated = true;
        for (int i = 0; i < listSentences[count].Length; i++)
        {

            text.text += listSentences[count][i]; // 1글자씩 출력
            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (talking && keyActivated)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                keyActivated = false;
                count++;
                text.text = "";

                if (count == listSentences.Count)
                {
                    StopAllCoroutines();
                    ExitDialogue();
                }

                else
                {
                    StopAllCoroutines();
                    StartCoroutine(StartDialougeCoroutine());
                }
            }
        }
    }
}
