using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MySceneManager : MonoBehaviour
{
    public CanvasGroup Fade_img;
    float fadeDuration = 1.5f; //�����Ǵ� �ð�

    public GameObject Loading;
    public Text Loading_text; //�ۼ�Ʈ ǥ���� �ؽ�Ʈ

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
            Fade_img.blocksRaycasts = true; //�Ʒ� ����ĳ��Ʈ ����
        })

        .OnComplete(() =>
        {
            //�ε�ȭ�� ����, �� �ε� ����
            StartCoroutine("LoadScene", sceneName);
        });
    }

    IEnumerator LoadScene(string sceneName)
    {
        Loading.SetActive(true); //�ε� ȭ���� ���

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; //�ۼ�Ʈ �����̿�

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
        SceneManager.sceneLoaded -= OnSceneLoaded; // �̺�Ʈ���� ����*
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
