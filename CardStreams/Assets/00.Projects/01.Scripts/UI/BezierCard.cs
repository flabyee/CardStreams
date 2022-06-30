using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BezierCard : MonoBehaviour
{
    [Header("������׷��� ����")]
    [SerializeField] [Range(0, 1)] float time = 0;
    [SerializeField] float speed = 1.5f;
    [SerializeField] float radiusA = 0.55f;
    [SerializeField] float radiusB = 0.45f;
    
    [Header("UI")]
    [SerializeField] Image cardIconImage;


    private RectTransform _rectTrm;
    private CanvasGroup _cg;
    private List<Vector2> point = new List<Vector2>(); // ��� �̷�� ����
    private Vector3 objectPos; // �����Ұ�
    private bool initComplete = false;
    private Vector3 direction = Vector3.zero; // ��ǥ�� �ִ� ����

    private void Awake()
    {
        _rectTrm = GetComponent<RectTransform>();
        _cg = GetComponent<CanvasGroup>();
    }

    void Update() // � ���Ĵ�� ��ü�� ��������
    {
        if (initComplete)
        {
            if (time > 1)
                return;
            time += Time.deltaTime * speed;
            transform.position = MoveThreeBezier();
            
            // transform.position = MoveFourBezier(); // 3�� ������ �̰� ����ؿ�
        }
    }

    public void StartBezier(Transform targetTrm)
    {
        ThreeBezierInit(targetTrm.position - transform.position); // 2�� ������ ����
    }

    /// <summary> ����ī�� Init </summary>
    /// <param name="targetTrm">ī�尡 ���� ������</param>
    /// <param name="icon">ī�� ������</param>
    /// <param name="cardID">ȹ���� ī�� ID</param>
    /// <param name="callback">������ �������� �߰��ν����� �Լ�</param>
    public void Init(Transform targetTrm, Sprite icon, Action callback)
    {
        cardIconImage.sprite = icon;

        Sequence seq = DOTween.Sequence();

        seq.Append(_rectTrm.DORotate(new Vector3(0, 0, 135f), 1f / speed));
        seq.Join(_rectTrm.DOScale(0.15f, 1f / speed).OnComplete( () =>
        {
            callback?.Invoke();
            _cg.alpha = 0;
        })); // �����ϸ� �ϴ� ���� �Ϸ� Action ����
        seq.AppendInterval(0.2f).OnComplete(() => Destroy(gameObject));
         // ��� ��ٷȴٰ� ����(�� �ƴ� Ǯ�Ŵ������ؾ���, ��ٸ������� = trail �ڿ�������)

        StartBezier(targetTrm);
    }

    #region 2�� ������ �

    public void ThreeBezierInit(Vector3 dir) // ��3��(2��) ������ �� ����
    {
        direction = dir;
        objectPos = transform.position + direction;

        point.Add(transform.position);
        point.Add(SetRandomBezierPointP2(transform.position));
        point.Add(objectPos);
        initComplete = true;
    }

    private Vector2 MoveThreeBezier() // ���Ŀ� t�� �־ ��������� ������ġ�� ���� t=�ð�
    {
        return new Vector2(
            ThreePointBezier(point[0].x, point[1].x, point[2].x),
            ThreePointBezier(point[0].y, point[1].y, point[2].y)
            );
    }
    

    private float ThreePointBezier(float a, float b, float c) // a=������ b=������� c=������
    {
        float Mt = (1 - time) * a + time * b;
        float Nt = (1 - time) * b + time * c;
        return (1 - time)* Mt + time * Nt;
    }

    Vector2 SetRandomBezierPointP2(Vector2 origin) // ����-�� ������ ��1���� ������(�����ƴ�)
    {
        return new Vector2(
            radiusA * Mathf.Cos(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x,
            radiusA * Mathf.Sin(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad + origin.y)
            );
    }

    #endregion

    #region 3�� ������ �

    public void FourBezierInit(Vector3 dir)
    {
        direction = dir;
        objectPos = transform.position + direction;

        point.Add(transform.position);
        point.Add(SetRandomBezierPointP2(transform.position));
        point.Add(SetRandomBezierPointP3(objectPos));
        point.Add(objectPos);
        initComplete = true;
    }

    private Vector2 MoveFourBezier()
    {
        return new Vector2(
            FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
            FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y)
            );
    }

    private float FourPointBezier(float a, float b, float c, float d)
    {
        return Mathf.Pow(1 - time, 3) * a
            + Mathf.Pow(1 - time, 2) * 3 * time * b
            + Mathf.Pow(time, 2) * 3 * (1 - time) * c
            + Mathf.Pow(time, 3) * d;
    }

    Vector3 SetRandomBezierPointP3(Vector3 origin)
    {
        return new Vector2(
            radiusB * Mathf.Cos(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x,
            radiusB * Mathf.Sin(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad + origin.y)
            );

    }

    #endregion
}
