using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] Text _volumeText;
    public SoundType _soundType;

    public Action<SoundType, float> SetVolumeEvent;

    private void Start()
    {
        SetVolumeEvent += SoundManager.Instance.ChangeVolume;
    }

    /// <summary> Slider의 OnValueChanged 이벤트에 추가할 함수. 볼륨이 바뀔때마다 Update해주는 함수입니다. </summary>
    /// <param name="type">볼륨조절할 소리타입</param>
    public void VolumeChangeUpdate()
    {
        // 여기서 오류터지거나 작동안되면 VolumeSlider Start가 SoundManager Awake보다 먼저실행된거임
        SetVolumeEvent(_soundType, _slider.value);
        _volumeText.text = _slider.value.ToString();
    }
}
