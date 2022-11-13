using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager instance;
    public static ObjectManager Instance {
        get { return instance; }
    }

    [SerializeField]
    Transform player;
    [SerializeField]
    GameObject arrowPrefab;
    [SerializeField]
    GameObject waterBallPrefab;
    [SerializeField]
    GameObject horrorBallPrefab;
    [SerializeField]
    GameObject angryBallPrefab;
    [SerializeField]
    GameObject cloud1Prefab;
    [SerializeField]
    GameObject cloud2Prefab;
    [SerializeField]
    GameObject cloud3Prefab;
    [SerializeField]
    GameObject batPoopPrefab;

    Queue<GameObject> arrow;
    Queue<GameObject> waterBall;
    Queue<GameObject> horrorBall;
    Queue<GameObject> angryBall;

    Queue<GameObject> cloud1;
    Queue<GameObject> cloud2;
    Queue<GameObject> cloud3;

    Queue<GameObject> batPoop;
    void Awake() {
        instance = this;

        QueueReset();

        if (arrowPrefab != null)
            Initialize(arrow, 1, "arrow");
        if (waterBallPrefab != null)
            Initialize(waterBall, 10, "waterBall");
        if(horrorBallPrefab != null)
            Initialize(horrorBall, 10, "horrorBall");
        if (angryBallPrefab != null)
            Initialize(angryBall, 10, "angryBall");
        if (cloud1Prefab != null)
            Initialize(cloud1, 30, "cloud1");
        if (cloud2Prefab != null)
            Initialize(cloud2, 30, "cloud2");
        if (cloud3Prefab != null)
            Initialize(cloud3, 30, "cloud3");
        if (batPoopPrefab) {
            Initialize(batPoop, 50, "batPoop");
        }
    }
    private void QueueReset() {
        arrow = new Queue<GameObject>();
        waterBall = new Queue<GameObject>();
        horrorBall = new Queue<GameObject>();
        angryBall = new Queue<GameObject>();

        cloud1 = new Queue<GameObject>();
        cloud2 = new Queue<GameObject>();
        cloud3 = new Queue<GameObject>();

        batPoop = new Queue<GameObject>();
    }

    void Initialize(Queue<GameObject> obj, int cnt, string name) {
        GameObject prefab = SearchPrefab(name);
        for(int i = 0; i < cnt; i++) {
            obj.Enqueue(CreateNewObject(prefab, name));
        }
    }
    GameObject CreateNewObject(GameObject prefab, string name) {
        GameObject newObj = Instantiate(prefab);

        if(name == "cloud1" || name == "cloud2" || name == "cloud3") {
            Cloud cloudClass = newObj.GetComponent<Cloud>();
            cloudClass.player = player.gameObject;
        }

        newObj.SetActive(false);

        return newObj;
    }
    public GameObject GetObject(string name) {
        Queue<GameObject> targetPool = null;
        switch (name) {
            case "arrow":
                targetPool = arrow;
                break;
            case "waterBall":
                targetPool = waterBall;
                break;
            case "horrorBall":
                targetPool = horrorBall;
                break;
            case "angryBall":
                targetPool = angryBall;
                break;
            case "cloud1":
                targetPool = cloud1;
                break;
            case "cloud2":
                targetPool = cloud2;
                break;
            case "cloud3":
                targetPool = cloud3;
                break;
            case "batPoop":
                targetPool = batPoop;
                break;
            default:
                Debug.Log("GetObject not find name Error");
                return null;
        }
        if(targetPool.Count > 0) {
            GameObject obj = targetPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else {
            GameObject prefab = SearchPrefab(name);
            GameObject newObj = CreateNewObject(prefab, name);
            newObj.SetActive(true);
            return newObj;
        }
    }
    public void ReturnObject(GameObject obj, string name) {
        if (!obj.activeSelf) {
            //이미 오브젝트가 돌아와 있다면
            return;
        }
        Queue<GameObject> targetPool = SearchQueue(name);

        if (name.Equals("waterBall") || name.Equals("horrorBall") || name.Equals("angryBall")) {
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
        targetPool.Enqueue(obj);
        obj.transform.position = Vector2.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(false);
    }
    GameObject SearchPrefab(string name) {
        GameObject returnObj = null;
        switch (name) {
            case "arrow":
                returnObj = arrowPrefab;
                break;
            case "waterBall":
                returnObj = waterBallPrefab;
                break;
            case "horrorBall":
                returnObj = horrorBallPrefab;
                break;
            case "angryBall":
                returnObj = angryBallPrefab;
                break;
            case "cloud1":
                returnObj = cloud1Prefab;
                break;
            case "cloud2":
                returnObj = cloud2Prefab;
                break;
            case "cloud3":
                returnObj = cloud3Prefab;
                break;
            case "batPoop":
                returnObj = batPoopPrefab;
                break;
            default:
                Debug.Log("SearchPrefab not find name Error [" + name + "]");
                break;
        }
        return returnObj;
    }
    Queue<GameObject> SearchQueue(string name) {
        Queue<GameObject> returnQueue = null;
        switch (name) {
            case "arrow":
                returnQueue = arrow;
                break;
            case "waterBall":
                returnQueue = waterBall;
                break;
            case "horrorBall":
                returnQueue = horrorBall;
                break;
            case "angryBall":
                returnQueue = angryBall;
                break;
            case "cloud1":
                returnQueue = cloud1;
                break;
            case "cloud2":
                returnQueue = cloud2;
                break;
            case "cloud3":
                returnQueue = cloud3;
                break;
            case "batPoop":
                returnQueue = batPoop;
                break;
            default:
                Debug.Log("SearchQueue not find name Error");
                return null;
        }
        return returnQueue;
    }
}
