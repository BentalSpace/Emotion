using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mask : MonoBehaviour
{
    public enum MaskType { Sad, Horror, Angry, Happy }
    [SerializeField]
    Image maskSprite;

    public MaskType mask;

    bool doMaskEvent;

    PlayerRenewal player;

    void Update() {
        if (doMaskEvent) {
            if (Input.anyKeyDown) {
                doMaskEvent = false;
                player.dontInput = false;
                maskSprite.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
    public void MaskEvent(PlayerRenewal player) {
        this.player = player;
        if (mask == MaskType.Sad) {
            GameManager.manager.haveSadMask = true;
        }
        else if(mask == MaskType.Horror) {
            GameManager.manager.haveHorrorMask = true;
        }
        else if (mask == MaskType.Angry)
        {
            GameManager.manager.haveAngryMask = true;
        }
        if (maskSprite) {
            doMaskEvent = true;
            player.dontInput = true;
            maskSprite.gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
        }
    }
}
