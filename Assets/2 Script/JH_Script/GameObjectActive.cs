using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectActive : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objs;

    // public float invokeTime;

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(OBJ());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject == null)
                return;

            Destroy(this);
            gameObject.GetComponent<GameObjectActive>().enabled = false;
        }
    }

    IEnumerator OBJ()
    {
        yield return new WaitForSeconds(0.5f);
        objs[0].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        objs[0].SetActive(false);

        yield return new WaitForSeconds(2.0f);
        objs[1].SetActive(true);
        //yield return new WaitForSeconds(6.0f);
        //objs[1].SetActive(false);
    }
}
