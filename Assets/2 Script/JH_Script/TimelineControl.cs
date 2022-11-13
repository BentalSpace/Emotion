using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineControl : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    public float time;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            MySceneManager.Instance.ChangeScene(sceneName);
        }
    }

    void FixedUpdate()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(time);
        MySceneManager.Instance.ChangeScene(sceneName);
        Destroy(this);
    }
}
