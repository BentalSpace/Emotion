using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    [SerializeField]
    private GameObject[] gameObjects;

    CameraMove cameraMove;

    void Start()
    {
        cameraMove = GameObject.Find("Main Camera").GetComponent<CameraMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] == null)
                    return;

                gameObjects[i].SetActive(true);
            }
        }
        cameraMove.GetComponent<CameraMove>().enabled = false;
    }
}