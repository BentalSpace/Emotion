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
        if (!spawning) // �������� �ڶ�� ���̶�� �����̸� ä���� �ʴ´�.
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
                //������ ����
                curDelay = 0;
                spawning = true;
                spawnStart = true;
            }
        }

        if (spawning)
        {
            if (spawnStart)
            {
                // �������� �ʱ� �������� ��¦ ���� ����
                gameObject.transform.position = new Vector2(initialTransform.position.x, initialTransform.position.y + 1.0f);
                gameObject.SetActive(true);
                sprite.enabled = true;
                col.enabled = true;
                spawnStart = false;
            }
            // ������ ��ġ���� �Ʒ��� �����鼭 ��¦ �ڶ�µ��� ����
            rigid.velocity = new Vector2(0, -1.0f);

            //y�������� �ʱ� �����ǿ� �����ϸ� ���� ��
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
                //������ �߻�
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
            //�÷��̾ ������
            if (!PlayerRenewal.Horroring)
            {
                isSpawn = false;
                StartCoroutine(collision.gameObject.GetComponent<PlayerRenewal>().Die());
                rigid.bodyType = RigidbodyType2D.Kinematic;
            }
        }
        if (collision.gameObject.layer == 7)
        {
            //�÷����� ������
            isSpawn = false;
            rigid.bodyType = RigidbodyType2D.Kinematic;
            col.enabled = false;
            sprite.enabled = false; // ��������Ʈ�� ����� trigger�� �״�� �����ֱ� ������, gameobject�� �����?
            rigid.velocity = Vector2.zero;
        }
    }
}
