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
    public MainMenu mainMenuPrefab;
    public EquipMenu equipMenuPrefab;
    public UnlockMenu unlockMenuPrefab;


    // �޴����� �θ�� �� Ʈ������ ���� ����
    [SerializeField]
    private Transform _menuParent;

    // �������� ĵ���� �޴����� �����Ұ���
    private Stack<Menu> _menuStack = new Stack<Menu>();

    private static MenuManager _instance;
    public static MenuManager Instance { get { return _instance; } }

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

                //ù �����ϴ� �޴��� ���θ޴��� �ϰڴ�
                if (prefab != mainMenuPrefab)
                {
                    menuInstance.gameObject.SetActive(false);
                }
                else
                {
                    OpenMenu(menuInstance);
                }
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
}