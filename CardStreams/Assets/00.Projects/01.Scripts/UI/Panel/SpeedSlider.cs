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
        _speed = Mathf.Abs(_slider.value - 1);
        _speedText.text = $"{0.05f + _slider.value / 4f}";

        GameManager.Instance.SetMoveDuration(0.05f + _speed / 3f);
    }
}
