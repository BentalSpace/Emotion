using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private GameObject startScene;
    [SerializeField]
    private GameObject onOffUI;
    [SerializeField]
    private GameObject stageSelect;
    [SerializeField]
    private GameObject stageNum;
    [SerializeField]
    private StageManager sm;

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        EscOption();
    }

    public void StartBtnClick() {
        // 프롤로그 시작 코드
        SceneManager.LoadScene(1);

    }
    public void ContinueBtnClick() {
        onOffUI.SetActive(true);
    }
    public void Option() {
        Debug.Log("Option");
    }
    public void Quit() {
        Debug.Log("Test");
        Application.Quit();
    }
    public void ChapterClick() {
        stageSelect.SetActive(true);
    }
    public void StageSelect() {
        string clickBtn = EventSystem.current.currentSelectedGameObject.name;
        sm.ChapterStageNum = clickBtn;
        string[] chapterNum = clickBtn.Split('-');
        SceneManager.LoadScene(int.Parse(chapterNum[0]) + 1);
        DontDestroyOnLoad(stageNum);
    }
    public void SelectBackClick() {
        onOffUI.SetActive(false);
    }

    public void GoToTitle() {
        SceneManager.LoadScene(0);
    }

    private void EscOption() {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (onOffUI.activeSelf)
                onOffUI.SetActive(false);
            else
                onOffUI.SetActive(true);
        }
    }
}
