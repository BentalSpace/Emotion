using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitPassObj : MonoBehaviour {
    [SerializeField]
    PlayerRenewal player;
    [SerializeField]
    float pos1;
    [SerializeField]
    float pos2; // pos2가 pos1보다 무조건 더 커야한다.
    [SerializeField]
    new BoxCollider2D collider;

    bool isUse;
    bool isLoop;

    void Update() {
    }

    public IEnumerator playerAutoCrawl() {
        if (isUse)
            yield break;
        isUse = true;
        player.dontInput = true;
        collider.isTrigger = true;

        while (true) {
            if (!player.FlipX) {
                player.Rigid.velocity = Vector2.right * 3;
                if (player.transform.position.x >= pos2)
                    break;
            }
            else {
                player.Rigid.velocity = Vector2.right * -3;
                if (player.transform.position.x <= pos1)
                    break;
            }
            yield return null;
        }

        player.Rigid.velocity = Vector2.zero;
        collider.isTrigger = false;
        player.dontInput = false;
        isUse = false;
    }
}
