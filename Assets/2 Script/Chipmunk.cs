using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chipmunk : MonoBehaviour
{
    [SerializeField]
    int StartAnim;
    [SerializeField]
    GameObject parentObject;
    [SerializeField]
    GameObject targetObject;
    [SerializeField]
    Sprite needSprite;
    [SerializeField]
    int sceneNumber;


    bool isCoroutineStart;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    void Awake() {
        rigid = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();

        switch (StartAnim) {
            case 0:
                anim.SetTrigger("stun");
                break;
            case 1:
                anim.SetTrigger("stand");
                break;
        }
    }
    public void CutScene(PlayerRenewal player) {
        switch (sceneNumber) {
            case 1:
                if(!isCoroutineStart)
                    StartCoroutine(firstCutScene(player));
                break;
        }
    }
    public void TargetObjectSpriteChange(bool isPlayerFront, Vector2 changeScale, Vector2 changeColliderOffset, Vector2 changeColliderSize) {
        SpriteRenderer targetSprite = targetObject.GetComponent<SpriteRenderer>();
        targetSprite.sprite = needSprite;
        if (isPlayerFront) {
            targetSprite.sortingOrder = 21;
        }
        if (changeScale != Vector2.zero) {
            targetObject.transform.localScale = changeScale;
            BoxCollider2D collider = targetObject.GetComponent<BoxCollider2D>();
            collider.offset = changeColliderOffset;
            collider.size = changeColliderSize;
        }
    }
    public void AcornSitPassObj() {
        targetObject.layer = 11;
    }
    IEnumerator firstCutScene(PlayerRenewal player) {
        isCoroutineStart = true;

        player.dontInput = true;
        anim.SetTrigger("stand");
        yield return new WaitForSeconds(1f);
        spriteRenderer.flipX = true;

        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("run");
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector3.right * 5, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.6f);
        rigid.velocity = Vector2.zero;
        rigid.AddForce(new Vector2(5, 5), ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.5f);
        rigid.velocity = Vector2.zero;
        rigid.AddForce(new Vector2(8, 6), ForceMode2D.Impulse);

        yield return new WaitForSeconds(2f);
        anim.SetTrigger("stand");
        parentObject.transform.position = new Vector2(551, -5);
        targetObject.layer = 10;

        SpriteRenderer targetSprite = targetObject.GetComponent<SpriteRenderer>();
        targetSprite.sprite = needSprite;
        targetSprite.sortingOrder = 21;
        targetObject.transform.localScale = new Vector2(2f, 2f);

        player.dontInput = false;

        isCoroutineStart = false;
    }
}
