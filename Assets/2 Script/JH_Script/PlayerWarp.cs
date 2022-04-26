using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerWarp : MonoBehaviour
{
    [SerializeField]
    private GameObject currentWarp;

    public Image Panel;

    float time = 0f;
    float F_time = 1f;
    public float invokeTime = 0f;

    CameraMove cameraMove;
    PlayerRenewal playerRenewal;
    Animator anim;

    void Start()
    {
        cameraMove = GameObject.Find("Main Camera").GetComponent<CameraMove>();
        playerRenewal = GameObject.Find("player").GetComponent<PlayerRenewal>();
        anim = GameObject.Find("player").GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Warp"))
        {
            playerRenewal.dontInput = true;
            anim.SetBool("isWalk", false);
            StartCoroutine(FadeFlow());
            currentWarp = collision.gameObject;
            Invoke("Destination", invokeTime);
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Warp"))
        {
            playerRenewal.dontInput = false;
            cameraMove.GetComponent<CameraMove>().enabled = true;
        }
    }

    private void Destination()
    {
        if (currentWarp == null)
            return;

        if(currentWarp != null)
        {
            transform.position = currentWarp.GetComponent<Warp>().GetDestination().position;
        }
    }

    IEnumerator FadeFlow()
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }

        time = 0f;

        yield return new WaitForSeconds(1f);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }

        Panel.gameObject.SetActive(false);
        yield return null;
    }
}