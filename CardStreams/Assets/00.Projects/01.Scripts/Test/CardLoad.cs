using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardLoad : MonoBehaviour
{
    private Image[] _imgList;
    private Transform _cardStartPos;
    private Transform _cardEndPos;

    private void Awake()
    {
         _imgList = GetComponentsInChildren<Image>();
        _cardStartPos = transform.Find("CardStartPosition");
        _cardEndPos = transform.Find("CardEndPostion");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            AlignCards();
        }
    }

    private void AlignCards()
    {
        Vector3 delta = _cardEndPos.transform.position - _cardStartPos.transform.position;

        Vector3 stepPos = delta / 4;  //4��� ����

        Vector3 cp1 = _cardStartPos.transform.position + stepPos + new Vector3(0, 2f);
        Vector3 cp2 = _cardEndPos.transform.position - stepPos + new Vector3(0, 2f);

        int index = 5; //5~��-4������ �ε����� ����ϴϱ� �̷���
        int step = 2;

        Vector3[] points = DOCurve.CubicBezier.GetSegmentPointCloud(_cardStartPos.position, cp1, _cardEndPos.position, cp2, (_imgList.Length - 1) * step + 10);

        Debug.Log(_imgList.Length);

        for(int i = 0; i < _imgList.Length; i++)
        {
            Debug.Log(index);
            Debug.Log("��ġ : " + points[index]);
            _imgList[i].transform.position = points[index];
            index += step;
        }

        // ȸ��

        for (int i = 0; i <= _imgList.Length / 2 - 1; i++) // 4�� 1 5���� 1 6�̸� 2
        {
            Vector3 normal = (_imgList[i + 1].transform.position - _imgList[i].transform.position).normalized;

            float cos = normal.x;
            float sin = normal.y;
            float theta = Mathf.Atan2(sin,cos) * Mathf.Rad2Deg;

            _imgList[i].transform.Rotate(new Vector3(0, 0, theta));
        }

        for (int i = _imgList.Length - 1; i >= (_imgList.Length + 1) / 2; i--) // 4�� 32 5�� 43 6�̸� 543
        {
            Vector3 normal = (_imgList[i].transform.position - _imgList[i-1].transform.position).normalized;

            float cos = normal.x;
            float sin = normal.y;
            float theta = Mathf.Atan2(sin, cos) * Mathf.Rad2Deg;

            _imgList[i].transform.Rotate(new Vector3(0, 0, theta));
        }
    }
}
