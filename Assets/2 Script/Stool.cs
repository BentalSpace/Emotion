using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stool : MonoBehaviour
{
    SpriteRenderer renderer;

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
    }
    public void Hide() {
        StartCoroutine(Enum_Hide());
    }
    IEnumerator Enum_Hide() {
        float progress = 0;
        while(progress < 1) {
            progress += 0.2f;
            renderer.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), progress);
            if(progress > 0.5f) {
                GetComponent<BoxCollider2D>().enabled = false;
            }
            yield return new WaitForSeconds(0.05f);
         }
    }
}
