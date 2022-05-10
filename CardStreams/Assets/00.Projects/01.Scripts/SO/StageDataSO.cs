using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "ScriptableObject/Stage/StageData")]
public class StageDataSO : ScriptableObject
{
    public int id;
    public string mapStr;
    public List<CardData> deck;
    public int moveCount;       // �ѹ��� �̵��� ��
    public bool isDeckShuffle;  // ���� ������, Ʃ�丮�󿡼��� �ȼ��� �ϰ�;
    public int mobSpawnAmount;  // �ʱ� ��ȯ ����, �׸޴������� ���� �þ�°�
    public int mobIncreaseAmount;  // �� �þ�� ��
    public int mobAttackAmount;            // ���� ���ݷ�
    public int mobAttackIncreaseAmount;     // �� ���ݷ� ������
}
