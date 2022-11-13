using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MySceneManager : MonoBehaviour
{
    public CanvasGroup Fade_img;
    float fadeDuration = 1.5f; //암전되는 시간

    public GameObject Loading;
    public Text Loading_text; //퍼센트 표시할 텍스트

    [SerializeField]
    private Slider slider;

    float percentage = 0f;

    public static MySceneManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static MySceneManager instance;

    void Start()
    {

        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ChangeScene(string sceneName)
    {
        slider.value = 0f;
        percentage = 0f;

        Fade_img.DOFade(1, fadeDuration)

        .OnStart(() =>
        {
            Fade_img.blocksRaycasts = true; //아래 레이캐스트 막기
        })

        .OnComplete(() =>
        {
            //로딩화면 띄우며, 씬 로드 시작
            StartCoroutine("LoadScene", sceneName);
        });
    }

    IEnumerator LoadScene(string sceneName)
    {
        Loading.SetActive(true); //로딩 화면을 띄움

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; //퍼센트 딜레이용

        float timer = 0f;
      
        while (!(async.isDone))
        {
            yield return null;

            timer += Time.deltaTime * 0.2f;

            percentage = slider.value * 100;
            Loading_text.text = percentage.ToString("0") + " %";

            if (async.progress < 0.9f)
            {
                slider.value = Mathf.Lerp(slider.value, async.progress, timer);

                if (slider.value >= async.progress)
                {
                    timer = 0f;
                }
            }

            else
            {
                slider.value = Mathf.Lerp(0f, 1f, timer);

                // past_time += Time.unscaledDeltaTime * 0.1f;

                if (slider.value == 1.0f)
                {
                    async.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트에서 제거*
    }       

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Fade_img.DOFade(0, fadeDuration)
        .OnStart(() =>
        {
            Loading.SetActive(false);
        })
        .OnComplete(() =>
        {
            Fade_img.blocksRaycasts = false;
        });
    }
}
