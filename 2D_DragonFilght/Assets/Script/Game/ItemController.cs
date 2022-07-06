using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer m_icon;
    [SerializeField]
    AnimationCurve m_posYCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField]
    AnimationCurve m_posXCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField]
    TweenRotation m_tweenRot;
    [SerializeField]
    Vector3 m_from;
    [SerializeField]
    Vector3 m_to;
    ItemManager.ItemType m_type;
    Transform m_target;

    bool m_isMagnet;
    float m_duration = 1f;
    float m_speed = 4f;
    const float maxHeight = 2f;
    const float maxDist = 13f;
    const float maxDuration = 5f;

    public void SetItem(ItemManager.ItemType type, Vector3 pos, Vector3 target, Sprite icon)
    {
        m_isMagnet = false;
        m_type = type;
        m_icon.sprite = icon;
        //m_tweenRot.enabled = false;
        //로테이션값 0으로 초기화!
        transform.rotation = Quaternion.identity;
        if (m_type > ItemManager.ItemType.Gem_Red && m_type <= ItemManager.ItemType.Gem_Blue)
        {
            m_tweenRot.enabled = true;
            m_tweenRot.ResetToBeginning();
            m_tweenRot.PlayForward();
        }
        StopAllCoroutines();
        gameObject.SetActive(true);
        var dir = target - pos;
        dir.y = 0f;
        m_from = pos;
        m_to = pos + dir.normalized * 0.3f;
        m_to.y = -7f;
        var dist = Vector3.Distance(m_from, m_to);
        var normalizedValue = dist / maxDist;
        m_duration = maxDuration * normalizedValue;
        StartCoroutine("Coroutine_PlayCurve");

    }

    IEnumerator Coroutine_PlayCurve()
    {
        float time = 0f;
        Vector3 dir = Vector3.zero;
        float yValue = 0f;
        float xValue = 0f;
        while(true)
        {
            if(!m_isMagnet)
            {
                time += Time.deltaTime / m_duration;
                yValue = m_posYCurve.Evaluate(time);
                xValue = m_posXCurve.Evaluate(time);
                dir = (m_from * (1f - xValue) + m_to * xValue) + Vector3.up * yValue * maxHeight;
                transform.position = dir;
                //시간에 해당하는 값 받아오기 Evaluate(time)
                //dir.Set(m_posXCurve.Evaluate(time), m_posYCurve.Evaluate(time), 0f);
                if (time > 1f)
                {
                    ItemManager.Instance.RemoveItem(this);
                    yield break;
                }
            }
            yield return null;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ItemManager.Instance.RemoveItem(this);
            switch(m_type)
            {
                case ItemManager.ItemType.Coin:
                    GameUIManager.Instance.IncreaseCoin(1);
                    SoundManager.Instance.PlaySfx(SoundManager.SfxClip.Get_coin);
                    break;
                case ItemManager.ItemType.Gem_Red:
                case ItemManager.ItemType.Gem_Green:
                case ItemManager.ItemType.Gem_Blue:
                    GameUIManager.Instance.IncreaseCoin(((uint)m_type) * 10);
                    SoundManager.Instance.PlaySfx(SoundManager.SfxClip.Get_gem);
                    break;
                case ItemManager.ItemType.Invincible:
                    SoundManager.Instance.PlaySfx(SoundManager.SfxClip.Get_Invincible);
                    var buffCtr = collision.GetComponent<BuffController>();
                    buffCtr.SetBuff(BuffController.BuffType.Invincible);
                    break;
                case ItemManager.ItemType.Magnet:
                    SoundManager.Instance.PlaySfx(SoundManager.SfxClip.Get_Item);
                    buffCtr = collision.GetComponent<BuffController>();
                    buffCtr.SetBuff(BuffController.BuffType.Magnet);
                    break;

            }
        }
        else if(collision.CompareTag("Magnet"))
        {
            m_isMagnet = true;
            m_target = collision.transform;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnet"))
        {
            m_isMagnet = false;
        }
    }
    void Update()
    {
        if(m_isMagnet)
        {
            transform.position += (m_target.position - transform.position).normalized * m_speed * 3f * Time.deltaTime;
        }
    }
}
