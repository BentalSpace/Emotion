using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private GameObject startScene;
    [SerializeField]
    private GameObject onOffUI;
    [SerializeField]
    private GameObject stageNum;
    [SerializeField]
    private StageManager sm;
    [SerializeField]
    PlayerRenewal player;
    [SerializeField]
    private RectTransform abilityGaugeBar;
    [SerializeField]
    Image[] abilityBoxes;
    [SerializeField]
    Sprite playerAbilityBox;
    [SerializeField]
    Sprite fairyAbilityBox;

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        EscOption();
    }
    void LateUpdate() {
        AbilityGaugeBarUpdate();
        AbilityBoxesUpdate();
    }

    private void AbilityGaugeBarUpdate() {
        if (abilityGaugeBar != null)
            abilityGaugeBar.localScale = new Vector3((player.abilityCurGauge / 5) * 0.5f, 0.5f, 1);
    }
    private void AbilityBoxesUpdate() {
        if (abilityBoxes.Length == 0)
            return;
        if (GameManager.manager.playerAbilityOn && abilityBoxes[abilityBoxes.Length-1].sprite == fairyAbilityBox) {
            //플레이어 어빌리티가 켜진 상태이고, && ui스프라이트가 요정박스 스프라이트일때 실행
            foreach(Image abilityBox in abilityBoxes) {
                abilityBox.sprite = playerAbilityBox;
            }
        }
        else if(!GameManager.manager.playerAbilityOn && abilityBoxes[abilityBoxes.Length - 1].sprite == playerAbilityBox) {
            //요정 어빌리티가 켜진 상태이고, && ui스프라이트가 플레이어박스 스프라이트일때 실행
            foreach (Image abilityBox in abilityBoxes) {
                abilityBox.sprite = fairyAbilityBox;
            }
        }
    }
    public void StartBtnClick() {
        // 프롤로그 시작 코드
        // SceneManager.LoadScene(1);
        LoadingSceneManager.Instance.ChangeScene("CutScene");
        // SceneManager.LoadScene("CutScene");
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
    public void ChapterClick(GameObject stageSelect) {
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
        if (!onOffUI)
            return;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (onOffUI.activeSelf)
                onOffUI.SetActive(false);
            else
                onOffUI.SetActive(true);
        }
    }
}
