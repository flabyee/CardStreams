using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class MenuManager : MonoBehaviour
{
    // �ʿ��� �޴� �����յ��� ������ ����
    //public MainMenu mainMenuPrefab;
    //public InformationMenu informationMenuPrefab;
    //public ReinforcementMenu reinforcementMenu;
    //public RevivalMenu revivalMenu;
    //public DungeonJoinMenu dungeonJoinMenu;
    //public ShopMenu shopMenu;
    //public Menu2 menu2Prefab;


    // �޴����� �θ�� �� Ʈ������ ���� ����
    [SerializeField]
    private Transform _menuParent;

    // �������� ĵ���� �޴����� �����Ұ���
    private Stack<Menu> _menuStack = new Stack<Menu>();

    private static MenuManager _instance;
    public static MenuManager Instance { get { return _instance; } }


    [SerializeField]
    private Image fadeInOutImage;


    // To Do : ���߿� ������ �Ŵ����� �ű�°� �´µ�?
    public GoldTextClass shopGT;    // �������� ����ϴ� ��ȭ
    public GoldTextClass reinforceGT;  // ��ȭ�� ����ϴ� ��ȭ
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
        // �޴� �ý��� �ʱ�ȭ ����
        if (_menuParent == null)
        {
            GameObject menuParentObject = new GameObject("Menus");
            _menuParent = menuParentObject.transform;
        }

        //DontDestroyOnLoad(_menuParent.gameObject);

        // ���÷����� ���� �Լ� Ÿ���� ���ͼ� ���ս����ִ� �ڵ�
        System.Type myType = this.GetType();
        BindingFlags myFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;  // DeclaredOnly : ����� �� ��
        FieldInfo[] fields = myType.GetFields(myFlag);  // �� 3���� flag�� �����ϴ� �ʵ�� menuPrafab���̴�

        // ������ ���� �ʵ���� Ȯ�ο� �ڵ� (������)
        for (int i = 0; i < fields.Length; i++)
        {
            //print("�ʵ� : " + fields[i]);
        }

        foreach (FieldInfo field in fields)
        {
            Menu prefab = field.GetValue(this) as Menu;


            // �޴��� �߰��� ������ �������� �޴������յ��� �߰����ִ� �ڵ�
            //Menu[] menuPrefabs = { mainMenuPrefab, optionMenuPrefab, creditMenuPrefab };
            //foreach(Menu prefab in menuPrefabs)
            //{
            // �� �۾��� �־�� menu���� Awake�Ǽ� Instance ����
            if (prefab != null)
            {
                Menu menuInstance = Instantiate(prefab, _menuParent);

                // ù �����ϴ� �޴��� ���θ޴��� �ϰڴ�
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
            Debug.Log("�޴� �ν��Ͻ��� �������");
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

    // menu�� ���� id �����Ҽ��ְ�
    public void OpenMenu(Menu menuInstance, int index)
    {
        if (menuInstance == null)
        {
            Debug.Log("�޴� �ν��Ͻ��� �������");
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
            Debug.Log("�޴� ���ÿ� �ƹ��͵� ���� ����");
            return;
        }


        // �������� ���� �޴��� ������ ����
        Menu topMenu = _menuStack.Pop();
        topMenu.gameObject.SetActive(false);

        if (_menuStack.Count > 0)
        {
            // �ٷ� �� ���� �޴��� ������ Ȱ��ȭ(���� No)
            Menu nextMenu = _menuStack.Peek();  // Pop�� ���ű��� ��, Peek�� �����⸸
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
        if (gt.gold < gt.targetGold)   // ��尡 �ö� ��
        {
            while (gt.gold <= gt.targetGold)
            {
                // To Do : ����
                gt.gold += gt.amount;
                gt.ApplyText(GoldTextClass.GoldType.reinforceGold);
                yield return null;
            }

            gt.gold = gt.targetGold;
            gt.ApplyText(GoldTextClass.GoldType.reinforceGold);
        }
        else if (gt.gold > gt.targetGold)  // ��尡 ������ ��
        {
            while (gt.gold >= gt.targetGold)
            {
                // To Do : ����
                gt.gold -= gt.amount;
                gt.ApplyText(GoldTextClass.GoldType.reinforceGold);
                yield return null;
            }

            gt.gold = gt.targetGold;
            gt.ApplyText(GoldTextClass.GoldType.reinforceGold);
        }
    }
}

// gold�� targetGold�� ��ġ�Ǿ��־�� ��
[System.Serializable]
public class GoldTextClass
{
    public int gold;
    public int targetGold;  // �߿�! ���� Ÿ�ٰ��� ����ȭ(�Ȱ�)�Ǿ�� �Ѵ�.
    public int amount = 100;  // ��� �پ��ų� �þ�� ���ϸ��̼ǿ��� ���� �ӵ�? ��? �̴�   // To Do : ��ȭ �ѷ��� ���� �ڵ����� ����
    public TextMeshProUGUI setText;

    public enum GoldType
    {
        reinforceGold,
        shopGold,
    }

    public void ApplyText(GoldType goldType)   // ����
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
