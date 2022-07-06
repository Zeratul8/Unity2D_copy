using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Ok : MonoBehaviour
{
    [SerializeField]
    UILabel m_subjectLabel;
    [SerializeField]
    UILabel m_bodyLabel;
    [SerializeField]
    UILabel m_okBtnLabel;
    ButtonDelegate m_okBtnDelegate;
    public void SetPopup(string subject, string body, ButtonDelegate okDel = null, string okBtnText = "Ok")
    {
        m_subjectLabel.text = subject;
        m_bodyLabel.text = body;
        m_okBtnLabel.text = okBtnText;
        m_okBtnDelegate = okDel;
    }
    public void OnPressOk()
    {
        if (m_okBtnDelegate != null)
            m_okBtnDelegate();
        else
            PopupManager.Instance.ClosePopup();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
   
}
