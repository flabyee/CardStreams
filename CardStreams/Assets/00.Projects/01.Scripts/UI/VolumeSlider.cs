using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] Text _volumeText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary> Slider�� OnValueChanged �̺�Ʈ�� �߰��� �Լ�. ������ �ٲ𶧸��� Update���ִ� �Լ��Դϴ�. </summary>
    /// <param name="type">���������� �Ҹ�Ÿ��</param>
    public void VolumeChangeUpdate()
    {
        // ���⼭ ���������ų� �۵��ȵǸ� VolumeSlider Start�� SoundManager Awake���� ��������Ȱ���
        _volumeText.text = _slider.value.ToString();

       
    }


}
