using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerByObject : MonoBehaviour
{
    [SerializeField]
    PlayerRenewal player;
    [SerializeField]
    CamerMove cameraMove;
    [SerializeField]
    Fairy fairy;
    [SerializeField]
    GameObject seedling;
    [SerializeField]
    GameObject downFairy;

    int index;
    float[] targetPosX;

    bool coroutineRun;
    bool move;

    IEnumerator dieCoroutine;
    IEnumerator plantCoroutine;


    Animator animator;
    Rigidbody2D rigid;

    void Awake() {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        targetPosX = new float[2] { 158, 165f };

        plantCoroutine = PlantATree();
    }
    private void Update() {
        charMove();
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "animationStart" && !coroutineRun) {
            player.dontInput = true;
            StartCoroutine(plantCoroutine);
            collision.gameObject.SetActive(false);
        }
    }

    IEnumerator PlantATree() {
        index = 0;
        animator.SetBool("isWalk", false);
        coroutineRun = true;
        rigid.sharedMaterial = null;
        move = true;

        yield return new WaitForSeconds(2f);
        seedling.SetActive(true);
        index = 1;
        yield return new WaitForSeconds(2f);
        cameraMove.animMove = true;
        yield return new WaitForSeconds(2f);
        move = true;
        yield return new WaitForSeconds(3f);
        downFairy.SetActive(false);
        //fairy.animMove = true;
        fairy.StartCoroutine("AnimMove1");

        yield return new WaitForSeconds(2.5f);
        player.dontInput = false;
        animator.SetBool("isSit", false);
        coroutineRun = false;
    }
    void charMove() {
        if (!move)
            return;
        animator.SetTrigger("sitTrigger");
        animator.SetBool("isSit", false);
        animator.SetBool("isWalk", true);
        rigid.velocity = new Vector2(player.MaxSpeed, rigid.velocity.y);
        if (gameObject.transform.position.x >= targetPosX[index]) {
            move = false;
            animator.SetBool("isWalk", false);
            animator.SetBool("isSit", true);
        }
    }
}
