using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerMove : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    float y, z;

    Vector3 cameraPos;
    void Awake() {
        y = transform.position.y;
        z = transform.position.z;
    }
    void FixedUpdate() {
        cameraMove();
    }

    public void cameraMove() {
        if (player.transform.position.x <= 0.8)
            return;
        cameraPos = new Vector3(player.transform.position.x, y, z);
        transform.position = cameraPos;
    }
}
