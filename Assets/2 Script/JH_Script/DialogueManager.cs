using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    #region Singleton
    private void Awake()
    {
        if(instance == null)
        {
            // DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    public Text text;
    public SpriteRenderer rendererSprite;
    public SpriteRenderer rendererDialogueWindow;
    // public SpriteRenderer rendererNamespace;

    private List<string> listSentences;
    private List<Sprite> listSprites;
    private List<Sprite> listDialogueWindows;
    // private List<Sprite> listNamespaces;

    private int count;

    public Animator animSprite;
    public Animator animDialogueWindow;
    // public Animator animNamespace;

    [SerializeField]
    private GameObject[] activeGameObjects;

    public bool talking = false;
    private bool keyActivated = false;

    PlayerRenewal playerRenewal;
    CameraToObject cameraToObject;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        text.text = "";
        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();
        // listNamespaces = new List<Sprite>();
        playerRenewal = GameObject.Find("player").GetComponent<PlayerRenewal>();
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        talking = true;

        for(int i = 0; i < dialogue.sentences.Length; i++)
        {
            listSentences.Add(dialogue.sentences[i]);
            listSprites.Add(dialogue.sprites[i]);
            listDialogueWindows.Add(dialogue.dialogueWindows[i]);
            // listNamespaces.Add(dialogue.namespaces[i]);
        }

        animSprite.SetBool("Appear", true);
        animDialogueWindow.SetBool("Appear", true);
        // animNamespace.SetBool("Appear", true);
        StartCoroutine(StartDialougeCoroutine());
    }

    public void ExitDialogue()
    {
        text.text = "";
        count = 0;
        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        animSprite.SetBool("Appear", false);
        animDialogueWindow.SetBool("Appear", false);
        // animNamespace.SetBool("Appear", false);
        talking = false;
        playerRenewal.dontInput = false;
        ActiveObject();
    }

    public void ActiveObject()
    {
        for(int i = 0; i < activeGameObjects.Length; i++)
        {
            if (activeGameObjects[i] == null)
                return;

            else
            {
                activeGameObjects[i].SetActive(true);
            }
        }
    }

    IEnumerator StartDialougeCoroutine()
    {
        if(count > 0)
        {
            if (listDialogueWindows[count] != listDialogueWindows[count - 1])
            {
                // animSprite.SetBool("Change", true);
                animDialogueWindow.SetBool("Appear", false);
                yield return new WaitForSeconds(0.2f);
                rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
                rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
                // rendererNamespace.GetComponent<SpriteRenderer>().sprite = listNamespaces[count];
                animDialogueWindow.SetBool("Appear", true);
                // animSprite.SetBool("Change", false);
            }
            else
            {
                if (listSprites[count] != listSprites[count - 1])
                {
                    // animSprite.SetBool("Change", true);
                    yield return new WaitForSeconds(0.1f);
                    rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
                    // animSprite.SetBool("Change", false);
                }
                //if(listNamespaces[count] != listNamespaces[count - 1])
                //{
                //    yield return new WaitForSeconds(0.1f);
                //    rendererNamespace.GetComponent<SpriteRenderer>().sprite = listNamespaces[count];
                //}
                else
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }

        else
        {
            rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
            rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
            // rendererNamespace.GetComponent<SpriteRenderer>().sprite = listNamespaces[count];
        }

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
                    ActiveObject();
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
