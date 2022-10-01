using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VillageShopItem : MonoBehaviour
{
    public static Vector2Int buildPos;
    public VillageBuildSO buildItemSO;
    
    [SerializeField] VillageShop shop;
    [SerializeField] Image buildImage;
    [SerializeField] TextMeshProUGUI buildName;
    [SerializeField] TextMeshProUGUI detailText;
    [SerializeField] TextMeshProUGUI requireCrystalText;

    private Button thisButton;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
    }

    public void Init(BuildSO so)
    {
        buildItemSO = so as VillageBuildSO;
        if (buildItemSO == null)
        {
            Debug.LogError("마을 건물을 받아오지 못했습니다");
            return;
        }

        buildImage.sprite = so.sprite;
        buildName.text = so.buildName;
        detailText.text = so.tooltip;
        requireCrystalText.text = so.price.ToString();
    }

    public void OnClickBuy()
    {
        // 크리스탈 충분 체크
        if (ResourceManager.Instance.UseResource(ResourceType.crystal, buildItemSO.price) == false) return;

        // buildItemSO를 깔기
        CreateNpcBuilding();
        shop.OnOffMenu(); // 샀으니까 사이드 창은 끄기
    }
    public void SetInteractable(bool isInteractable)
    {
        thisButton.interactable = isInteractable;
    }

    private void CreateNpcBuilding()
    {
        RectTransform rectTrm = VillageMapManager.Instance.GetMapRectTrm(buildPos.y, buildPos.x);
        BuildCard building = CardPoolManager.Instance.GetBuildCard(rectTrm).GetComponent<BuildCard>();
        building.transform.position = rectTrm.position;

        building.Init(buildItemSO);
        building.VillageBuildDrop(new Vector2(buildPos.x, buildPos.y));

        CardPower cardPower = building.GetComponent<CardPower>();
        cardPower.OnField();
    }
}
