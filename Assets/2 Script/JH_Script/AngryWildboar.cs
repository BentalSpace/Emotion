using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngryWildboar : MonoBehaviour
{
    [SerializeField]
    PlayerRenewal player;
    [SerializeField]
    BoxCollider2D attackBox;
    //[SerializeField]
    //Text hitText;

    [SerializeField]
    GameObject[] hitObject = new GameObject[3];
    // Sprite hitImage;

    bool isPlay;
    bool isClear;
    bool isHit;
    bool isChase;

    float randDis;
    float dis;
    [SerializeField]
    float maxHit;
    float nowHit;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        //Debug.DrawRay(transform.position + Vector3.down * 0.8f, Vector2.right * (spriteRenderer.flipX ? 4 : -4), Color.magenta);
    }
    void FixedUpdate()
    {
        if (isClear)
        {
            //passObject로 변경해서 더이상 플레이어와 작용하지 않게 함
            attackBox.gameObject.layer = 10;
            gameObject.layer = 10;
            return;
        }
        dis = Vector2.Distance(transform.position, player.transform.position);
        if (dis < 40)
        {
            //플레이

            rigid.velocity = new Vector2(randDis * 3, rigid.velocity.y);

            //앞에 벽이 있다면 반대로 이동
            RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * 1.5f, Vector2.right, (spriteRenderer.flipX ? 4 : -20), LayerMask.GetMask("Platform"));
            if (hit && !isChase)
            {
                randDis = spriteRenderer.flipX ? -1 : 1;
                spriteRenderer.flipX = !spriteRenderer.flipX;
                attackBox.offset = spriteRenderer.flipX ? new Vector2(0.68f, 1.57f) : new Vector2(-0.56f, 1.57f);
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
            attackBox.offset = new Vector2(0.68f, 1.57f);
        }
        else if (randDis <= -1)
        {
            spriteRenderer.flipX = false;
            attackBox.offset = new Vector2(-0.56f, 1.57f);
        }
        float playTime = Random.Range(2f, 4f);

        if (randDis == 0)
        {
            anim.SetTrigger("nose");
            playTime = 1;
        }
        else
        {
            anim.SetTrigger("run");
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * 1.5f, Vector3.right, (spriteRenderer.flipX ? 18 : -18), LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position + Vector3.up * 1.5f, Vector3.right * (spriteRenderer.flipX ? 18 : -18), Color.white);

        if (hit && !isHit)
        {
            isChase = true;
            switch (nowHit)
            {
                case 0:
                    randDis = (spriteRenderer.flipX ? 2.5f : -2.5f);
                    break;
                case 1:
                    randDis = (spriteRenderer.flipX ? 1.8f : -1.8f);
                    break;
                case 2:
                    randDis = (spriteRenderer.flipX ? 0.8f : -0.8f);
                    break;
                default:
                    randDis = (spriteRenderer.flipX ? 0f : 0f);
                    break;
            }
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("WildBoar run"))
                anim.SetTrigger("run");
        }
        else if (!hit)
        {
            isChase = false;
        }
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
        gameObject.transform.position = new Vector2(800, -5.8f);
    }

    void HitNextReady(float delayTime)
    {
        isHit = true;
        isPlay = false;
        CancelInvoke();
        nowHit++;
        randDis = 0;
        StartCoroutine(HitNextThinkOrChase(delayTime));
    }

    IEnumerator HitNextThinkOrChase(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        isHit = false;

        if (!isClear && !isChase)
            //추격중이 아니였다면, 생각을 하고,  || 추격중이였다면 하던 추격 계속 해라
            Think();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ThrowObject" && !isClear)
        {
            //퍼즐 해방 조건
            if (!isHit)
            {
                ObjectManager.Instance.ReturnObject(collision.gameObject, "angryBall");
                switch (nowHit)
                {
                    case 0:
                        HitNextReady(0.8f);
                        hitObject[0].SetActive(true);
                        anim.SetTrigger("stand2");
                        break;
                    case 1:
                        HitNextReady(1.2f);
                        hitObject[1].SetActive(true);
                        anim.SetTrigger("stand3");
                        break;
                    case 2:
                        HitNextReady(1.2f);
                        hitObject[2].SetActive(true);
                        anim.SetTrigger("stun");
                        break;
                    default:
                        Debug.Log("멧돼지 Hit 관련 부분 에러");
                        break;

                }
                if (maxHit <= nowHit)
                {
                    // 퍼즐 클리어
                    isClear = true;
                    Debug.Log("Clear");
                }
                Debug.Log(nowHit);
            }
        }
    }
}