using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
    [SerializeField] Slider _slider;

    [Header("테스트")]
    [SerializeField] float _speed = 50; // 딱절반 : 100(느림) ~ 0(빠름)

    public void ChangeSpeed()
    {
        _speed = Mathf.Abs(_slider.value - 100);
        //_speedText.text = $"{0.05f + _slider.value / 4f}";

        GameManager.Instance.SetMoveDuration(0.05f + _speed / 300f);
    }
}
