using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [SerializeField]
    Transform player;
    float delay;
    float curDelay;

    GameObject cloud;
    void Awake() {
        Invoke("CreateCloud", 0.5f);
    }

    void FixedUpdate() {
        
    }

    void CreateCloud() {
        int rand = Random.Range(0, 3);
        int rand2 = Random.Range(0, 2);

        switch (rand) {
            case 0:
                cloud = ObjectManager.Instance.GetObject("cloud1");
                break;
            case 1:
                cloud = ObjectManager.Instance.GetObject("cloud2");
                break;
            case 2:
                cloud = ObjectManager.Instance.GetObject("cloud3");
                break;
            default:
                Debug.Log("CreateCloud func random Error");
                break;
        }
        Rigidbody2D cloudRigid = cloud.GetComponent<Rigidbody2D>();
        Cloud cloudClass = cloud.GetComponent<Cloud>();
        cloudClass.speed = Random.Range(1.0f, 6.5f);
        if (rand2 == 0) {
            cloud.transform.position = new Vector2(player.position.x + 25, Random.Range(10.0f, 23.0f));
            cloudClass.dir = -1;
            cloudClass.isMove = true;
        }
        else {
            cloud.transform.position = new Vector2(player.position.x - 25, Random.Range(10.0f, 23.0f));
            cloudClass.dir = 1;
            cloudClass.isMove = true;
        }
        delay = Random.Range(1, 4);
        Invoke("CreateCloud", delay);
    }
}
