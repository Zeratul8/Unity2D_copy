using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgController : MonoBehaviour
{
    [SerializeField]
    float m_speed = 0.1f;
    float m_scale = 1f;
    SpriteRenderer m_sprRenderer;
    public void SetSpeed(float scale = 1f)
    {
        m_scale = scale;
    }
    void Start()
    {
        m_sprRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        m_sprRenderer.material.mainTextureOffset += Vector2.up * m_speed * m_scale * Time.deltaTime;
        if(GameUIManager.Instance != null)
            GameUIManager.Instance.SetDistanceScore(m_sprRenderer.material.mainTextureOffset.y);
    }
}
