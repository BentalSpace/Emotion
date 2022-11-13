using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    [SerializeField]
    float spawnDelay;
    [SerializeField]
    float shotDelay;

    Transform initialTransform;

    float curDelay;

    bool isSpawn;
    bool spawning;
    bool spawnStart;

    Rigidbody2D rigid;
    SpriteRenderer sprite;
    CircleCollider2D col;
    void Awake()
    {
        initialTransform = gameObject.transform.parent;
        //spawnDelay = 1.5f;
        //shotDelay = 2.0f;
        isSpawn = true;

        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
    }
    void Update()
    {
        if (!spawning) // 종유석이 자라는 중이라면 딜레이를 채우지 않는다.
            curDelay += Time.deltaTime;
    }
    void FixedUpdate()
    {
        StalactiteSpawn();
        StalactiteShot();
    }

    void StalactiteSpawn()
    {
        if (!isSpawn)
        {
            if (curDelay > spawnDelay)
            {
                //종유석 스폰
                curDelay = 0;
                spawning = true;
                spawnStart = true;
            }
        }

        if (spawning)
        {
            if (spawnStart)
            {
                // 종유석을 초기 포지션의 살짝 위로 스폰
                gameObject.transform.position = new Vector2(initialTransform.position.x, initialTransform.position.y + 1.0f);
                gameObject.SetActive(true);
                sprite.enabled = true;
                col.enabled = true;
                spawnStart = false;
            }
            // 스폰한 위치에서 아래로 내리면서 살짝 자라는듯한 느낌
            rigid.velocity = new Vector2(0, -1.0f);

            //y포지션이 초기 포지션에 도달하면 스폰 끝
            if (gameObject.transform.position.y <= initialTransform.position.y)
            {
                rigid.velocity = Vector2.zero;
                spawning = false;
                isSpawn = true;
            }
        }
    }
    void StalactiteShot()
    {
        if (isSpawn)
        {
            if (curDelay > shotDelay)
            {
                //종유석 발사
                StartCoroutine(StalactiteShotAndInit());
            }
        }
    }
    IEnumerator StalactiteShotAndInit()
    {
        curDelay = 0;
        rigid.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(spawnDelay);
        isSpawn = false;
        rigid.velocity = Vector2.zero;
        rigid.bodyType = RigidbodyType2D.Kinematic;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //플레이어에 닿으면
            if (!PlayerRenewal.Horroring)
            {
                isSpawn = false;
                StartCoroutine(collision.gameObject.GetComponent<PlayerRenewal>().Die());
                rigid.bodyType = RigidbodyType2D.Kinematic;
            }
        }
        if (collision.gameObject.layer == 7)
        {
            //플랫폼에 닿으면
            isSpawn = false;
            rigid.bodyType = RigidbodyType2D.Kinematic;
            col.enabled = false;
            sprite.enabled = false; // 스프라이트만 지우면 trigger는 그대로 남아있기 때문에, gameobject를 지우면?
            rigid.velocity = Vector2.zero;
        }
    }
}
