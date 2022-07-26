using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineControl : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    void FixedUpdate()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(5.5f);
        MySceneManager.Instance.ChangeScene(sceneName);
        Destroy(this);
    }   
}
