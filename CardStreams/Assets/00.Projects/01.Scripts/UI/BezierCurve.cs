using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    [Header("�׽�Ʈ��")]
    [SerializeField] bool isTest; // ��� �׽�Ʈ�ҰŸ� �̰� true�� �ٲٸ���
    [SerializeField] GameObject testTarget;
    
    [Header("������� �����Ȱ�")]
    [SerializeField] [Range(0, 1)] float time = 0;
    [SerializeField] float speed = 5;
    [SerializeField] float radiusA = 0.55f;
    [SerializeField] float radiusB = 0.45f;

    private List<Vector2> point = new List<Vector2>(); // ��� �̷�� ����
    private Vector3 objectPos; // �����Ұ�
    private bool initComplete = false;
    private Vector3 direction = Vector3.zero; // ��ǥ�� �ִ� ����

    private void Start()
    {
        if (isTest) StartBezier(testTarget);
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

    public void StartBezier(GameObject target)
    {
        ThreeBezierInit(target.transform.position - transform.position); // ������ ����
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
            radiusA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x,
            radiusA * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad + origin.y)
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
            radiusB * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x,
            radiusB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad + origin.y)
            );

    }

    #endregion
}
