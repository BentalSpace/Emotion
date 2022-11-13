using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Spider : MonoBehaviour
{
    [SerializeField]
    GameObject web;
    GameObject webPool;
    [SerializeField, Tooltip("������ ��� �ִ�Ÿ�")]
    float atkDistance;

    bool isRunaway;
    bool isUp;
    bool doingUp;
    float runTime;

    float atkDelay;

    Transform playerTr;
    public AnimationCurve curve;
    Animator anim;
    void Awake() {
        playerTr = GameObject.Find("player").transform;
        anim = GetComponent<Animator>();
    }
    void Start() {
        webPool = Instantiate(web, transform);
        webPool.SetActive(false);
    }
    public void Runaway() {
        anim.SetBool("isRunaway", true);
        // y 10���� �̵�
        isRunaway = true;
    }
    void Update()
    {
        DoingRunaway();
        atkDelay += Time.deltaTime;
        float dis = transform.position.x - playerTr.position.x;

        // �����̰� �ư�, ���� �ȿ� ���Դٸ� �߻� �غ�
        if (dis < atkDistance && dis > 0) {
            // ���ʸ� ����
            if(atkDelay > 2.5f) {
                anim.SetBool("isAtk", true);
                atkDelay = 0;
            }
        }
        // ���� �ִϸ��̼� ���� �� ź �߻�
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Spider_Attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.55f) {
            webPool.transform.localPosition = new Vector2(-3.5f, 3);

            webPool.SetActive(true);
            webPool.GetComponent<SpiderWeb>().Fire(playerTr, curve);
            anim.SetBool("isAtk", false);
        }
    }
    void DoingRunaway() {
        if (isRunaway) {
            runTime += Time.deltaTime;
            if (!isUp) {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Spider_Runaway") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f && !isUp) {
                    isUp = true;
                    doingUp = true;
                }
            }
            if (isUp && doingUp) {
                RunawayUp();
            }
            else if(runTime > 7f) {
                // �������� �ð�
                RunawayDown();
            }
        }
    }
    void RunawayUp() {
        Vector2 vec = transform.position;
        vec.y = 10.1f;
        transform.position = Vector2.Lerp(transform.position, vec, Time.deltaTime * 1.5f);
        if(Vector2.Distance(transform.position, vec) < 0.1f) {
            doingUp = false;
        }
    }
    void RunawayDown() {
        // -5.6���� ��������
        //Debug.Log("down");
        Vector2 vec = transform.position;
        vec.y = -5.7f;
        transform.position = Vector2.Lerp(transform.position, vec, Time.deltaTime * 2f);
        if(Vector2.Distance(transform.position, vec) < 0.1f) {
            isRunaway = false;
            runTime = 0;
            anim.SetBool("isRunaway", false);
            anim.SetBool("isAtk", false);
            isUp = false;
        }
    }
}
