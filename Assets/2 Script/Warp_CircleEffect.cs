using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp_CircleEffect : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    Transform targetWarp;
    [SerializeField]
    Animator circleEffect;

    public IEnumerator Warp() {
        circleEffect.gameObject.SetActive(true);
        circleEffect.gameObject.transform.position = player.transform.position;
        circleEffect.SetTrigger("big");

        yield return new WaitForSeconds(1.1f);
        GameObject chapterStage = GameObject.Find("StageManager");
        chapterStage.name = "StageNum";
        chapterStage.transform.parent = default;
        SceneManager.LoadScene("HorrorUnderGround");
        DontDestroyOnLoad(chapterStage);
    }
    private void Update() {
    }
}
