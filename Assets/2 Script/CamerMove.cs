using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerMove : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Transform animVec;

    float y, z;

    public bool animMove;

    Vector3 cameraPos;
    void Awake() {
        y = transform.position.y;
        z = transform.position.z;
    }
    void FixedUpdate() {
        cameraMove();
        zoomMove();
    }

    void cameraMove() {
        if (player.transform.position.x <= 0.8f || animMove)
            return;
        if (player.transform.position.x >= 167.3 || animMove)
            return;
        cameraPos = new Vector3(player.transform.position.x, y, z);
        transform.position = cameraPos;
    }

    void zoomMove() {
        if (!animMove)
            return;
        cameraPos = new Vector3(animVec.position.x, animVec.position.y, z);
        transform.Translate(cameraPos * 0.001f);

        if (transform.position.x >= animVec.position.x) {
            cameraPos = new Vector3(animVec.position.x, y, z);
            transform.position = cameraPos;
            StartCoroutine(delay());
        }
    }

    IEnumerator delay() {
        yield return new WaitForSeconds(4f);
        animMove = false;
    }
}
