using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
    public AudioMixer audioMix;
    public Slider[] volumeSlider;
    private void Start() {
        if(!PlayerPrefs.HasKey("Master")){
            PlayerPrefs.SetInt("Master", 0);
            PlayerPrefs.SetInt("SFX", 0);
            PlayerPrefs.SetInt("BGM", 0);
            PlayerPrefs.Save();
        }
        volumeSlider[0].value = PlayerPrefs.GetInt("Master");
        audioMix.SetFloat("Master",PlayerPrefs.GetInt("Master"));
        volumeSlider[1].value = PlayerPrefs.GetInt("SFX");
        audioMix.SetFloat("SFX",PlayerPrefs.GetInt("SFX"));
        volumeSlider[2].value = PlayerPrefs.GetInt("BGM");
        audioMix.SetFloat("BGM",PlayerPrefs.GetInt("BGM"));
    }
    public void VolumeChange(int num){
        int volumeValue = (int)volumeSlider[num].value;
        string name = null;
        switch (num)
        {
            case 0:
                name = "Master";
                break;
            case 1:
                name = "SFX";
                break;
            case 2:
                name = "BGM";
                break;   
        }
        audioMix.SetFloat(name,(volumeValue == -30)? -80 : volumeValue);
        PlayerPrefs.SetInt(name, volumeValue);
        PlayerPrefs.Save();
    }
}
