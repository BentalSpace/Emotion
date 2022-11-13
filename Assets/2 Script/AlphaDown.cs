using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaDown : MonoBehaviour
{
    float curTime = 0;
    float progress = 0;
    Text text;
    
    void Awake() {
        text = GetComponent<Text>();
    }
    void Update() {
        curTime += Time.deltaTime;
        if(curTime > 2f) {
            text.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), progress);
            progress += Time.deltaTime / 2;
        }
        if(progress >= 1) {
            gameObject.SetActive(false);
            curTime = 0;
            progress = 0;
            text.color = Color.white;
        }
    }
    //IEnumerator TextHide() {

    //}
}
