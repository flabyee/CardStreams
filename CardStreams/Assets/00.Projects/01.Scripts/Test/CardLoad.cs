using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardLoad : MonoBehaviour
{
    private List<Image> _imgList;
    [SerializeField] Transform _cardStartPos;
    [SerializeField] Transform _cardEndPos;

    public void AddList(Image obj) // ī�帮��Ʈ�� ���
    {
        _imgList.Add(obj);
    }

    public void RemoveList(Image obj)
    {
        if (_imgList.Contains(obj))
            _imgList.Remove(obj);
    }

    public void AlignCards() // ī�� ��������
    {
        Vector3 delta = _cardEndPos.transform.position - _cardStartPos.transform.position;

        Vector3 stepPos = delta / 4;  //4��� ����

        Vector3 cp1 = _cardStartPos.transform.position + stepPos + new Vector3(0, 2f);
        Vector3 cp2 = _cardEndPos.transform.position - stepPos + new Vector3(0, 2f);

        int index = 5; //5~��-4������ �ε����� ����ϴϱ� �̷���
        int step = 2;

        Vector3[] points = DOCurve.CubicBezier.GetSegmentPointCloud(_cardStartPos.position, cp1, _cardEndPos.position, cp2, (_imgList.Count - 1) * step + 10);


        for(int i = 0; i < _imgList.Count; i++)
        {
            _imgList[i].transform.position = points[index];
            index += step;
        }

        // ȸ��

        for (int i = 0; i <= _imgList.Count / 2 - 1; i++) // 4�� 1 5���� 1 6�̸� 2
        {
            Vector3 normal = (_imgList[i + 1].transform.position - _imgList[i].transform.position).normalized;

            float cos = normal.x;
            float sin = normal.y;
            float theta = Mathf.Atan2(sin,cos) * Mathf.Rad2Deg;

            _imgList[i].transform.Rotate(new Vector3(0, 0, theta));
        }

        for (int i = _imgList.Count - 1; i >= (_imgList.Count + 1) / 2; i--) // 4�� 32 5�� 43 6�̸� 543
        {
            Vector3 normal = (_imgList[i].transform.position - _imgList[i-1].transform.position).normalized;

            float cos = normal.x;
            float sin = normal.y;
            float theta = Mathf.Atan2(sin, cos) * Mathf.Rad2Deg;

            _imgList[i].transform.Rotate(new Vector3(0, 0, theta));
        }
    }
}
