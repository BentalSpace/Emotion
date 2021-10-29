using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startScene;
    [SerializeField]
    private GameObject chapterSelect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBtnClick() {
        startScene.SetActive(false);
        // 프롤로그 시작 코드
    }
    public void ContinueBtnClick() {
        chapterSelect.SetActive(true);
    }
    public void Option() {
        Debug.Log("TATATATAT");
    }
    public void Quit() {
        Debug.Log("Test");
        Application.Quit();
    }
    public void Chapter1Click() {
        SceneManager.LoadScene(1);
    }
    public void SelectBackClick() {
        chapterSelect.SetActive(false);
    }
}
