using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryObject : MonoBehaviour
{
    [SerializeField]
    float moveX;
    [SerializeField]
    float moveY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rock_Move()
    {
        StartCoroutine(RockMove());
    }

    public void Rock_Destroy()
    {
        StartCoroutine(RockDestroy());
    }

    public void Stalactite_Destroy()
    {
        StartCoroutine(StalactiteDestroy());

    }

    IEnumerator RockMove()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.transform.position = new Vector2(moveX, moveY);
    }

    IEnumerator RockDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    IEnumerator StalactiteDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
