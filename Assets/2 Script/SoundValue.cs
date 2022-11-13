using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CafofoStudio;
using UnityEngine.UI;

public class SoundValue : MonoBehaviour
{
    public static SoundValue instance;
    [Header("사운드 옵션 변수")]
    [Range(0,1)]
    public float bgmSound;
    [Range(0, 1)]
    public float sfxSound;
    [Range(0, 1)]
    public float footSound;


    void Awake() {
        if(instance == null)
            instance = this;
        else {
            Destroy(gameObject);
        }
            
        DontDestroyOnLoad(this.gameObject);
    }
}
