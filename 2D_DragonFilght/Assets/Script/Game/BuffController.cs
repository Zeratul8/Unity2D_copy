using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    #region Constants and Fields
    [SerializeField]
    HeroController m_hero;
    [SerializeField]
    CameraShake m_camShake;
    [SerializeField]
    TextMesh m_text;



    public enum BuffType
    {
        Invincible,
        Magnet,
        Max
    }
    public struct BuffData
    {
        public BuffType type;
        public float duration;
    }
    public class BuffInfo
    {
        public float time;
        public BuffData data;
    }


    Dictionary<BuffType, BuffData> m_buffTable = new Dictionary<BuffType, BuffData>()
    {
        {BuffType.Invincible, new BuffData(){type = BuffType.Invincible, duration = 3f} },
        {BuffType.Magnet, new BuffData(){type = BuffType.Magnet, duration = 10f} },
    };
    List<BuffInfo> m_buffList = new List<BuffInfo>();
    #endregion
    #region Properties

    #endregion
    #region Public Methods
    public void SetBuff(BuffType type)
    {
        var curBuff = m_buffList.Find(buff => buff.data.type == type);
        if(curBuff == null)
        {
            BuffInfo buffInfo = new BuffInfo() { time = 0f, data = m_buffTable[type] };
            m_buffList.Add(buffInfo);
            StartCoroutine(Coroutine_BuffProcess(buffInfo));
            switch(type)
            {
                case BuffType.Invincible:
                    GameStateManager.Instance.SetState(GameStateManager.GameState.Invincible);
                    m_camShake.Shake(0.1f, buffInfo.data.duration);
                    m_hero.curBuffSet = 2;
                    break;
                case BuffType.Magnet:
                    m_hero.SetFxMagnet(true);
                    m_hero.curBuffSet = 1;
                    break;
            }
        }
        else
        {
            curBuff.time = 0f;
        }
    }
    #endregion
    #region Coroutine
    IEnumerator Coroutine_BuffProcess(BuffInfo buff)
    {
        while(true)
        {
            yield return null;

            buff.time += Time.deltaTime;
            if(buff.time > buff.data.duration)
            {
                buff.time = 0f;
                m_buffList.Remove(buff);
                switch(buff.data.type)
                {
                    case BuffType.Invincible:
                        MonsterManager.Instance.ResetCreateInterval();
                        m_hero.ResetHero();
                        GameStateManager.Instance.SetState(GameStateManager.GameState.Normal);
                        m_hero.curBuffSet = 0;
                        break;
                    case BuffType.Magnet:
                        m_hero.SetFxMagnet(false);
                        m_hero.curBuffSet = 0;
                        break;
                }
                yield break;
            }
            
        }
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        
    }

}
