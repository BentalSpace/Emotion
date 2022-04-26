using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextChapter : MonoBehaviour
{
    [SerializeField]
    private StageManager sm;

    public Image Panel;
    public string sceneName;

    float time = 0f;
    float F_time = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "nextChapter")
        {
            StartCoroutine(FadeFlow());
            Invoke("ChapterManager", 2f);
        }
    }

    private void ChapterManager()
    {
        //다음 챕터로 이동
        // SceneManager.LoadScene(int.Parse(sm.ChapterStageNum.Split('-')[0]) + 2);
        SceneManager.LoadScene(sceneName);
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
