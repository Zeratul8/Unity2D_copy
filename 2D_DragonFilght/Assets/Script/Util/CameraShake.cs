using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    float m_power = 0.5f;
    [SerializeField]
    float m_duration = 1f;
    Vector3 m_orgPos;

    public void Shake(float power, float duration)
    {
        m_orgPos = transform.position;
        m_power = power;
        m_duration = duration;
        StopAllCoroutines();
        StartCoroutine(Coroutine_Shake());
    }
    public void Shake()
    {
        Shake(m_power, m_duration);
    }
    IEnumerator Coroutine_Shake()
    {
        float time = 0f;
        while(true)
        {
            yield return null;
            time += Time.deltaTime;
            var dir = Random.insideUnitCircle;
            transform.localPosition = new Vector3(dir.x, dir.y, m_orgPos.z) * m_power;
            if (time > m_duration)
            {
                transform.localPosition = new Vector3(0f, 0f, -10f);
                yield break;
            }
        }
    }


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
