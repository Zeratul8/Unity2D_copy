using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoDestroy : MonoBehaviour
{
    [SerializeField]
    float m_lifeTime = 0;
    float m_activeTime;

    ParticleSystem[] m_particles;


    // Start is called before the first frame update
    void Start()
    {
        m_particles = GetComponentsInChildren<ParticleSystem>();
    }

    void OnEnable()
    {
        m_activeTime = Time.time;
    }


    void Update()
    {
        if (m_lifeTime > 0)
        {
            if (Time.time > m_activeTime + m_lifeTime)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            bool isPlaying = false;
            for (int i = 0; i < m_particles.Length; i++)
            {
                if (m_particles[i].isPlaying)
                {
                    isPlaying = true;
                    break;
                }
            }
            if (!isPlaying)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
