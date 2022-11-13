using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CafofoStudio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    GameObject optionPanel;
    [SerializeField]
    Slider bgmSlider;
    [SerializeField]
    Slider footSlider;
    [SerializeField]
    Slider sfxSlider;
    [SerializeField]
    AudioSource bgmAudio;
    [SerializeField]
    AudioSource footAudio;
    [SerializeField]
    AudioSource[] sfxAudio;

    // 공포챕터 배경음 & 효과음
    CaveAmbientMixer caveMixer;

    // 공포 박쥐 소리 체크
    public delegate void batSoundHandler();
    public event batSoundHandler BatSoundCheck;
    [HideInInspector] public int batSoundCnt;
    [HideInInspector] public float batDistance;

    // 공포 물소리 체크
    public delegate void waterSoundHandler();
    public event waterSoundHandler waterSoundCheck;
    [HideInInspector] public int waterSoundCnt;
    [HideInInspector] public float waterDistance;

    bool isHorror = false;
    void Awake() {
        instance = this;
        optionPanel = GameObject.Find("Panel_Option");
        optionPanel.SetActive(false);
        footAudio = GameObject.Find("player")?.GetComponent<AudioSource>();
        if(SceneManager.GetActiveScene().name.Equals("Horror") || SceneManager.GetActiveScene().name.Equals("HorrorUnderGround")) {
            isHorror = true;
            caveMixer = GameObject.Find("CaveAmbience")?.GetComponent<CaveAmbientMixer>();
        }
    }
    private void Start() {
        // 저장되어 있는 볼륨으로 오디오 볼륨 변경
        bgmSlider.value = SoundValue.instance.bgmSound / 100f;
        if (isHorror) {
            float volume = SoundValue.instance.bgmSound / 100f;
            caveMixer.Atmosphere1.SetVolumeMultiplier(volume);
            caveMixer.Atmosphere1.Play();
            caveMixer.Atmosphere2.SetVolumeMultiplier(volume);
            caveMixer.Atmosphere2.Play();
            caveMixer.Atmosphere3.SetVolumeMultiplier(volume);
            caveMixer.Atmosphere3.Play();
        }
        else {
            bgmAudio.volume = bgmSlider.value;
        }

        footSlider.value = SoundValue.instance.footSound / 100;
        if (footAudio) {
            footAudio.volume = footSlider.value;
        }

        sfxSlider.value = SoundValue.instance.sfxSound / 100;
        if (isHorror) {
            float volume = sfxSlider.value;
            caveMixer.Critters.SetVolumeMultiplier(volume);
            caveMixer.Critters.Play();
            caveMixer.WaterStream.SetVolumeMultiplier(volume);
            caveMixer.WaterStream.Play();
        }
        foreach (AudioSource item in sfxAudio) {
            item.volume = sfxSlider.value;
        }
    }
    void Update() {
        if (isHorror) {
            BatSoundCheckUpdate();
            WaterSoundCheckUpdate();
        }
    }
    public void SoundOptionChange(Slider self) {
        // 음량 슬라이더 변경 시, 텍스트 숫자 변경
        int value = (int)(self.value * 100);

        self.transform.GetChild(3).GetComponent<Text>().text = value.ToString();
    }
    public void SoundOptionSave() {
        // 옵션 저장
        SoundValue.instance.bgmSound = bgmSlider.value;
        SoundValue.instance.footSound = footSlider.value;
        SoundValue.instance.sfxSound = sfxSlider.value;
        SoundUpdate();
    }
    void SoundUpdate() {
        if (isHorror) {
            // 배경음
            float volume = SoundValue.instance.bgmSound / 100f;
            caveMixer.Atmosphere1.SetVolumeMultiplier(volume);
            caveMixer.Atmosphere2.SetVolumeMultiplier(volume);
            caveMixer.Atmosphere3.SetVolumeMultiplier(volume);
            // 효과음
            volume = SoundValue.instance.sfxSound / 100f;
            caveMixer.Critters.SetVolumeMultiplier(volume);
            caveMixer.WaterStream.SetVolumeMultiplier(volume);
        }
        else {
            bgmAudio.volume = SoundValue.instance.bgmSound;
        }
        if(footAudio)
            footAudio.volume = SoundValue.instance.footSound;
        foreach (AudioSource item in sfxAudio) {
            item.volume = SoundValue.instance.sfxSound;
        }
    }
    void BatSoundCheckUpdate() {
        batSoundCnt = 0;
        batDistance = 999;
        BatSoundCheck();
        if (batSoundCnt > 0 && caveMixer.Critters.GetIntensity() != 1) {
            // 박쥐가 들리기 시작했을 때
            caveMixer.Critters.SetIntensity(1);
            float volume = 0.01f + (0.0396f * (30 - batDistance));
            volume *= (SoundValue.instance.sfxSound / 100);
            // 30 min / 5 max
            caveMixer.Critters.SetVolumeMultiplier(volume);
        }
        else if(batSoundCnt > 0 && caveMixer.Critters.GetIntensity() == 1) {
            // 이미 거리 안에 들어와 있을때
            float volume = 0.01f + (0.0396f * (30 - batDistance));
            volume *= (SoundValue.instance.sfxSound / 100);
            caveMixer.Critters.SetVolumeMultiplier(volume);
        }
        else if (batSoundCnt <= 0 && caveMixer.Critters.GetIntensity() != 0) {
            // 거리가 멀어질때
            caveMixer.Critters.SetIntensity(0);
        }
    }
    void WaterSoundCheckUpdate() {
        waterSoundCnt = 0;
        waterDistance = 999;
        waterSoundCheck();
        if (waterSoundCnt > 0 && caveMixer.WaterStream.GetIntensity() != 0.5f) {
            caveMixer.WaterStream.SetIntensity(0.5f);
            float volume = 0.01f + (0.066f * (20 - waterDistance));
            volume *= (SoundValue.instance.sfxSound / 100);
            //0.01 * 15 = 0.15 => 1
            // 15 * 0.066 = 0.99
            // dis = 15
            // 0.066 * 5 = 0.33 + 0.01f = 0.34 volume
            // dis = 10
            // 0.066 * 10 = 0.66 + 0.01f =  0.67 volume
            // dis = 5
            // 0.066 * 15 = 0.99 + 0.01f = 1 volume

            caveMixer.WaterStream.SetVolumeMultiplier(volume);
        }
        else if (waterSoundCnt > 0 && caveMixer.WaterStream.GetIntensity() == 0.5f) {
            float volume = 0.01f + (0.066f * (20 - waterDistance));
            volume *= (SoundValue.instance.sfxSound / 100);
            caveMixer.WaterStream.SetVolumeMultiplier(volume);
        }
        else if (waterSoundCnt <= 0 && caveMixer.WaterStream.GetIntensity() != 0) {
            caveMixer.WaterStream.SetIntensity(0);
        }
    }
    public void OnOption() {
        optionPanel.SetActive(true);
    }
    public void OffOption() {
        optionPanel.SetActive(false);
    }
}
