using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField]
    FairyAbility fairyAbility;

    [SerializeField]
    GameObject[] playerAbilitys;

    [SerializeField]
    GameObject[] fairyAbilitys;

    [SerializeField]
    GameObject character;

    [SerializeField]
    GameObject fairy;

    public static GameManager manager;

    //요정과 플레이어 능력 스왑 bool값
    public bool playerAbilityOn;
    //y축으로 맵이 넓은가?
    public bool thisHighMap;
    public int abilityCnt;

    //능력 가면을 가지고 있나?
    //씬 정보 가져와서 가면 얻은 이후 맵은 기본값 true로 바꿔야 한다.
    public bool haveSadMask;
    public bool haveHorrorMask;
    public bool haveAngryMask;
    public bool haveHappyMask;

    void Awake() {
        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 5 ||
                SceneManager.GetActiveScene().buildIndex == 8)
        {
            playerAbilityOn = true;
        }
        else
        {
            playerAbilityOn = false;
        }

        if (SceneManager.GetActiveScene().buildIndex == 2) {
            thisHighMap = true;
        }
        manager = this;
    }
    void Update() {
        Swap();
    }
    void Swap() {
        if (fairyAbility == null)
            return;
        if (fairyAbility.Sading)
            return;
        if (Input.GetButtonDown("Swap"))
        {
            playerAbilityOn = !playerAbilityOn;

            if (!playerAbilityOn)
            {
                for (int i = 0; i < fairyAbilitys.Length; i++)
                {
                    fairyAbilitys[i].SetActive(true);
                    playerAbilitys[i].SetActive(false);
                }
                character.SetActive(false);
                fairy.SetActive(true);
            }
            else if (playerAbilityOn)
            {
                for (int i = 0; i < fairyAbilitys.Length; i++)
                {
                    fairyAbilitys[i].SetActive(false);
                    playerAbilitys[i].SetActive(true);
                }
                character.SetActive(true);
                fairy.SetActive(false);
            }
        }
    }
}
