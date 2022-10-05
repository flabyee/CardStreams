using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �Ⱦ��� ��ũ��Ʈ, ui�гε����� ����ϰ��ϸ� ��������? �����Ű��Ƽ�����
// Show, Hide, ShowOrHide ������
[RequireComponent(typeof(CanvasGroup))] 
public class Panel : MonoBehaviour
{
    private CanvasGroup _cg;
    private bool isActive;

    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
        Hide();
    }

    public virtual void Show()
    {
        _cg.alpha = 1;
        _cg.interactable = true;
        _cg.blocksRaycasts = true;

        isActive = true;
    }

    public virtual void Hide()
    {
        _cg.alpha = 0;
        _cg.interactable = false;
        _cg.blocksRaycasts = false;

        isActive = false;
    }

    public virtual void ShowOrHide()
    {
        if(isActive) // on -> off
        {
            Hide();
        }
        else // off -> on
        {
            Show();
        }
    }
}
