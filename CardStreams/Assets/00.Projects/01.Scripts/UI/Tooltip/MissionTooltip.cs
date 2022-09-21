using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionTooltip : MonoBehaviour
{
    public static MissionTooltip instance;

    public TextMeshProUGUI missionNameText;
    public TextMeshProUGUI missionInfoText;

    public TextMeshProUGUI rewardText;

    [Header("Debug")]
    public float xOffset = 0;

    private TooltipTimer tooltipTimer;

    private void Awake()
    {
        instance = this;

        Hide();
    }

    private void Update()
    {
        // 툴팁 타이머 처리
        if (tooltipTimer != null)
        {
            tooltipTimer.timer -= Time.deltaTime;
            if (tooltipTimer.timer <= 0)
            {
                Hide();
            }
        }
    }

    public void Show(string missionName, string missionInfo, string rewardInfo,  Vector3 pos, TooltipTimer tooltipTimer = null)
    {
        // 타이머 세팅
        this.tooltipTimer = tooltipTimer;

        missionNameText.text = missionName;
        missionInfoText.text = missionInfo;
        rewardText.text = rewardInfo;

        transform.position = pos + new Vector3(xOffset, 0, 0);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public class TooltipTimer
    {
        public float timer;

        public TooltipTimer(float time)
        {
            timer = time;
        }
    }
}
