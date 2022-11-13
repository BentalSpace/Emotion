using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenNoise : MonoBehaviour
{
    // 켜지는 시간 관리
    float turnOnCurTime;
    float turnOnMaxTime;
    bool alreadyRun;

    // 움직이는 효과?
    float moveCurTime;
    float moveMaxTime;

    RectTransform rect;
    void Awake() {
        rect = GetComponent<RectTransform>();
    }
    void Update()
    {
        turnOnCurTime += Time.deltaTime;
        moveCurTime += Time.deltaTime;
        if(turnOnCurTime > turnOnMaxTime) {
            turnOnCurTime = 0;
            turnOnMaxTime = Random.Range(0.2f, 0.5f);
            alreadyRun = false;
            gameObject.SetActive(false);
        }
        if(moveCurTime > moveMaxTime) {
            moveCurTime = 0;
            moveMaxTime = Random.Range(0.1f, 0.15f);
            float randX = Random.Range(400, -400);
            float randY = Random.Range(200, -200);
            rect.offsetMin = new Vector2(randX, randY);
            rect.offsetMax = new Vector2(randX, randY);
        }
    }
}
