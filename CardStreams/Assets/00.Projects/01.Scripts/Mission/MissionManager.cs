using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    // �̼� ���� ����
    // �̼� UI ����, ����, ��ġ, ���� ��� (���� ������Ʈ�� �̼ǿ��� ��)
    // ���� ���� Ŭ���� �˻��ϴ� �̼� �˻����ֱ�

    public static MissionManager instance;
    public GameObject missionPrefab;
    public RectTransform missionParentTrm;

    private List<Mission> missionList;

    // ���� ������ �� ���� �̼� 3�� ȹ��
    public void GetRandomMission()
    {

    }

    // ���� ���� �� �̼ǵ� Ŭ���� ���� Ȯ���ϰ� ���� ȹ��
    public void IsCompleteMission()
    {

    }
}
