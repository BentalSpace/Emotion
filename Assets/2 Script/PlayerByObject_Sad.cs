using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerByObject_Sad : MonoBehaviour
{
    [SerializeField]
    private GameObject seedling;

    private float eightFrame;

    public bool isSkillRange;

    void Awake() {
        eightFrame = 8;
    }

    void Update() {
        if (isSkillRange) {
            if (Input.GetButtonDown("Sad")) {
                StartCoroutine(treeGrowing());
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "skill") {
            isSkillRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "skill") {
            isSkillRange = false;
        }
    }

    IEnumerator treeGrowing() {
        for (int i = 1; i <= 8; i++) {
            Vector3 tempVec = new Vector3(((1.5f * i) / eightFrame) + 1.0f, ((1.5f * i) / eightFrame) + 1.0f);
            seedling.transform.localScale = tempVec;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
