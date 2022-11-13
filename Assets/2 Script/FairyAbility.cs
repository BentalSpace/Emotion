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
    // 슬픔 능력
    bool sadD;
    bool sading;
    // 공포 능력
    bool horrorD;
    bool horroring;
    // 분노 능력
    bool angryD;
    bool angrying;

    public bool isLeft;
    public bool Sading {
        get { return sading; }
    }

    GameObject waterBall;
    GameObject horrorBall;
    GameObject angryBall;

    GameObject arrow;
    Rigidbody2D waterRigid;
    Rigidbody2D horrorRigid;
    Rigidbody2D angryRigid;

    void Update() {
        SkillDownCheck();
        SadSkill();
        HorrorSkill();
        AngrySkill();
    }
    void SkillDownCheck() {
        if (!GameManager.manager.playerAbilityOn) {
            sadD = Input.GetButtonDown("Sad");
            horrorD = Input.GetButtonDown("Horror");
            angryD = Input.GetButtonDown("Angry");
        }
        else if(GameManager.manager.playerAbilityOn) {
            sadD = false;
            sading = false;
            horrorD = false;
            horroring = false;
            angryD = false;
            angrying = false;
        }
    }

    void SadSkill() {
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
                StartCoroutine(ReturnBall(waterBall, "waterBall"));
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
    void HorrorSkill() {
        if (!horroring && (player.abilityCurGauge >= player.abilityMaxGauge || GameManager.manager.haveHorrorMask)) {
            if (horrorD) {
                player.dontInput = true;
                if (!GameManager.manager.haveHorrorMask) {
                    player.abilityCurGauge = 0;
                }
                horrorBall = ObjectManager.Instance.GetObject("horrorBall");
                horrorBall.transform.position = transform.position;
                horrorBall.tag = "Untagged";
                arrow = ObjectManager.Instance.GetObject("arrow");
                arrow.transform.position = transform.position;

                horrorRigid = horrorBall.GetComponent<Rigidbody2D>();
                maxRotation = 80;
                minRotation = 30;
                rotation = minRotation;
                if (!player.FlipX) {
                    horrorBall.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                    isLeft = false;
                }
                else if (player.FlipX) {
                    horrorBall.transform.localEulerAngles = new Vector3(0, 0, rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, rotation);
                    isLeft = true;
                }
                horroring = true;
            }
        }
        else if (horroring) {
            if (horrorD) {
                horrorBall.tag = "ThrowObject";
                horrorRigid.bodyType = RigidbodyType2D.Dynamic;
                horrorRigid.AddRelativeForce(transform.up * 20, ForceMode2D.Impulse);
                StartCoroutine(ReturnBall(horrorBall, "horrorBall"));
                ObjectManager.Instance.ReturnObject(arrow, "arrow");
                horroring = false;
                player.dontInput = false;
            }
        }

        if (horroring) {
            if (!isLeft) {
                if (Input.GetKey(KeyCode.UpArrow)) {
                    rotation = rotation > minRotation ? rotation - (30 * Time.deltaTime) : rotation = minRotation;
                    horrorBall.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                }
                if (Input.GetKey(KeyCode.DownArrow)) {
                    rotation = rotation < maxRotation ? rotation + (30 * Time.deltaTime) : rotation = maxRotation;
                    horrorBall.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                }
            }
            else {
                if (Input.GetKey(KeyCode.UpArrow)) {
                    rotation = rotation > minRotation ? rotation - (30 * Time.deltaTime) : rotation = minRotation;
                    horrorBall.transform.localEulerAngles = new Vector3(0, 0, rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, rotation);
                }
                if (Input.GetKey(KeyCode.DownArrow)) {
                    rotation = rotation < maxRotation ? rotation + (30 * Time.deltaTime) : rotation = maxRotation;
                    horrorBall.transform.localEulerAngles = new Vector3(0, 0, rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, rotation);
                }
            }
        }
    }
    void AngrySkill()
    {
        if (!angrying && (player.abilityCurGauge >= player.abilityMaxGauge || GameManager.manager.haveAngryMask))
        {
            if (angryD)
            {
                player.dontInput = true;
                if (!GameManager.manager.haveAngryMask)
                {
                    player.abilityCurGauge = 0;
                }
                angryBall = ObjectManager.Instance.GetObject("angryBall");
                angryBall.transform.position = transform.position;
                angryBall.tag = "Untagged";
                arrow = ObjectManager.Instance.GetObject("arrow");
                arrow.transform.position = transform.position;

                angryRigid = angryBall.GetComponent<Rigidbody2D>();
                maxRotation = 80;
                minRotation = 30;
                rotation = minRotation;
                if (!player.FlipX)
                {
                    angryBall.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                    isLeft = false;
                }
                else if (player.FlipX)
                {
                    angryBall.transform.localEulerAngles = new Vector3(0, 0, rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, rotation);
                    isLeft = true;
                }
                angrying = true;
            }
        }
        else if (angrying)
        {
            if (angryD)
            {
                angryBall.tag = "ThrowObject";
                angryRigid.bodyType = RigidbodyType2D.Dynamic;
                angryRigid.AddRelativeForce(transform.up * 20, ForceMode2D.Impulse);
                StartCoroutine(ReturnBall(waterBall, "angryBall"));
                ObjectManager.Instance.ReturnObject(arrow, "arrow");
                angrying = false;
                player.dontInput = false;
            }
        }
        if (angrying)
        {
            if (!isLeft)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    rotation = rotation > minRotation ? rotation - (30 * Time.deltaTime) : rotation = minRotation;
                    angryBall.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    rotation = rotation < maxRotation ? rotation + (30 * Time.deltaTime) : rotation = maxRotation;
                    angryBall.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, -rotation);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    rotation = rotation > minRotation ? rotation - (30 * Time.deltaTime) : rotation = minRotation;
                    angryBall.transform.localEulerAngles = new Vector3(0, 0, rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, rotation);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    rotation = rotation < maxRotation ? rotation + (30 * Time.deltaTime) : rotation = maxRotation;
                    angryBall.transform.localEulerAngles = new Vector3(0, 0, rotation);
                    arrow.transform.localEulerAngles = new Vector3(0, 0, rotation);
                }
            }
        }
    }
    IEnumerator ReturnBall(GameObject obj, string name) {
        yield return new WaitForSeconds(10f);
        ObjectManager.Instance.ReturnObject(obj, name);
    }
}
