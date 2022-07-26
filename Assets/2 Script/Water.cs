using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour {
    [SerializeField]
    GameObject wood;

    int curWaterCnt;
    [SerializeField]
    int maxWaterCnt;

    float abilityDelay;
    float waterMoveTime;
    bool isLeft;

    Animator anim;
    // Start is called before the first frame update
    void Awake() {
        anim = GetComponent<Animator>();

        curWaterCnt = 0;
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
            progress += 0.005f;
            yield return null;
        }
        if (collActive) {
            wood.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
