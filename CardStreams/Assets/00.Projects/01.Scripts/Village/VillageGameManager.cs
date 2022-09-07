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
            Debug.LogError("VillageGameManager가 중복되었습니다");
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
            // 플레이어가 위치한 필드의 UI창 상호작용
            foreach (BuildCard build in player.curField.accessBuildList)
            {
                VillageBuildSO villageSO = build.buildSO as VillageBuildSO;
                if(villageSO != null)
                {
                    MenuManager.Instance.OpenMenu(villageSO.openMenuType);
                    OpenUI();
                    break;
                }

                // UI창 열리는 건물이면(build.isCardUIShow?) 열고 foreach break
                // blur 배경도 띄우면 좋겠다
                // 근데 이방법으로는 2번째건물 못쓰니까 나중엔 다른방법을 찾아야됨
            }
        }
    }

    public void Init() // afterMapCreate 0.03초 늦게부르면 버그안생기고 바로부르면 위치버그생김 뭐지??????????
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

    public void OnClickPlayerAction() // 나중엔 자동으로 돌아가게해야함
    {
        player.MoveStart();
    }

    public void IntoDungeon() // 던전들어가기
    {
        SaveFile.SaveGame();
    }
}
