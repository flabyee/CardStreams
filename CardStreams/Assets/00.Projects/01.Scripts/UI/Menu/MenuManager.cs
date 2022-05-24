using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class MenuManager : MonoBehaviour
{
    // 필요한 메뉴 프리팹들을 변수로 선언
    //public MainMenu mainMenuPrefab;
    //public InformationMenu informationMenuPrefab;
    //public ReinforcementMenu reinforcementMenu;
    //public RevivalMenu revivalMenu;
    //public DungeonJoinMenu dungeonJoinMenu;
    //public ShopMenu shopMenu;
    //public Menu2 menu2Prefab;


    // 메뉴들의 부모로 쓸 트랜스폼 변수 선언
    [SerializeField]
    private Transform _menuParent;

    // 스택으로 캔버스 메뉴들을 관리할거임
    private Stack<Menu> _menuStack = new Stack<Menu>();

    private static MenuManager _instance;
    public static MenuManager Instance { get { return _instance; } }


    [SerializeField]
    private Image fadeInOutImage;


    // To Do : 나중에 빌리지 매니저로 옮기는게 맞는듯?
    public GoldTextClass shopGT;    // 상점에서 사용하는 재화
    public GoldTextClass reinforceGT;  // 강화에 사용하는 재화
    public GoldTextClass spendGT;

    public GameObject moneyObj;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            InitMenu();
            //DontDestroyOnLoad(this.gameObject);
        }


    }

    private void Start()
    {
        Instance.spendGT.FirstInit(0, false, GoldTextClass.GoldType.reinforceGold);
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private void InitMenu()
    {
        // 메뉴 시스템 초기화 로직
        if (_menuParent == null)
        {
            GameObject menuParentObject = new GameObject("Menus");
            _menuParent = menuParentObject.transform;
        }

        //DontDestroyOnLoad(_menuParent.gameObject);

        // 리플렉션을 통한 함수 타입을 얻어와서 통합시켜주는 코드
        System.Type myType = this.GetType();
        BindingFlags myFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;  // DeclaredOnly : 선언된 것 만
        FieldInfo[] fields = myType.GetFields(myFlag);  // 저 3개의 flag에 부합하는 필드는 menuPrafab들이다

        // 실제로 들어온 필드들을 확인용 코드 (디버깅용)
        for (int i = 0; i < fields.Length; i++)
        {
            //print("필드 : " + fields[i]);
        }

        foreach (FieldInfo field in fields)
        {
            Menu prefab = field.GetValue(this) as Menu;


            // 메뉴가 추가될 때마다 수동으로 메뉴프리팹들을 추가해주는 코드
            //Menu[] menuPrefabs = { mainMenuPrefab, optionMenuPrefab, creditMenuPrefab };
            //foreach(Menu prefab in menuPrefabs)
            //{
            // 이 작업이 있어야 menu들이 Awake되서 Instance 생김
            if (prefab != null)
            {
                Menu menuInstance = Instantiate(prefab, _menuParent);

                // 첫 오픈하는 메뉴는 메인메뉴로 하겠다
                //if (prefab != mainMenuPrefab)
                //{
                //    menuInstance.gameObject.SetActive(false);
                //}
                //else
                //{
                //    OpenMenu(menuInstance);
                //}
            }
        }
    }

    public void OpenMenu(Menu menuInstance)
    {
        if (menuInstance == null)
        {
            Debug.Log("메뉴 인스턴스가 존재안함");
            return;
        }

        if (_menuStack.Count > 0)
        {
            foreach (Menu menu in _menuStack)
            {
                menu.gameObject.SetActive(false);
            }
        }

        menuInstance.gameObject.SetActive(true);
        menuInstance.OnOpen();

        _menuStack.Push(menuInstance);
    }

    // menu를 열때 id 전달할수있게
    public void OpenMenu(Menu menuInstance, int index)
    {
        if (menuInstance == null)
        {
            Debug.Log("메뉴 인스턴스가 존재안함");
            return;
        }

        if (_menuStack.Count > 0)
        {
            foreach (Menu menu in _menuStack)
            {
                menu.gameObject.SetActive(false);
            }
        }

        menuInstance.gameObject.SetActive(true);
        menuInstance.OnOpen();

        _menuStack.Push(menuInstance);
    }

    public void CloseMenu()
    {
        if (_menuStack.Count == 0)
        {
            Debug.Log("메뉴 스택에 아무것도 존재 안함");
            return;
        }


        // 마지막에 열린 메뉴를 꺼내고 제거
        Menu topMenu = _menuStack.Pop();
        topMenu.gameObject.SetActive(false);

        if (_menuStack.Count > 0)
        {
            // 바로 그 다음 메뉴를 꺼내서 활성화(제거 No)
            Menu nextMenu = _menuStack.Peek();  // Pop은 제거까지 함, Peek은 꺼내기만
            nextMenu.gameObject.SetActive(true);
        }
    }

    public void FadeInOut()
    {
        fadeInOutImage.DOColor(new Color(0, 0, 0, 1), 0.5f).SetDelay(0.25f);
        fadeInOutImage.DOColor(new Color(0, 0, 0, 0), 0.5f).SetDelay(0.75f);
    }

    public void OnOffMoney(bool b)
    {
        moneyObj.SetActive(b);
    }

    public void SetSpendText(TextMeshProUGUI spendText)
    {
        spendGT.setText = spendText;
    }
    public void GoldApplyAnim(GoldTextClass gt)
    {
        StartCoroutine(UpDownGoldCor(gt));
    }
    public IEnumerator UpDownGoldCor(GoldTextClass gt)
    {
        if (gt.gold < gt.targetGold)   // 골드가 올라갈 때
        {
            while (gt.gold <= gt.targetGold)
            {
                // To Do : 사운드
                gt.gold += gt.amount;
                gt.ApplyText(GoldTextClass.GoldType.reinforceGold);
                yield return null;
            }

            gt.gold = gt.targetGold;
            gt.ApplyText(GoldTextClass.GoldType.reinforceGold);
        }
        else if (gt.gold > gt.targetGold)  // 골드가 내려갈 때
        {
            while (gt.gold >= gt.targetGold)
            {
                // To Do : 사운드
                gt.gold -= gt.amount;
                gt.ApplyText(GoldTextClass.GoldType.reinforceGold);
                yield return null;
            }

            gt.gold = gt.targetGold;
            gt.ApplyText(GoldTextClass.GoldType.reinforceGold);
        }
    }
}

// gold와 targetGold는 일치되어있어야 함
[System.Serializable]
public class GoldTextClass
{
    public int gold;
    public int targetGold;  // 중요! 골드와 타겟골드는 동기화(똑같)되어야 한다.
    public int amount = 100;  // 골드 줄어들거나 늘어나는 에니메이션에서 변경 속도? 양? 이다   // To Do : 변화 총량에 따라 자동으로 설정
    public TextMeshProUGUI setText;

    public enum GoldType
    {
        reinforceGold,
        shopGold,
    }

    public void ApplyText(GoldType goldType)   // 적용
    {
        setText.text = $"<sprite={(int)goldType}>" + gold.ToString();
    }
    public void SetColor(Color color)
    {
        setText.color = color;
    }

    public void UpGold(int amount)
    {
        targetGold += amount;
    }

    public void DownGold(int amount)
    {
        targetGold -= amount;
    }

    public void ResetGold()
    {
        targetGold = 0;
    }

    public void ApplyGold()
    {
        gold = targetGold;
    }

    public void FirstInit(int _gold, bool isApply, GoldType goldType)
    {
        this.gold = _gold;
        targetGold = gold;
        if (isApply)
        {
            ApplyText(goldType);
        }
    }
}
