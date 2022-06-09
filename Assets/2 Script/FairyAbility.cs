using UnityEngine;
using System.Collections;

public class FairyAbility : MonoBehaviour {
    [SerializeField]
    PlayerRenewal player;
    [SerializeField]
    GameObject sadAbility;
    [SerializeField]
    GameObject arrowObj;

    float maxRotation;
    float minRotation;
    float rotation;
    bool sadD;
    bool sading;
    public bool isLeft;
    public bool Sading {
        get { return sading; }
    }

    GameObject waterBall;
    GameObject arrow;
    Rigidbody2D waterRigid;

    void Update() {
        SkillDownCheck();
        Skill();
    }
    void SkillDownCheck() {
        if (!GameManager.manager.playerAbilityOn) {
            sadD = Input.GetButtonDown("Sad");
        }
        else if(GameManager.manager.playerAbilityOn) {
            sadD = false;
            sading = false;
        }
    }
    void Skill() {
        //슬픔 요정능력
        if (!sading && (player.abilityCurGauge >= player.abilityMaxGauge || GameManager.manager.haveSadMask)) {
            if (sadD) {
                player.dontInput = true;
                if (!GameManager.manager.haveSadMask) {
                    player.abilityCurGauge = 0;
                }
                waterBall = ObjectManager.Instance.GetObject("waterBall");
                waterBall.transform.position = transform.position;
                waterBall.tag = "Untagged";
                arrow = ObjectManager.Instance.GetObject("arrow");
                arrow.transform.position = transform.position;

                waterRigid = waterBall.GetComponent<Rigidbody2D>();
                maxRotation = 80;
                minRotation = 30;
                rotation = minRotation;
                if (!player.FlipX) {
                    waterBall.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                    isLeft = false;
                }
                else if (player.FlipX) {
                    waterBall.transform.localEulerAngles = new Vector3(0, 0, rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, rotation);
                    isLeft = true;
                }
                sading = true;
            }
        }
        else if (sading) {
            if (sadD) {
                waterBall.tag = "ThrowObject";
                waterRigid.bodyType = RigidbodyType2D.Dynamic;
                waterRigid.AddRelativeForce(transform.up * 20, ForceMode2D.Impulse);
                StartCoroutine(ReturnWaterBall(waterBall));
                ObjectManager.Instance.ReturnObject(arrow, "arrow");
                sading = false;
                player.dontInput = false;
            }
        }
        if (sading) {
            if (!isLeft) {
                if (Input.GetKey(KeyCode.UpArrow)) {
                    rotation = rotation > minRotation ? rotation - (30 * Time.deltaTime) : rotation = minRotation;
                    waterBall.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                }
                if (Input.GetKey(KeyCode.DownArrow)) {
                    rotation = rotation < maxRotation ? rotation + (30 * Time.deltaTime) : rotation = maxRotation;
                    waterBall.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                }
            }
            else {
                if (Input.GetKey(KeyCode.UpArrow)) {
                    rotation = rotation > minRotation ? rotation - (30 * Time.deltaTime) : rotation = minRotation;
                    waterBall.transform.localEulerAngles = new Vector3(0, 0, rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, rotation);
                }
                if (Input.GetKey(KeyCode.DownArrow)) {
                    rotation = rotation < maxRotation ? rotation + (30 * Time.deltaTime) : rotation = maxRotation;
                    waterBall.transform.localEulerAngles = new Vector3(0, 0, rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, rotation);
                }
            }
        }
    }

    IEnumerator ReturnWaterBall(GameObject obj) {
        yield return new WaitForSeconds(10f);
        ObjectManager.Instance.ReturnObject(obj, "waterBall");
    }
}
