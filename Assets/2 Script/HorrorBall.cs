using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorBall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Object")) {
            if (collision.GetComponent<Spider>() != null) {
                collision.GetComponent<Spider>().Runaway();
            }
            else if (collision.GetComponent<Mole>() != null) {
                collision.GetComponent<Mole>().GoDown();
            }
        }
    }
}
