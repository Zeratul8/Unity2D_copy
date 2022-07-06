using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    bool m_isInit;
    void OnDisable()
    {
        if(m_isInit)
        {
            m_isInit = false;
            return; 
        }
        EffectManager.Instance.RemoveEffect(this);
    }
    void Awake()
    {
        m_isInit = true;
    }


    void Start()
    {
        var effectDestroy = gameObject.GetComponent<EffectAutoDestroy>();
        if(effectDestroy == null)
        {
            effectDestroy = gameObject.AddComponent<EffectAutoDestroy>();
        }
    }


    void Update()
    {
        
    }
}
