using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadingSceneManager : MonoBehaviour
{
    public CanvasGroup Fade_img;
    float fadeDuration = 1.5f; //암전되는 시간

    public static LoadingSceneManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static LoadingSceneManager instance;

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
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; //퍼센트 딜레이용

        float timer = 0f;

        while (!(async.isDone))
        {
            yield return null;

            timer += Time.deltaTime * 0.2f;

            if (async.progress < 0.9f)
            {
                async.allowSceneActivation = true;
                yield break;
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
            
        })
        .OnComplete(() =>
        {
            Fade_img.blocksRaycasts = false;
        });
    }
}
