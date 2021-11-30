using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerByObject : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            //Àå¾Ö¹°°ú ºÎµúÇûÀ»¶§
            Debug.Log("Test");
        }
    }
}
