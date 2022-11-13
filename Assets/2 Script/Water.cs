using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour {
    Transform player;
    [SerializeField]
    GameObject wood;

    int curWaterCnt;
    [SerializeField]
    int maxWaterCnt;

    float abilityDelay;
    float waterMoveTime;
    bool isLeft;

    Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
        player = GameObject.Find("player").transform;

        curWaterCnt = 0;
    }
    void Start() {
        SoundManager.instance.waterSoundCheck += this.DistanceCheck;
    }
    void DistanceCheck() {
        float dis = player.position.x - transform.position.x > 0 ? player.position.x - transform.position.x : (player.position.x - transform.position.x) * -1f;
        if (dis < 20) {
            SoundManager.instance.waterSoundCnt++;
            if(SoundManager.instance.waterSoundCnt == 1) {
                SoundManager.instance.waterDistance = dis;
            }
            else {
                if(SoundManager.instance.waterDistance > dis) {
                    SoundManager.instance.waterDistance = dis;
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        waterMoveTime += Time.deltaTime;
        abilityDelay += Time.deltaTime;

        if (waterMoveTime >= 0.6f) {
            isLeft = !isLeft;
            waterMoveTime = 0;
        }
        if (isLeft)
            transform.position = Vector2.MoveTowards(gameObject.transform.position, gameObject.transform.position + Vector3.left, 2 * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(gameObject.transform.position, gameObject.transform.position + Vector3.right, 2 * Time.deltaTime);
    }
    void FixedUpdate() {
    }
    public void WaterAbility() {
        if (maxWaterCnt <= curWaterCnt) {
            return;
        }
        if (abilityDelay < 0.3f)
            return;
        curWaterCnt++;
        abilityDelay = 0;
        switch (maxWaterCnt) {
            case 1:
                anim.SetTrigger("OneTimeFill");
                UpWood(maxWaterCnt, curWaterCnt);
                break;
            case 2:
                anim.SetTrigger("Fill");
                UpWood(maxWaterCnt, curWaterCnt);
                break;
        }
    }
    void UpWood(int maxCnt, int curCnt) {
        if (!wood)
            return;
        float x = wood.transform.localPosition.x;
        float y = wood.transform.localPosition.y;

        if(maxCnt == 1) {
            StartCoroutine(WoodCoroutine(x, y, 2.2f, true));
        }
        else if (maxCnt == 2) {
            switch (curCnt) {
                case 1:
                    StartCoroutine(WoodCoroutine(x, y, 1.7f, false));
                    break;
                case 2:
                    StartCoroutine(WoodCoroutine(x, y, 2.2f, true));
                    break;
            }
        }

    }
    IEnumerator WoodCoroutine(float x, float y, float targetY, bool collActive) {
        float progress = 0;
        while (progress < 1) {
            wood.transform.localPosition = Vector2.Lerp(new Vector2(x, y), new Vector2(x, targetY), progress);
            progress += 0.05f;
            yield return new WaitForSeconds(0.02f);
        }
        if (collActive) {
            wood.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
