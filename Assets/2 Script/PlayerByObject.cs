using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerByObject : MonoBehaviour
{
    [SerializeField]
    PlayerRenewal player;
    [SerializeField]
    StageManager sm;

    bool coroutineRun;

    IEnumerator dieCoroutine;


    Animator animator;
    Rigidbody2D rigid;

    void Awake() {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "stageSave") {
            sm.ChapterStageNum = collision.gameObject.name;
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && !coroutineRun) {
            //Àå¾Ö¹°°ú ºÎµúÇûÀ»¶§
            animator.SetTrigger("dieTrigger");
            dieCoroutine = Die();
            StartCoroutine(dieCoroutine);
        }

        if(collision.gameObject.tag == "nextChapter") {
            GameObject chapterStage = GameObject.Find("StageManager");
            chapterStage.name = "StageNum";
            SceneManager.LoadScene(int.Parse(sm.ChapterStageNum.Split('-')[0]) + 2);
            DontDestroyOnLoad(chapterStage);
        }
    }

    IEnumerator Die() {
        coroutineRun = true;
        rigid.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(2f);
        coroutineRun = false;

        //transform.position = new Vector2(0.0f, -2.0f);
        GameObject chapterStage = GameObject.Find("StageManager");
        chapterStage.name = "StageNum";
        //rigid.bodyType = RigidbodyType2D.Dynamic;
        //animator.SetTrigger("resTrigger");
        SceneManager.LoadScene(int.Parse(sm.ChapterStageNum.Split('-')[0]) + 1);
        DontDestroyOnLoad(chapterStage);
    }
}
