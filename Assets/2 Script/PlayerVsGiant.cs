using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVsGiant : MonoBehaviour
{
    bool isHide;
    public bool isStart;

    [SerializeField]
    Image noiseImg;
    float noiseCurTime;
    float noiseMaxTime;

    public bool IsStart { get { return isStart; } }

    public bool IsHide { get { return isHide; } }

    PlayerRenewal player;
    Giant giant;
    public void Init() {
        isStart = true;
        isHide = true;
    }
    void Awake() {
        player = GetComponent<PlayerRenewal>();
        giant = GameObject.Find("Giant").GetComponent<Giant>();
        isHide = false;
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("EventStart")) {
            if (collision.transform.position.x >= 890)
            {
                // 마지막 추격전
                // 거인 x879 스폰 ~> 이동
                giant.gameObject.SetActive(true);
                giant.LastChaseStart();
                isHide = false;
            }
            else
            {
                isHide = true;
                isStart = true;
            }
        }
        if (collision.CompareTag("EventEnd"))
        {
            isHide = true;
            isStart = false;
            giant.EventEndHide();
        }

        if (!isStart)
            return;
        if (collision.CompareTag("Pillar")) {
            isHide = true;
            giant.playerHidePillar = collision.transform;
        }
    }
    void OnTriggerExit2D(Collider2D collision) {
        if (!isStart)
            return;
        if (collision.CompareTag("Pillar")) {
            isHide = false;
            giant.playerHidePillar = null;
        }
    }
    public void GiantApproaching() {
        noiseCurTime += Time.deltaTime;
        if(noiseCurTime > noiseMaxTime) {
            noiseImg.gameObject.SetActive(true);
            noiseCurTime = 0;
            noiseMaxTime = Random.Range(1, 2.5f);
        }
    }
    public void NotGiantApproaching() {
    }
    public void PlayerDie() {
        isHide = true;
        StartCoroutine(player.Die());
    }

    public void EnvironmentPlayerDie()
    {
        isHide = true;
        giant.EnvironmentPlayerDie();
    }
}
