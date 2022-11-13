using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject fairyAbilities;
    [SerializeField] private GameObject playerAbilities;

    private bool isActive = false;

    void Start()
    {
        // Debug.Log(gameObject);
        // Debug.Log("°³¼ö : " + this.transform.childCount);
    }

    // Update is called once per frame
    void Update()
    {
        AblilityChange();
    }

    private void AblilityChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            OnClickSwich();
        }
    }

    void OnClickSwich()
    {
        isActive = !isActive;
        playerAbilities.SetActive(isActive);  
        fairyAbilities.SetActive(!isActive);
    }
}
