using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Result : MonoBehaviour
{
    [SerializeField]
    UI2DSprite m_sdSprite;
    [SerializeField]
    GameObject m_bestRecordObj;
    [SerializeField]
    UILabel m_totalScoreLabel;
    [SerializeField]
    UILabel m_distScoreLabel;
    [SerializeField]
    UILabel m_huntScoreLabel;
    [SerializeField]
    UILabel m_coinCountLabel;
    [SerializeField]
    UILabel m_bestScoreLabel;

    public void SetResult()
    {
        bool isBest = false;
        GameUIManager.Instance.HideUI();
        m_bestRecordObj.SetActive(false);
        ShowUI();
        uint distScore = GameUIManager.Instance.GetDistScore();
        uint huntScore = GameUIManager.Instance.GetHuntScore();
        uint totalscore = distScore + huntScore;
        uint bestRecord = PlayerDataManager.Instance.GetBestRecord();
        if (totalscore > bestRecord)
        {
            isBest = true;
            m_bestRecordObj.SetActive(true);
            PlayerDataManager.Instance.SetBestRecord(totalscore);
        }
        m_sdSprite.sprite2D = Resources.Load<Sprite>(string.Format("Images/SD/sd_{0:00}{1}", PlayerDataManager.Instance.GetHeroIndex() + 1, isBest == true ? "_highscore" : string.Empty));
        m_totalScoreLabel.text = string.Format("{0:n0}", totalscore);
        m_distScoreLabel.text = string.Format("{0:n0}", distScore);
        m_huntScoreLabel.text = string.Format("{0:n0}", huntScore);
        m_coinCountLabel.text = string.Format("{0:n0}", GameUIManager.Instance.GetCoin());
        m_bestScoreLabel.text = string.Format("{0:n0}", isBest == true ? totalscore : bestRecord);
        PlayerDataManager.Instance.IncreaseGold(GameUIManager.Instance.GetCoin());
        PlayerDataManager.Instance.Save();
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }
    public void HideUI()
    {
        gameObject.SetActive(false);
    }


    void Start()
    {
        
    }


}
