using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/Mission")]
public class MissionSO : ScriptableObject
{
    public Sprite missionSprite;
    public string missionName;
    public string infoStr;
    public MissionGrade grade;

    protected Slider progressSlider;
    protected TextMeshProUGUI progressText;

    public void SetUI(Slider progressSlider, TextMeshProUGUI progressText)
    {
        this.progressSlider = progressSlider;
        this.progressText = progressText;
    }
    public void UnSetUI()
    {
        this.progressSlider = null;
        this.progressText = null;
    }

    public virtual void GetMission()
    {
        // ���⼭ ������ ����� �׼ǿ� �ڱ� �߰�
        // �Ű����� ��� �ٸ��� ������ �˾Ƽ� ����
        // ����
        //GameManager.Instance.player.OnBasicCardEvent += ObserverUseBasicCard;
    }

    // �߰��Ϸ��� ���� ��� �´� �Լ� �����
    // ����
    private void ObserverUseBasicCard(BasicCard basicCard)
    {
        // �̷������� �Լ��ϳ� ����� ���� ��
    }

    // UI ��� �ٸ�����? ������ �˾Ƽ� ����
    public virtual void ApplyUI()
    {
        
    }

    public virtual bool IsComplete()
    {
        // GetMisiion���� �߰��� �̺�Ʈ�� IsComplete���� ����
        // ����
        GameManager.Instance.player.OnBasicCardEvent -= ObserverUseBasicCard;

        return false;
    }
}
