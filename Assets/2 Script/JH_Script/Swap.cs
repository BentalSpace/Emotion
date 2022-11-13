using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : MonoBehaviour
{
    public bool isChange;

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

    // Start is called before the first frame update
    void Awake()
    {
        isChange = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Change();
    }

    void Change()
    {
        if (fairyAbility == null)
            return;
        if (fairyAbility.Sading)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isChange = !isChange;

            if (!isChange)
            {
                for(int i = 0; i < fairyAbilitys.Length; i++)
                {
                    fairyAbilitys[i].SetActive(true);
                    playerAbilitys[i].SetActive(false);
                }
                character.SetActive(false);
                fairy.SetActive(true);
            }
            else if(isChange)
            {
                for (int i = 0; i < fairyAbilitys.Length; i++)
                {
                    fairyAbilitys[i].SetActive(false);
                    playerAbilitys[i].SetActive(true);
                }
                character.SetActive(true);
                fairy.SetActive(false);
            }
            Debug.Log("ÅÇ ´­¸²");
        }
    }
}
