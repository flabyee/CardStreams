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
    public MainMenu mainMenuPrefab;
    public EquipMenu equipMenuPrefab;
    public UnlockMenu unlockMenuPrefab;


    // 메뉴들의 부모로 쓸 트랜스폼 변수 선언
    [SerializeField]
    private Transform _menuParent;

    // 스택으로 캔버스 메뉴들을 관리할거임
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

                //첫 오픈하는 메뉴는 메인메뉴로 하겠다
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
}