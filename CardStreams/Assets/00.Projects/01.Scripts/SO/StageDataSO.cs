using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "ScriptableObject/Stage/StageData")]
public class StageDataSO : ScriptableObject
{
    [Header("�⺻ ����")]
    public int id;
    public bool isRandomMap;
    [TextArea(minLines:6, maxLines:6)] public string mapStr;
    public int moveCount;       // �ѹ��� �̵��� ��

    [Header("�� ���� ����")]
    public bool isDeckShuffle;  // ���� ������, Ʃ�丮�󿡼��� �ȼ��� �ϰ�;
    public int firstDeckValueAmount;  // �ʱ� ����ġ ��
    public int deckValueIncreaseAmount; // ����ġ �پ��� ��
    public float deckValueIncreaseMultipication;  // ����ġ �پ��� ���� ����


    [Header("���� �� ���� ����")]
    public int firstMobSpawnAmount;  // �ʱ� ��ȯ ����, �׸޴������� ���� �þ�°�
    public int mobIncreaseAmount;  // �� �þ�� ��
    public int firstMobAttackAmount;            // ���� ���ݷ�
    public int mobAttackIncreaseAmount;     // �� ���ݷ� ������

    [Header("���� ���� ����")]
    public int bossRound;   // ������ ������ ������
    public int bossHP;      // ���� ü��
    public int bossAtk;     // ������ ���ݷ�
    public int spawnEnemyAtk;   // ��ȯ�ϴ� ���� ���ݷ�
}
