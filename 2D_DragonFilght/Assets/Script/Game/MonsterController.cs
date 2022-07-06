using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    //[SerializeField]
    float m_speed = 3f;
    int m_hp;
    int m_hpMax;
    int m_line;
    [SerializeField]
    Animator m_animator;
    MonsterManager.MonsterType m_type;
    public MonsterManager.MonsterType Type { get { return m_type; } }
    public int Line { get { return m_line; } set { m_line = value; } }

    public void InitMonster(MonsterManager.MonsterType type)
    {
        m_type = type;
        m_hpMax = ((int)type + 1) * 2;
        m_hp = m_hpMax;
    }
    public void Setmonster()
    {
        m_hp = m_hpMax;
    }

    public void SetDie()
    {
        if(GameUIManager.Instance != null)
            GameUIManager.Instance.SetHuntScore(((uint)m_type + 1) * 75);
        SoundManager.Instance.PlaySfx(SoundManager.SfxClip.Mon_die);
        ItemManager.Instance.CreateItem(transform.position);
        EffectManager.Instance.CreateEffect(transform.position);
    }

    public void SetDamage(int dmg)
    {
        if (dmg == -1)
            m_hp = 0;
        else
            m_hp -= dmg;
        //(애니메이션, 레이어, 시작시간(시간초기화))
        m_animator.Play("Hit", 0, 0f);
        if(m_hp<=0)
        {
            if (Type == MonsterManager.MonsterType.Bomb)
            {
                MonsterManager.Instance.RemoveMonstersLine(Line);
            }
            else
            {
                SetDie();
                m_animator.Play("Idle", 0, 0f);
                MonsterManager.Instance.RemoveMonster(this);
            }
        }
    }

    public void Move()
    {
        transform.position += Vector3.down * m_speed * MonsterManager.Instance.SpeedScale * Time.deltaTime;
    }

    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Collider_Bottom") /*|| collision.CompareTag("Fire")*/)
        {
            MonsterManager.Instance.RemoveMonster(this);
        }
        else if(collision.CompareTag("Invincible"))
        {
            this.SetDamage(-1);
        }
    }

}
