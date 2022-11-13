using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRise : MonoBehaviour
{
    [SerializeField]
    GameObject background;

    [SerializeField]
    private float speed;

    private float endPoint = 20.8f;

    //public Transform startPosition;
    // public Transform endPosition;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Rise();
    }

    void Rise()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (background.transform.position.y < endPoint)
            {
                background.transform.Translate(0, Time.deltaTime * speed, 0);
            }
            //background.transform.Translate(backVec);
        }
    }
}