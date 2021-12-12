using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    SpriteRenderer playerSpriteRenderer;

    Vector2 playerNeedPos;

    void Awake() {
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }
    void FixedUpdate() {
        playerNeedPos = new Vector2(player.transform.position.x, 0);
        if (playerSpriteRenderer.flipX) {
            transform.position = new Vector2(1.5f, transform.position.y) + playerNeedPos;
        }
        else {
            transform.position = new Vector2(-1.5f, transform.position.y) + playerNeedPos;
        }
    }
}
