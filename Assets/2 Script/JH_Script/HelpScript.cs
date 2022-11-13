using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScript : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    //[SerializeField]
    //private GameObject loadingImage;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        HelpOnOff();
    }

    public void OnButtonClick()
    {
        panel.SetActive(true);
    }

    public void OffButtonClick()
    {
        panel.SetActive(false);
    }

    public void HelpOnOff()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            panel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            panel.SetActive(true);
        }
    }

    //public void LoadingObjectAcive()
    //{
    //    loadingImage.SetActive(true);
    //}
}
