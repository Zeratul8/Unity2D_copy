using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_OkCancel : MonoBehaviour
{
    [SerializeField]
    UILabel m_subjectLabel;
    [SerializeField]
    UILabel m_bodyLabel;
    [SerializeField]
    UILabel m_okBtnLabel;
    [SerializeField]
    UILabel m_cancelBtnLabel;
    ButtonDelegate m_okBtndelegate;
    ButtonDelegate m_cancelBtndelegate;
    public void SetPopup(string subject, string body, ButtonDelegate okDel = null, ButtonDelegate cancelDel = null, string okBtnText = "Ok", string cancelBtnText = "Cancel")
    {
        m_subjectLabel.text = subject;
        m_bodyLabel.text = body;
        m_okBtnLabel.text = okBtnText;
        m_cancelBtnLabel.text = cancelBtnText;
        m_okBtndelegate = okDel;
        m_cancelBtndelegate = cancelDel;
    }
    public void OnPressOk()
    {
        if (m_okBtndelegate != null)
            m_okBtndelegate();
        else
            PopupManager.Instance.ClosePopup();
    }
    public void OnPressCancel()
    {
        if (m_cancelBtndelegate != null)
            m_cancelBtndelegate();
        else
            PopupManager.Instance.ClosePopup();
    }
    // Start is called before the first frame update
    void Start()
    {      
    }
}
