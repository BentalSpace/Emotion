using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    #region 임시 주석처리
    //public Transform potal_1;
    //public Transform player;

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.name == "Player")
    //    {
    //        Invoke("Teleport", 0.8f);
    //    }
    //}
    //void Teleport()
    //{
    //    player.transform.position = potal_1.position;
    //}
    #endregion

    [SerializeField] private Transform destination;

    public Transform GetDestination()
    {
        return destination;
    }
}
