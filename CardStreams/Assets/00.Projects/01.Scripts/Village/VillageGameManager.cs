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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �÷��̾ ��ġ�� �ʵ��� UIâ ��ȣ�ۿ�
            foreach (BuildCard build in player.curField.accessBuildList)
            {
                VillageBuildSO villageSO = build.buildSO as VillageBuildSO;
                if(villageSO != null)
                {
                    MenuManager.Instance.OpenMenu(villageSO.openMenuType);
                    OpenUI();
                    break;
                }

                // UIâ ������ �ǹ��̸�(build.isCardUIShow?) ���� foreach break
                // blur ��浵 ���� ���ڴ�
                // �ٵ� �̹�����δ� 2��°�ǹ� �����ϱ� ���߿� �ٸ������ ã�ƾߵ�
            }
        }
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
