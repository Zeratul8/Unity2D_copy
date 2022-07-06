using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    [SerializeField]
    GameObject m_fxexplosionPrefab;
    GameObjectPool<VFXController> m_effectPool;

    //풀에서 사용
    public void CreateEffect(Vector3 pos)
    {
        var effect = m_effectPool.Get();
        effect.gameObject.SetActive(true);
        effect.transform.position = pos;
    }

    //다시 풀로 환원
    public void RemoveEffect(VFXController effect)
    {
        m_effectPool.Set(effect);
    }
    protected override void OnStart()
    {
        m_effectPool = new GameObjectPool<VFXController>(5, () =>
        {
            var obj = Instantiate(m_fxexplosionPrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            var vfx = obj.GetComponent<VFXController>();
            return vfx;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
