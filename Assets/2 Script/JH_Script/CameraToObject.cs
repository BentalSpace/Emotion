using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToObject : MonoBehaviour
{
    CameraMove cameraMove;
    new Camera camera;

    DialogueManager theDM;
    PlayerRenewal playerRenewal;

    [SerializeField]
    Transform targetObject;
    //GameObject target;

    Vector3 targetPosition;
    Vector3 velocity = Vector3.zero;

    public float smoothTime = 0.3f;
    // public float invokeTime;

    // Start is called before the first frame update
    void Start()
    {
        cameraMove = GameObject.Find("Main Camera").GetComponent<CameraMove>();
        playerRenewal = GameObject.Find("player").GetComponent<PlayerRenewal>();
        camera = Camera.main;
        theDM = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRenewal.dontInput = true;
            MoveToCamera();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    public void MoveToCamera()
    {
        cameraMove.GetComponent<CameraMove>().enabled = false;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        playerRenewal.dontInput = false;
        yield return new WaitForSeconds(1.0f);
        camera.transform.position = new Vector3(195, 2.5f, -10);

        yield return new WaitForSeconds(3.0f);
        cameraMove.GetComponent<CameraMove>().enabled = true;

        yield return new WaitForSeconds(0f);
        theDM.ExitDialogue();
    }
}
