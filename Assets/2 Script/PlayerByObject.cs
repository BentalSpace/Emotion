using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerByObject : MonoBehaviour
{
    [SerializeField]
    PlayerRenewal player;
    [SerializeField]
    CameraMove cameraMove;
    [SerializeField]
    Fairy fairy;
    [SerializeField]
    GameObject seedling;
    [SerializeField]
    GameObject downFairy;
    [SerializeField]
    GameObject seedlingDialouge;

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "animationStart" && !coroutineRun)
        {
            player.dontInput = true;
            StartCoroutine(plantCoroutine);
            Destroy(collision.gameObject);
        }
    }

    IEnumerator PlantATree() {
        animator.SetBool("isWalk", false);
        yield return new WaitForSeconds(1.0f);
        index = 0;
        coroutineRun = true;
        rigid.sharedMaterial = null;
        move = true;

        yield return new WaitForSeconds(1.0f);
        seedling.SetActive(true);
        index = 1;

        #region 요정 애니메이션 주석처리
        // yield return new WaitForSeconds(2f);
        // cameraMove.animMove = true;
        // yield return new WaitForSeconds(2f);
        // move = true;
        // yield return new WaitForSeconds(3f);
        // downFairy.SetActive(false);
        // fairy.animMove = true;
        #endregion

        yield return new WaitForSeconds(1.0f);
        player.dontInput = true;
        animator.SetBool("isWalk", false);

        yield return new WaitForSeconds(1.0f);
        player.dontInput = false;
        coroutineRun = false;
    }

    void charMove() {
        if (!move)
            return;
        animator.SetTrigger("sitTrigger");
        animator.SetBool("isSit", false);
        animator.SetBool("isWalk", true);
        rigid.velocity = new Vector2(player.ApplySpeed, rigid.velocity.y);
        if (gameObject.transform.position.x >= targetPosX[index]) {
            move = false;
            animator.SetBool("isWalk", false);
            animator.SetBool("isSit", true);
        }
    }
}
