using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBoar : MonoBehaviour
{
    [SerializeField]
    PlayerRenewal player;
    [SerializeField]
    BoxCollider2D attackBox;

    bool isPlay;
    bool isClear;

    float randDis;
    float dis;
    [SerializeField]
    float maxHit;
    float nowHit;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;
    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update() {
        //Debug.DrawRay(transform.position + Vector3.down * 0.8f, Vector2.right * (spriteRenderer.flipX ? 4 : -4), Color.magenta);
    }
    void FixedUpdate() {
        if (isClear) {
            //passObject로 변경해서 더이상 플레이어와 작용하지 않게 함
            attackBox.gameObject.layer = 10;
            gameObject.layer = 10;
            return;
        }
        dis = Vector2.Distance(transform.position, player.transform.position);
        if (dis < 40) {
            //플레이

            rigid.velocity = new Vector2(randDis * 3, rigid.velocity.y);

            RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down * 0.8f, Vector2.right, (spriteRenderer.flipX ? 4 : -4), LayerMask.GetMask("Platform"));

            if (hit) {
                randDis = spriteRenderer.flipX ? -1 : 1;
                spriteRenderer.flipX = !spriteRenderer.flipX;
                attackBox.offset = spriteRenderer.flipX ? new Vector2(0.68f, 0.1f) : new Vector2(-0.56f, 0.1f);
            }

            if (!isPlay)
                Think();
        }
        else {
            CancelInvoke();
        }
        PlayerChase();
    }
    void Think() {
        isPlay = true;

        randDis = Random.Range(-1, 2);
        //randDis = 0; //테스트용

        if (randDis >= 1) {
            spriteRenderer.flipX = true;
            attackBox.offset = new Vector2(0.68f, 0.1f);
        }
        else if(randDis <= -1) {
            spriteRenderer.flipX = false;
            attackBox.offset = new Vector2(-0.56f, 0.1f);
        }
        float playTime = Random.Range(2f, 4f);

        if (randDis == 0) {
            anim.SetTrigger("nose");
            playTime = 1;
        }
        else {
            anim.SetTrigger("run");
        }

        Invoke("againThink", playTime);
    }
    void againThink() {
        isPlay = false;
    }
    void PlayerChase() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down * 0.8f, Vector3.right, (spriteRenderer.flipX ? 10 : -10), LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position + Vector3.down * 0.8f, Vector3.right * (spriteRenderer.flipX ? 10 : -10), Color.white);

        if (hit) {
            randDis = (spriteRenderer.flipX ? 1.5f : -1.5f);
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("WildBoar run"))
                anim.SetTrigger("run");
        }
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player") {
            Rigidbody2D playerRigid = collision.gameObject.GetComponent<Rigidbody2D>();

            player.Rigid.AddForce(new Vector2(randDis * 10, 10), ForceMode2D.Impulse);
        }
        
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "ThrowObject") {
            //퍼즐 해방 조건
            Destroy(collision.gameObject);
            if (maxHit <= ++nowHit) {
                anim.SetTrigger("nose");
                isClear = true;
            }
        }
    }
}
