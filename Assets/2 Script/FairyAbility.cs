using UnityEngine;

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
        if (!GameManager.playerAbilityOn) {
            sadD = Input.GetButtonDown("Sad");
        }
        else if(GameManager.playerAbilityOn) {
            sadD = false;
            sading = false;
        }
    }
    void Skill() {
        if (!sading) {
            if (sadD) {
                player.dontInput = true;
                waterBall = Instantiate(sadAbility, transform.position, transform.rotation);
                arrow = Instantiate(arrowObj, transform.position, transform.rotation);
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
                Destroy(arrow);
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

}
