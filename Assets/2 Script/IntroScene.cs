using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    [SerializeField]
    GameObject stageBtn;

    int chapter1Stage = 6;
    int chapter2Stage = 6;
    int chapter;
    GameObject nowStageSelect;


    void Awake() {
        
    }
    public void ChapterClick(GameObject stageSelect) {
        if(nowStageSelect != null)
            nowStageSelect.SetActive(false);
        chapter = int.Parse(stageSelect.transform.parent.name[stageSelect.transform.parent.name.Length-1].ToString());
        nowStageSelect = stageSelect;
        nowStageSelect.SetActive(true);
    }

    public void StageClick() {
        StageManager sm = GameObject.Find("StageNum").GetComponent<StageManager>();
        string clickBtn = EventSystem.current.currentSelectedGameObject.name;
        sm.ChapterStageNum = clickBtn;
        string[] chapterNum = clickBtn.Split('-');
        SceneManager.LoadScene(int.Parse(chapterNum[0]) + 1);
        DontDestroyOnLoad(sm.gameObject);
    }
    void temp() {
        Debug.Log("TEMP");
    }
}
