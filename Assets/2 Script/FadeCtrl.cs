using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCtrl : MonoBehaviour
{
    public static FadeCtrl instance;
    Image fadePanel;
    void Awake() {
        instance = this;

        fadePanel = GameObject.Find("FadePanel").GetComponent<Image>();
        fadePanel.color = new Color32(0, 0, 0, 0);
    }
    public void PanelColorChange() {
        
    }
    public void FadeOutCtrl(byte r = 0, byte g = 0, byte b = 0) {
        // 점점 어두워지는
        StartCoroutine(FadeOut(r,g,b));
    }
    public void FadeInCtrl(byte r = 0, byte g = 0, byte b = 0) {
        StartCoroutine(FadeIn(r, g, b));
    }
    IEnumerator FadeOut(byte r, byte g, byte b) {
        float progress = 0;
        while(progress < 1) {
            progress += 0.05f;
            fadePanel.color = Color.Lerp(new Color(r, g,b, 0), new Color(r, g, b, 1), progress);
            yield return new WaitForSeconds(0.03f);
        }   
    }

    IEnumerator FadeIn(byte r, byte g, byte b) {
        float progress = 0;
        while(progress < 1) {
            progress += 0.05f;
            fadePanel.color = Color.Lerp(new Color(r, g, b, 1), new Color(r, g, b, 0), progress);
            yield return new WaitForSeconds(0.03f);
        }
    }
}
