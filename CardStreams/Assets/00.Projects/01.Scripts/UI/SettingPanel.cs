using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] AudioSource _bgmPlayer;
    [SerializeField] AudioMixer _masterVolumeMixer;

    [SerializeField] VolumeSlider[] volumeSliders;


    public void ChangeVolume(float volume)
    {
        if (volume <= 0)
        {
            _masterVolumeMixer.SetFloat("BGM", -80f);
        }
        else
        {
            _masterVolumeMixer.SetFloat("BGM", 0.4f * volume - 40);
        }
    }
}
