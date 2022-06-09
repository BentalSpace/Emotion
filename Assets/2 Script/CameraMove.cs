using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    float leftMax;
    [SerializeField]
    float rightMax;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Transform animVec;


    //카메라 기준 포지션
    [SerializeField]
    float cameraYMin;
    [SerializeField]
    float cameraYMax;
    //플레이어 기준 포지션
    [SerializeField]
    float startHighMove;

    public bool animMove;
    public bool AnimationProgress;

    Vector3 cameraPos;
    void Awake() {
        AnimationProgress = false;
    }
    void FixedUpdate() {
        if (!AnimationProgress) {
            cameraMove();
            highMapMove();
        }
        zoomMove();
    }

    void cameraMove() {
        if (animMove)
            return;
        if (player.transform.position.x <= leftMax) {
            cameraPos = new Vector3(leftMax, transform.position.y, transform.position.z);
        }
        else if (player.transform.position.x >= rightMax) {
            cameraPos = new Vector3(rightMax, transform.position.y, transform.position.z);
        }
        else {
            cameraPos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }
        transform.position = cameraPos;
    }
    void highMapMove() {
        if (!GameManager.manager.thisHighMap)
            return;
        if(player.transform.position.y > startHighMove) {
            if (cameraYMax >= transform.position.y && cameraYMin <= transform.position.y) {
                cameraPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
                transform.position = cameraPos;
            }
        }

        //카메라가 필요이상 아래로 갔을때
        if (cameraYMin > transform.position.y) {
            cameraPos = new Vector3(transform.position.x, cameraYMin, transform.position.z);
            transform.position = cameraPos;
        }
        //카메라가 필요이상 위로 갔을때
        if(cameraYMax < transform.position.y) {
            cameraPos = new Vector3(transform.position.x, cameraYMax, transform.position.z);
            transform.position = cameraPos;
        }
    }
    void Jump() {

    }

    void zoomMove() {
        if (!animMove)
            return;
        cameraPos = new Vector3(animVec.position.x, animVec.position.y, transform.position.z);
        transform.Translate(cameraPos * 0.001f);

        if (transform.position.x >= animVec.position.x) {
            cameraPos = new Vector3(animVec.position.x, transform.position.y, transform.position.z);
            transform.position = cameraPos;
            StartCoroutine(delay());
        }
    }

    IEnumerator delay() {
        yield return new WaitForSeconds(4f);
        animMove = false;
    }
}
