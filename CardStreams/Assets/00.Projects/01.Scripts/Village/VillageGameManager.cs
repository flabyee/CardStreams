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
