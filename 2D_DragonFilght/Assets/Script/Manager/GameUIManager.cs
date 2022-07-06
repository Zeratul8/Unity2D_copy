using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class GameUIManager : SingletonMonoBehaviour<GameUIManager>
{
    #region Constants and Fields
    [SerializeField]
    UILabel m_distScoreLabel;
    [SerializeField]
    UILabel m_huntScoreLabel;
    [SerializeField]
    UILabel m_coinCountLabel;
    StringBuilder m_sb = new StringBuilder();

    uint m_distScore;
    uint m_huntScore;
    uint m_coinCount;
    #endregion

    #region Properties
    #endregion
    #region Public Methods

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }
    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void SetDistanceScore(float dist)
    {
        m_distScore = Convert.ToUInt32(dist * 100f);
        m_sb.AppendFormat("{0:n0}", m_distScore);
        m_distScoreLabel.text = m_sb.ToString();
        m_sb.Clear();
        //아래방식은 매 프레임마다 string객체를 생성하므로 비효율적
        //m_distScoreLabel.text = string.Format("{0:n0}", m_distScore);
    }

    public uint GetDistScore()
    {
        return m_distScore;
    }


    public void SetHuntScore(uint score)
    {
        m_huntScore += score;
        m_sb.AppendFormat("{0:n0}", m_huntScore);
        m_huntScoreLabel.text = m_sb.ToString();
        m_sb.Clear();
    }

    public uint GetHuntScore()
    {
        return m_huntScore;
    }

    public void IncreaseCoin(uint value)
    {
        m_coinCount += value;
        m_sb.AppendFormat("{0:n0}", m_coinCount);
        m_coinCountLabel.text = m_sb.ToString();
        m_sb.Clear();
    }

    public uint GetCoin()
    {
        return m_coinCount;
    }



    public void Pause()
    {
        Time.timeScale = 0f;
        PopupManager.Instance.Open_PopupOk("일시정지", "게임으로 돌아갑니다.", () => { Time.timeScale = 1f; PopupManager.Instance.ClosePopup(); }, "확인");
    }


    #endregion

    protected override void OnAwake()
    {
        m_distScore = 0;
        SetDistanceScore(m_distScore);
        m_huntScore = 0;
        SetHuntScore(m_huntScore);
        m_coinCount = 0;
        IncreaseCoin(m_coinCount);
    }


    void Update()
    {
        
    }
}
