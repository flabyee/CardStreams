using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "new Missioin", menuName = "ScriptableObject/Mission/Mission")]
public abstract class MissionSO : ScriptableObject
{
    public Sprite missionSprite;
    public string missionName;
    [TextArea] 
    public string infoStr;      // �����ϰ� ���� ���� ǥ���ϱ� ���� �̼� ����
    [TextArea] 
    public string timInfoStr;   // ���� �̼� ����
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

    // ���⼭ ������ ����� �׼ǿ� �ڱ� �߰�
    // �Ű����� ��� �ٸ��� ������ �˾Ƽ� ����
    // ����
    //GameManager.Instance.player.OnBasicCardEvent += ObserverUseBasicCard;
    public abstract void GetMission();


    // �߰��Ϸ��� ���� ��� �´� �Լ� �����
    // ����
    private void ObserverUseBasicCard(BasicCard basicCard)
    {
        // �̷������� �Լ��ϳ� ����� ���� ��
    }

    // UI ��� �ٸ�����? ������ �˾Ƽ� ����
    public abstract void ApplyUI();

    public abstract bool IsComplete();

    // GetMisiion���� �߰��� �̺�Ʈ�� Reset���� ����
    // ����
    //GameManager.Instance.player.OnBasicCardEvent -= ObserverUseBasicCard;
    public abstract void Reset();
}
