using UnityEngine;
using System.Collections;
using CafofoStudio;

public class bat : MonoBehaviour {
    Transform player;

    [SerializeField]
    float movePos1;
    [SerializeField]
    float movePos2;
    [SerializeField]
    float stopDelay;
    [SerializeField]
    float shotDelay;
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject poop;

    float curStopDelay;
    float curShotDelay;

    bool isMove;

    CaveAmbientMixer caveMixer;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer renderer;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();

        player = GameObject.Find("player").transform;
        caveMixer = GameObject.Find("CaveAmbience").GetComponent<CaveAmbientMixer>();
    }
    void Start() {
        SoundManager.instance.BatSoundCheck += this.DistanceCheck;
    }
    void DistanceCheck() {
        float dis = player.position.x - transform.position.x > 0 ? player.position.x - transform.position.x : (player.position.x - transform.position.x) * -1f;
        if (dis < 30) {
            SoundManager.instance.batSoundCnt++;
            // ����� �Ÿ� ���
            if(SoundManager.instance.batSoundCnt == 1) {
                SoundManager.instance.batDistance = dis;
            }
            else {
                if(SoundManager.instance.batDistance > dis) {
                    SoundManager.instance.batDistance = dis;
                }
            }
        }
    }
    void Update() {
        if (!isMove) {
            // ������϶���, ��ž�����̸� ä��
            curStopDelay += Time.deltaTime;
        }
        if (isMove) {
            //�����̰� ��������, ���ݵ����̸� ä��
            curShotDelay += Time.deltaTime;
        }
    }
    void FixedUpdate() {
        MoveStart();
        Move();
        Attack();
    }
    void MoveStart() {
        if (curStopDelay >= stopDelay) {
            //������1���� ������2�� �̵�
            anim.SetBool("isFly", true);
            curStopDelay = 0;
            isMove = true;
        }
    }
    void Move() {
        if (isMove) {
            if (movePos1 < movePos2) {
                rigid.velocity = new Vector2(speed, 0);

                if (gameObject.transform.position.x >= movePos2) {
                    MoveEnd();
                    renderer.flipX = true;
                }
            }
            else {
                rigid.velocity = new Vector2(-speed, 0);

                if (gameObject.transform.position.x <= movePos2) {
                    MoveEnd();
                    renderer.flipX = false;
                }
            }
        }
    }
    void MoveEnd() {
        //��ǥ�� ������ �����ߴٸ� Move����
        isMove = false;
        anim.SetBool("isFly", false);
        float temp = movePos1;
        movePos1 = movePos2;
        movePos2 = temp;
        rigid.velocity = Vector2.zero;
    }
    void Attack() {
        if (curShotDelay >= shotDelay) {
            // ����
            curShotDelay = 0;
            GameObject poop = ObjectManager.Instance.GetObject("batPoop");
            poop.transform.position = gameObject.transform.position;

            StartCoroutine(PoopOut(poop));
            //Instantiate(poop, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
    IEnumerator PoopOut(GameObject poop) {
        yield return new WaitForSeconds(2f);
        if (poop.activeSelf) {
            ObjectManager.Instance.ReturnObject(poop, "batPoop");
        }
    }
}
