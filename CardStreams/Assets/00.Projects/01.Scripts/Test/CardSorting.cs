using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardSorting : MonoBehaviour
{
    private List<CardPower> _imgList = new List<CardPower>();
    [SerializeField] Transform _cardStartPos;
    [SerializeField] Transform _cardEndPos;

    [SerializeField] int step = 5; // ī�� ���� ����(4-6������ �����ѵ�)


    public void AddList(CardPower obj) // ī�帮��Ʈ�� ���
    {
        if (_imgList.Contains(obj) == false)
            _imgList.Add(obj);
        else
            Debug.LogError("�̹� �ִ� ī�带 �ٽ� �߰��Ϸ� �߽��ϴ�");

        AlignCards();
    }

    public void RemoveList(CardPower obj)
    {
        if (_imgList.Contains(obj))
            _imgList.Remove(obj);
        else
            Debug.LogError("�������� �ʴ� ī�带 �����Ϸ� �߽��ϴ�");

        AlignCards();
    }

    private void InitCardSettings()
    {
        _imgList.Clear();

        // �ڽ��� �ڽ� ������Ʈ���� �ϱܾ�ͼ� �߸��ϸ� ����Ҽ�����, GetComponentsInChildren ����ȭ�� �ǿ��Ⳣ�ļ�(���ſ�) AddList RemoveList�� ��ü�ؾ���
        foreach (var item in GetComponentsInChildren<CardPower>())
        {
            _imgList.Add(item);
            item.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void AlignCards() // ī�� ��������
    {
        //InitCardSettings();

        // ȸ���� �ٽ� ����
        foreach(CardPower item in _imgList)
        {
            item.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        // �� �θ𿡼� ���ٰ�
        foreach (CardPower item in _imgList)
        {
            item.gameObject.transform.SetAsLastSibling();
        }

        Vector3 delta = _cardEndPos.transform.position - _cardStartPos.transform.position;

        Vector3 stepPos = delta / 4;  //4��� ����

        Vector3 cp1 = _cardStartPos.transform.position + stepPos + new Vector3(0, 1f);
        Vector3 cp2 = _cardEndPos.transform.position - stepPos + new Vector3(0, 1f);

        int index = 5; //5~��-4������ �ε����� ����ϴϱ� �̷���
        


        Vector3[] points = DOCurve.CubicBezier.GetSegmentPointCloud(_cardStartPos.position, cp1, _cardEndPos.position, cp2, (_imgList.Count - 1) * step + 10);

        int twoStep = 2;

        if(_imgList.Count == 2) // 2���϶��� ��ġ ����ؼ� ������ü
        {
            if (points.Length % 2 == 0) // ¦����(ex 14��) 14 / 2 = 7 => �߰��߿� ������, length/2 - 1 || length/2
            {
                _imgList[0].transform.position = points[points.Length / 2 - twoStep];
                _imgList[1].transform.position = points[points.Length / 2 - 1 + twoStep];
            }
            else if(points.Length % 2 == 1) // Ȧ����(ex 13��) 13 / 2 = 6 => ���, length/2 - 1 || length/2 + 1
            {
                _imgList[0].transform.position = points[points.Length / 2 - twoStep];
                _imgList[1].transform.position = points[points.Length / 2 + twoStep];
            }
            
        }
        else
        {
            for (int i = 0; i < _imgList.Count; i++)
            {
                //Debug.Log(index);
                _imgList[i].transform.position = points[index];
                index += step;
            }
        }

        // ȸ��

        for (int i = 0; i < (_imgList.Count - 1) / 2; i++) // 4�� 1 5���� 1 6�̸� 2
        {
            Vector3 normal = (_imgList[i + 1].transform.position - _imgList[i].transform.position).normalized;

            float cos = normal.x;
            float sin = normal.y;
            float theta = Mathf.Atan2(sin, cos) * Mathf.Rad2Deg;

            _imgList[i].transform.Rotate(new Vector3(0, 0, theta));
        }

        // 3 = 1 4 = 1 5 = 2 6 = 2 ((length-1)/2)

        for (int i = _imgList.Count - 1; i > _imgList.Count / 2; i--) // 4�� 3 5�� 43 6�̸� 54 (length           i < 2
        {
            Vector3 normal = (_imgList[i].transform.position - _imgList[i - 1].transform.position).normalized;

            float cos = normal.x;
            float sin = normal.y;
            float theta = Mathf.Atan2(sin, cos) * Mathf.Rad2Deg;

            _imgList[i].transform.Rotate(new Vector3(0, 0, theta));
        }
    }
}
