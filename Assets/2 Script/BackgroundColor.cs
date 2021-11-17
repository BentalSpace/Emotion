using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColor : MonoBehaviour {
    [SerializeField]
    private int colorChangeFrame;
    [SerializeField]
    private float[] rgb;
    [SerializeField]
    private GameObject[] sprites;

    private bool isColorChange;

    private SpriteRenderer sr;
    void Awake() {
        isColorChange = false;
    }

    void FixedUpdate() {
    }

    void BackgroundColorChange() {
        if (!isColorChange)
            return;
        for(int i = 0; i < sprites.Length; i++) {
            sr = sprites[i].GetComponent<SpriteRenderer>();
            float r = (255 - rgb[0]) / colorChangeFrame;
            float g = (255 - rgb[1]) / colorChangeFrame;
            float b = (255 - rgb[2]) / colorChangeFrame;
            sr.color = new Color((255 - r) / 255f, (255 - g) / 255f, (255 - b) / 255f);
        }
        colorChangeFrame--;
        if (colorChangeFrame > 0)
            Invoke("BackgroundColorChange", 0.2f);
        else
            isColorChange = false;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            isColorChange = true;
            BackgroundColorChange();
            gameObject.SetActive(false);
        }
    }
}
