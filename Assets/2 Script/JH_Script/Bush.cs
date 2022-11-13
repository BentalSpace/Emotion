using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    [SerializeField]
    private GameObject[] gameObjects;

    [SerializeField]
    private GameObject[] dirs;

    [SerializeField]
    private GameObject keys;

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
            keys.SetActive(false);
        }
        cameraMove.GetComponent<CameraMove>().enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            for (int i = 0; i < dirs.Length; i++)
            {
                if (dirs[i] == null)
                    return;

                dirs[i].SetActive(true);
            }
        }
    }
}