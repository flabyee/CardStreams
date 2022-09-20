using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnNpcButton : MonoBehaviour
{
    // Button UI 속성
    [SerializeField] Image npcImage;
    [SerializeField] TextMeshProUGUI npcNameText;
    [SerializeField] TextMeshProUGUI requirePrestigeText;
    [SerializeField] TextMeshProUGUI requireCrystalText;

    // NPCData SO 속성
    [SerializeField] NpcSpawnDataSO npcDataSO;
    private VillageNPCHouseSO npcBuildSO;
    private Vector2Int npcSpawnPos;
    private int requirePrestige;
    private int requireCrystal;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        npcBuildSO = npcDataSO.npcBuildSO;
        npcSpawnPos = npcDataSO.npcSpawnPos;
        requirePrestige = npcDataSO.requirePrestige;
        requireCrystal = npcDataSO.requireCrystal;

        requirePrestigeText.text = requirePrestige.ToString();
        requireCrystalText.text = requireCrystal.ToString();

        var npcSO = npcBuildSO.npcSO;
        npcImage.sprite = npcSO.npcSprite;
        npcNameText.text = npcSO.npcName;
    }

    public void SpawnNpc() // 버튼 Event에 넣고 누르면 NPC가 나옴(자원없으면 안나옴)
    {
        if (ResourceManager.Instance.prestige >= requirePrestige == false)
            return;

        if (ResourceManager.Instance.UseResource(ResourceType.crystal, requireCrystal) == false)
            return;

        CreateNpc();
    }

    private void CreateNpc()
    {
        RectTransform rectTrm = VillageMapManager.Instance.GetMapRectTrm(npcSpawnPos.y, npcSpawnPos.x);
        BuildCard building = CardPoolManager.Instance.GetBuildCard(rectTrm).GetComponent<BuildCard>();
        building.transform.position = rectTrm.position;

        building.Init(npcBuildSO);
        building.VillageBuildDrop(new Vector2(npcSpawnPos.x, npcSpawnPos.y));

        CardPower cardPower = building.GetComponent<CardPower>();
        cardPower.backImage.color = Color.magenta;
        cardPower.OnField();
    }
}