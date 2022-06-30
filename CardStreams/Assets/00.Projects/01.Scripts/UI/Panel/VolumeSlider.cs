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

    /// <summary> Slider�� OnValueChanged �̺�Ʈ�� �߰��� �Լ�. ������ �ٲ𶧸��� Update���ִ� �Լ��Դϴ�. </summary>
    /// <param name="type">���������� �Ҹ�Ÿ��</param>
    public void VolumeChangeUpdate()
    {
        // ���⼭ ���������ų� �۵��ȵǸ� VolumeSlider Start�� SoundManager Awake���� ��������Ȱ���
        SetVolumeEvent(_soundType, _slider.value);
        _volumeText.text = _slider.value.ToString();
    }
}
