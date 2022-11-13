using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour
{
    [SerializeField]
    PlayerRenewal player;

    bool isPlay;
    bool isClear;
    bool isHit;
    bool isChase;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    float randDis;
    float dis;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isClear)
        {
            //passObject로 변경해서 더이상 플레이어와 작용하지 않게 함
            gameObject.layer = 10;
            return;
        }

        dis = Vector2.Distance(transform.position, player.transform.position);

        if (dis < 40)
        {
            //플레이
            rigid.velocity = new Vector2(randDis * 3, rigid.velocity.y);

            //앞에 벽이 있다면 반대로 이동
            RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.forward, Vector2.right, (spriteRenderer.flipX ? 4 : -20), LayerMask.GetMask("Platform"));
            if (hit && !isChase)
            {
                randDis = spriteRenderer.flipX ? -1 : 1;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }

            if (!isPlay && !isHit && !isChase)
                Think();
        }
        else
        {
            CancelInvoke();
        }
        PlayerChase();
    }

    void Think()
    {
        isPlay = true;

        randDis = Random.Range(-1, 2);
        //randDis = 0; //테스트용

        if (randDis >= 1)
        {
            spriteRenderer.flipX = true;
        }
        else if (randDis <= -1)
        {
            spriteRenderer.flipX = false;
        }
        float playTime = Random.Range(2f, 4f);

        if (randDis == 0)
        {
            // anim.SetTrigger("Stand");
            playTime = 1;
        }
        else
        {
            anim.SetTrigger("Move");
        }

        Invoke("againThink", playTime);
    }

    void againThink()
    {
        isPlay = false;
    }

    void PlayerChase()
    {
        //앞에 플레이어가 있다면 플레이어를 향해 달림
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.forward, Vector3.right, (spriteRenderer.flipX ? 18 : -18), LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position + Vector3.forward * 1.5f, Vector3.right * (spriteRenderer.flipX ? 18 : -18), Color.white);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D playerRigid = collision.gameObject.GetComponent<Rigidbody2D>();

            player.Rigid.AddForce(new Vector2(randDis * 10, 10), ForceMode2D.Impulse);
            Invoke("StartPositionWarp", 2.0f);
        }
    }

    void StartPositionWarp()
    {
        gameObject.transform.position = new Vector2(887, 14);
    }
}
