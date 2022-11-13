using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moss : MonoBehaviour
{
    [SerializeField]
    float power;
    [SerializeField]
    int givePowerCnt;
    [SerializeField]
    float givePowerTime = 0.1f;
    [SerializeField]
    float clearTime;

    public IEnumerator MossAbility(PlayerRenewal player) {
        player.Rigid.velocity = Vector3.zero;
        player.dontInput = true;
        for(int i = 0; i < givePowerCnt; i++) {
            player.Rigid.AddForce(Vector3.right * power, ForceMode2D.Force);
            yield return new WaitForSeconds(givePowerTime);
        }
        yield return new WaitForSeconds(clearTime);
        player.dontInput = false;
    }
}
