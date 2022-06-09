using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp_CircleEffect : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    Transform targetWarp;
    [SerializeField]
    Animator circleEffect;

    public IEnumerator Warp() {
        circleEffect.gameObject.transform.position = player.transform.position;
        circleEffect.SetTrigger("big");
        player.gameObject.GetComponent<PlayerRenewal>().dontInput = true;
        yield return new WaitForSeconds(0.6f);
        player.position = targetWarp.position;
        circleEffect.gameObject.transform.position = player.transform.position;
        circleEffect.SetTrigger("small");
        
        player.gameObject.GetComponent<PlayerRenewal>().dontInput = false;
    }
    private void Update() {
    }
}
