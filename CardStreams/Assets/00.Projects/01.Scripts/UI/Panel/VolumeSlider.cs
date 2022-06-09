using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _volumeText;
    [SerializeField] AudioMixer _masterVolumeMixer;

    private bool isMute = false;
    private float _volume = 0;

    public void ChangeVolume()
    {
        _volume = _slider.value;
        _volumeText.text = _volume.ToString();

        Debug.Log("Change Volume");
        if (_volume <= 0)
        {
            _masterVolumeMixer.SetFloat("BGM", -80f);
        }
        else
        {
            Debug.Log(0.4f * _volume - 40);
            _masterVolumeMixer.SetFloat("BGM", 0.2f * _volume - 20);
        }
    }

    public void MuteVolume()
    {
        isMute = !isMute;

        AudioListener.volume = isMute ? 0 : 1;
    }
}
