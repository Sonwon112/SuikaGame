using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSetting : MonoBehaviour
{
    public AudioMixer mixer;
    public TMP_Text volumeText;
    public float startVolume = 0.9f;

    private float currVolume = 0f;

    private void Start()
    {
        mixer.GetFloat("bgmVolume",out currVolume);
        SetLevel(startVolume);
        SetvolumeTxt(startVolume);
    }

    public void SetLevel(float slideVal)
    {
        float tmp = remap(slideVal, 0f, 1f, -80f, 20f);
        mixer.SetFloat("MasterVolume", tmp);
    }

    public void SetvolumeTxt(float slideVal)
    {
        string txt = Mathf.Round(slideVal * 100f)  + "%";
        volumeText.text = txt;
    }

    public void Mute(bool mute)
    {
        if (mute)
        {
            mixer.SetFloat("bgmVolume", currVolume);
        }
        else
        {
            mixer.SetFloat("bgmVolume", -80f);
        }
    }

    public static float remap(float val, float in1, float in2, float out1, float out2)
    {
        return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
    }
}
