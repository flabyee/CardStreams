using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseImage : MonoBehaviour
{
    [SerializeField] Image pauseImage;
    [SerializeField] Sprite playSprite;
    [SerializeField] Sprite pauseSprite;

    public void PauseGame()
    {
        bool isPause = Time.timeScale == 0; // ���� �������� true

        // Pause�� �Ͻ�����->�÷���, �÷������̸� �÷���->�Ͻ������� ������ �ٲ�
        pauseImage.sprite = isPause ? playSprite : pauseSprite;
    }
}
