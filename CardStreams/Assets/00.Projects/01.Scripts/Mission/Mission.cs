using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum MissionType
{
    Now,
    AfterLoop,
}

public enum MissionGrade
{
    Easy,
    Normal,
    Hard,
}

public class Mission : MonoBehaviour
{
    // UI�� �ʼ����� �κи� ���⼭ �����ϰ� �������� �˾Ƽ�
    public TextMeshProUGUI missionNameText;
    public TextMeshProUGUI missionInfoText;

    public virtual void Complete()
    {
        // ����
    }

    public virtual void GetMission()
    {
        // ���⼭ ������ ����� �׼ǿ� �ڱ� �߰�
        // �Ű����� ��� �ٸ��� ������ �˾Ƽ� ����

        // UI�� ����
        ApplyUI();
    }

    public virtual void ApplyUI()
    {

    }
}
