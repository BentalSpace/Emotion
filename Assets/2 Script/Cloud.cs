using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField]
    string name;

    public bool isMove;
    public float speed;
    public int dir;

    public GameObject player;

    Rigidbody2D rigid;
    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Update() {
        CloudMove();
        CloudHide();
    }
    
    void CloudMove() {
        if (!isMove)
            return;

        rigid.velocity = Vector2.right * dir * speed ;
    }

    void CloudHide() {
        if (!isMove)
            return;
        if (player.transform.position.x > transform.position.x + 30 || player.transform.position.x < transform.position.x - 30) {
            //Debug.Log(player.transform.position.x + ", " + transform.position.x);
            rigid.velocity = Vector2.zero;
            ObjectManager.Instance.ReturnObject(gameObject, name);
        }
    }
}
