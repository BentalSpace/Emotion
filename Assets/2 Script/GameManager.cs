using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField]
    FairyAbility fairyAbility;

    //요정과 플레이어 능력 스왑 bool값
    public static bool playerAbilityOn;
    //y축으로 맵이 넓은가?
    public static bool thisHighMap;

    //능력 가면을 가지고 있나?
    //씬 정보 가져와서 가면 얻은 이후 맵은 기본값 true로 바꿔야 한다.
    public static bool haveSadMask;

    [SerializeField]
    public bool haveFairyAbility;

    void Awake() {
        playerAbilityOn = true;
        if (SceneManager.GetActiveScene().buildIndex == 2) {
            thisHighMap = true;
        }
    }
    void Update() {
        Swap();
    }
    void Swap() {
        if (fairyAbility.Sading)
            return;
        if (Input.GetButtonDown("Swap"))
            playerAbilityOn = !playerAbilityOn;
    }
}
