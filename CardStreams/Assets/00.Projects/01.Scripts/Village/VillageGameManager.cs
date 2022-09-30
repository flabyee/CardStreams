using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VillageGameManager : MonoBehaviour
{
    public static VillageGameManager Instance;

    [Header("Player")]
    public VillagePlayer player;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("VillageGameManager�� �ߺ��Ǿ����ϴ�");
            Destroy(this.gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        SaveFile.SaveGame();
    }

    public void Init() // afterMapCreate 0.03�� �ʰԺθ��� ���׾Ȼ���� �ٷκθ��� ��ġ���׻��� ����??????????
    {
        Vector3 movePos = VillageMapManager.Instance.fieldList[VillageMapManager.Instance.fieldCount - 1].transform.position;

        player.transform.DOMove(movePos, 0.25f);

    }

    public void OpenUI()
    {
        player.Stop();
    }

    public void CloseUI()
    {
        player.MoveStart();
    }

    public void OnClickPlayerAction() // ���߿� �ڵ����� ���ư����ؾ���
    {
        player.MoveStart();
    }

    public void IntoDungeon() // ��������
    {
        SaveFile.SaveGame();
    }
}
