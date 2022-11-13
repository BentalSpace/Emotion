using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerByObject_Angry : MonoBehaviour
{
    [SerializeField]
    private GameObject seedling;

    private float eightFrame;

    public bool isSkillRange;

    void Awake()
    {
        eightFrame = 8;
    }

    void Update()
    {
        if (isSkillRange)
        {
            if (Input.GetButtonDown("Angry"))
            {
                //StartCoroutine();
                Debug.Log("분노 스킬 사용");
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "skill")
        {
            isSkillRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "skill")
        {
            isSkillRange = false;
        }
    }
}
