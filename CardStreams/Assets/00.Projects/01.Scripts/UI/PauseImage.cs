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
        bool isPause = Time.timeScale == 0; // 게임 멈췄으면 true

        // Pause면 일시정지->플레이, 플레이중이면 플레이->일시정지로 아이콘 바꿈
        pauseImage.sprite = isPause ? playSprite : pauseSprite;
    }
}
