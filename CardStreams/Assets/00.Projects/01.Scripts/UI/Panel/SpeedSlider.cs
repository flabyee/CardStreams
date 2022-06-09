using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _speedText;

    private float _speed = 0;

    public void ChangeSpeed()
    {
        _speed = _slider.value;
        _speedText.text = _speed.ToString();

        GameManager.Instance.SetMoveDuration(1 / (_speed * 10));
    }
}
