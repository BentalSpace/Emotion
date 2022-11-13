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
    GameObject circleEffect;

    bool nowUnder;

    void Awake() {
    }
    void Start() {
        if (SceneManager.GetActiveScene().name.Equals("Horror")) {
            nowUnder = false;
        }
        else if (SceneManager.GetActiveScene().name.Equals("HorrorUnderGround")) {
            nowUnder = true;
        }
        if (GameObject.Find("BeforeWarp")) {
            Destroy(GameObject.Find("BeforeWarp"));
            FadeCtrl.instance.FadeInCtrl();
            // 원이 작아지는 효과
            //circleEffect.gameObject.SetActive(true);
            //circleEffect.gameObject.transform.localScale = Vector3.one * 53;
            //yield return new WaitForSeconds(0.01f);
            //circleEffect.gameObject.transform.position = player.position;
            //float progress = 0;
            //while(progress < 1) {
            //    progress += 0.03f;
            //    circleEffect.transform.localScale = Vector3.Lerp(Vector3.one * 53, Vector3.zero, progress);
            //    yield return new WaitForSeconds(0.03f);
            //}
            //circleEffect.SetTrigger("small");
            //yield return new WaitForSeconds(0.5f);
            //circleEffect.gameObject.SetActive(false);
        }
    }
    private void FixedUpdate() {
        
    }
    public IEnumerator Warp() {
        
        FadeCtrl.instance.FadeOutCtrl();

        // 원 커지는 효과
        //circleEffect.SetActive(true);
        //circleEffect.transform.position = player.transform.position;

        //float progress = 0;
        //while(progress < 1) {
        //    progress += 0.03f;
        //    circleEffect.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 53, progress);
        //    yield return new WaitForSeconds(0.03f);
        //}
        yield return new WaitForSeconds(0.7f);
        GameObject chapterStage = GameObject.Find("StageManager");
        chapterStage.name = "StageNum";
        chapterStage.transform.parent = default;
        if(!nowUnder)
            SceneManager.LoadScene("HorrorUnderGround");
        else
            SceneManager.LoadScene("Horror");
        DontDestroyOnLoad(chapterStage);
        //gameObject.name = "BeforeWarp";
        //DontDestroyOnLoad(gameObject);
    }
}
