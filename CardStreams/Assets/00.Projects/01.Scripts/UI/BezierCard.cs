using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BezierCard : MonoBehaviour
{
    [Header("베지어그래프 설정")]
    [SerializeField] [Range(0, 1)] float time = 0;
    [SerializeField] float speed = 1.5f;
    [SerializeField] float radiusA = 0.55f;
    [SerializeField] float radiusB = 0.45f;
    
    [Header("UI")]
    [SerializeField] Image cardIconImage;


    private RectTransform _rectTrm;
    private CanvasGroup _cg;
    private List<Vector2> point = new List<Vector2>(); // 곡선을 이루는 점들
    private Vector3 objectPos; // 도달할곳
    private bool initComplete = false;
    private Vector3 direction = Vector3.zero; // 목표가 있는 방향

    private void Awake()
    {
        _rectTrm = GetComponent<RectTransform>();
        _cg = GetComponent<CanvasGroup>();
    }

    void Update() // 곡선 형식대로 물체를 움직여요
    {
        if (initComplete)
        {
            if (time > 1)
                return;
            time += Time.deltaTime * speed;
            transform.position = MoveThreeBezier();
            
            // transform.position = MoveFourBezier(); // 3차 쓸때는 이걸 써야해요
        }
    }

    public void StartBezier(Transform targetTrm)
    {
        ThreeBezierInit(targetTrm.position - transform.position); // 2차 베지어 시작
    }

    /// <summary> 던질카드 Init </summary>
    /// <param name="targetTrm">카드가 날라갈 목적지</param>
    /// <param name="icon">카드 아이콘</param>
    /// <param name="cardID">획득할 카드 ID</param>
    /// <param name="callback">끝까지 날라갔을때 추가로실행할 함수</param>
    public void Init(Transform targetTrm, Sprite icon, Action callback)
    {
        cardIconImage.sprite = icon;

        Sequence seq = DOTween.Sequence();

        seq.Append(_rectTrm.DORotate(new Vector3(0, 0, 135f), 1f / speed));
        seq.Join(_rectTrm.DOScale(0.15f, 1f / speed).OnComplete( () =>
        {
            callback?.Invoke();
            _cg.alpha = 0;
        })); // 도착하면 일단 끄고 완료 Action 실행
        seq.AppendInterval(0.2f).OnComplete(() => Destroy(gameObject));
         // 잠시 기다렸다가 삭제(가 아닌 풀매니저로해야함, 기다리는이유 = trail 자연스럽게)

        StartBezier(targetTrm);
    }

    #region 2차 베지어 곡선

    public void ThreeBezierInit(Vector3 dir) // 점3개(2차) 베지어곡선 점 설정
    {
        direction = dir;
        objectPos = transform.position + direction;

        point.Add(transform.position);
        point.Add(SetRandomBezierPointP2(transform.position));
        point.Add(objectPos);
        initComplete = true;
    }

    private Vector2 MoveThreeBezier() // 공식에 t를 넣어서 곡선위에서의 현재위치를 얻어내요 t=시간
    {
        return new Vector2(
            ThreePointBezier(point[0].x, point[1].x, point[2].x),
            ThreePointBezier(point[0].y, point[1].y, point[2].y)
            );
    }
    

    private float ThreePointBezier(float a, float b, float c) // a=시작점 b=가운데랜덤 c=목적지
    {
        float Mt = (1 - time) * a + time * b;
        float Nt = (1 - time) * b + time * c;
        return (1 - time)* Mt + time * Nt;
    }

    Vector2 SetRandomBezierPointP2(Vector2 origin) // 시작-끝 사이의 점1개를 만들어요(랜덤아님)
    {
        return new Vector2(
            radiusA * Mathf.Cos(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x,
            radiusA * Mathf.Sin(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad + origin.y)
            );
    }

    #endregion

    #region 3차 베지어 곡선

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
