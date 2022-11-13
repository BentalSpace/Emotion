using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private GameObject currentWarp;

    CameraMove cameraMove;

    void Start()
    {
        cameraMove = GameObject.Find("Main Camera").GetComponent<CameraMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Warp"))
        {
            transform.position = currentWarp.GetComponent<Warp>().GetDestination().position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Warp"))
        {
            cameraMove.GetComponent<CameraMove>().enabled = true;
        }
    }

    private void Destination()
    {
        if (currentWarp == null)
            return;

        if (currentWarp != null)
        {
            transform.position = currentWarp.GetComponent<Warp>().GetDestination().position;
        }
    }
}
