using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    PlayerRenewal playerRenewal;

    public bool animMove;
    bool trigerStart;
    bool firstPos;

    SpriteRenderer playerSpriteRenderer;

    Vector2 playerNeedPos;

    Animator animator;
    void Awake() {
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        firstPos = true;
        trigerStart = false;
    }
    void FixedUpdate() {
        //Vector3 targetVec = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f);
        //transform.RotateAround(targetVec, Vector3.back, 3);
        //transform.rotation = Quaternion.identity;
        Move();
        //AnimMove();
    }
    void Move() {
        if (animMove)
            return;
        playerNeedPos = new Vector2(player.transform.position.x, 0);
        if (playerSpriteRenderer.flipX) {
            transform.position = new Vector2(1.5f, transform.position.y) + playerNeedPos;
        }
        else {
            transform.position = new Vector2(-1.5f, transform.position.y) + playerNeedPos;
        }
    }
    void AnimMove() {
        if (!animMove)
            return;
        Debug.Log("TEST");
        if (firstPos) {
            transform.position = new Vector2(166f, -5.5f);
            firstPos = false;
        }
        //Vector3 targetVec = new Vector3(transform.parent.position.x - 3f, 1f);
        //Vector3 dirVec = targetVec - transform.position;

        //transform.Translate(dirVec * 0.01f);
        if (!trigerStart) {
            animator.SetTrigger("prologueGetup");
            trigerStart = true;
        }
        //if(transform.localPosition.x <= -1.5f) {
        //    animMove = false;
        //}
    }
    public IEnumerator AnimMove1() {
        animMove = true;
        transform.localPosition = new Vector3(0.5f, 0f);
        animator.SetTrigger("prologueGetup");
        yield return new WaitForSeconds(1f);
        animMove = false;
    }
}
