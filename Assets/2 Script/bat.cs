using UnityEngine;

public class bat : MonoBehaviour {
    [SerializeField]
    float movePos1;
    [SerializeField]
    float movePos2;
    [SerializeField]
    float stopDelay;
    [SerializeField]
    float shotDelay;
    [SerializeField]
    GameObject poop;

    float curStopDelay;
    float curShotDelay;

    bool isMove;

    Rigidbody2D rigid;
    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update() {
        Debug.Log(isMove);
        if (!isMove) {
            curStopDelay += Time.deltaTime;
        }
        if (isMove) {
            curShotDelay += Time.deltaTime;
        }
    }
    void FixedUpdate() {
        if (curStopDelay >= stopDelay) {
            //포지션1에서 포지션2로 이동
            curStopDelay = 0;
            isMove = true;
        }
        if (curShotDelay >= shotDelay) {
            // 공격
            curShotDelay = 0;
            Instantiate(poop, gameObject.transform.position, gameObject.transform.rotation);
        }
        if (isMove) {
            if (movePos1 < movePos2) {
                rigid.velocity = new Vector2(5.0f, 0);

                if (gameObject.transform.position.x >= movePos2) {
                    isMove = false;
                    float temp = movePos1;
                    movePos1 = movePos2;
                    movePos2 = temp;
                    rigid.velocity = Vector2.zero;
                }
            }
            else {
                rigid.velocity = new Vector2(-5.0f, 0);
                if (gameObject.transform.position.x <= movePos2) {
                    isMove = false;
                    float temp = movePos1;
                    movePos1 = movePos2;
                    movePos2 = temp;
                    rigid.velocity = Vector2.zero;
                }
            }
        }
    }
}
