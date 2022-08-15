using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bossHpText;

    public int Hp  { get; set; }     // 보스의 현재 체력
    public int Attack { get; set; }    // 보스와 주고받는 공격량

    public void Init(int hp, int atk, Vector3 pos)
    {
        this.Hp = hp;
        this.Attack = atk;
        HpUpdate();

        Effects.Instance.TriggerTeleport(pos);
        transform.position = pos;
    }

    public void OnDamage(int damage)
    {
        Hp -= damage;
        HpUpdate();

        if (Hp <= 0)
        {
            Debug.Log("보스 사망");
        }
    }

    public void MovePos(Vector3 pos, float duration = 1f)
    {
        transform.DOMove(pos, duration);
    }

    private void HpUpdate()
    {
        bossHpText.text = Hp.ToString();
    }
}
