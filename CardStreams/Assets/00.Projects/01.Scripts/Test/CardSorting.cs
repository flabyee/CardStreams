using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardSorting : MonoBehaviour
{
    private List<DragbleCard> _imgList = new List<DragbleCard>();
    [SerializeField] Transform _cardStartPos;
    [SerializeField] Transform _cardEndPos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AlignCards();
    }

    public void AddList(DragbleCard obj) // ī�帮��Ʈ�� ���
    {
        _imgList.Add(obj);
    }

    public void RemoveList(DragbleCard obj)
    {
        if (_imgList.Contains(obj))
            _imgList.Remove(obj);
    }

    private void InitCardSettings()
    {
        _imgList.Clear();

        // �ڽ��� �ڽ� ������Ʈ���� �ϱܾ�ͼ� �߸��ϸ� ����Ҽ�����, GetComponentsInChildren ����ȭ�� �ǿ��Ⳣ�ļ�(���ſ�) AddList RemoveList�� ��ü�ؾ���
        foreach (var item in GetComponentsInChildren<DragbleCard>())
        {
            _imgList.Add(item);
            item.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void AlignCards() // ī�� ��������
    {
        

        InitCardSettings();

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

        for (int i = 0; i < (_imgList.Count - 1) / 2; i++) // 4�� 1 5���� 1 6�̸� 2
        {
            Debug.Log(_imgList.Count);

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
