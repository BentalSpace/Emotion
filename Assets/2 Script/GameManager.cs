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

    //������ �÷��̾� �ɷ� ���� bool��
    public bool playerAbilityOn;
    //y������ ���� ������?
    public bool thisHighMap;
    public int abilityCnt;

    //�ɷ� ������ ������ �ֳ�?
    //�� ���� �����ͼ� ���� ���� ���� ���� �⺻�� true�� �ٲ�� �Ѵ�.
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
