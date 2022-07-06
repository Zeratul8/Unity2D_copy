using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField]
    float m_speed = 15f;
    HeroController m_hero;
    int m_atk = 1;

    public void Initfire(HeroController hero)
    {
        m_hero = hero;
    }

    void Remove()
    {
        if (IsInvoking("Remove"))
            CancelInvoke("Remove");
        m_hero.RemoveFire(this);
    }
    
    void OnEnable()
    {
        //m_hero = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroController>();
        if (IsInvoking("Remove"))
            CancelInvoke("Remove");
        Invoke("Remove", 2f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            var mon = collision.gameObject.GetComponent<MonsterController>();
            mon.SetDamage(m_atk);
            //MonsterManager.Instance.RemoveMonster(mon);
            Remove();
        }
    }

    void Update()
    {
        transform.position += Vector3.up * m_speed * Time.deltaTime;
    }
}
