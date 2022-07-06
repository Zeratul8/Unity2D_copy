using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ButtonDelegate();
public class PopupManager : DontDestroy<PopupManager>
{
    [SerializeField]
    GameObject m_popupOkCancelPrefab;
    [SerializeField]
    GameObject m_popupOkPrefab;
    List<GameObject> m_popupList = new List<GameObject>();
    public bool IsOpen { get { return m_popupList.Count > 0; } }
    int m_popupDepth = 1000;
    int m_popupDepthGap = 10;
    // Start is called before the first frame update
    protected override void OnStart()
    {
        m_popupOkCancelPrefab = Resources.Load<GameObject>("Prefab/Popup/Popup_OkCancel");
        m_popupOkPrefab = Resources.Load<GameObject>("Prefab/Popup/Popup_Ok");
    }
    public void Open_PopupOkCancel(string subject, string body, ButtonDelegate okDel = null, ButtonDelegate cancelDel = null, string okBtnText = "Ok", string cancelBtnText = "Cancel")
    {
        var obj = Instantiate(m_popupOkCancelPrefab);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        var panels = obj.GetComponentsInChildren<UIPanel>();
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].depth = m_popupDepth + m_popupList.Count * m_popupDepthGap + i;
        }
        var popup = obj.GetComponent<Popup_OkCancel>();

        popup.SetPopup(subject, body, okDel, cancelDel, okBtnText, cancelBtnText);

        m_popupList.Add(obj);
    }

    public void Open_PopupOk(string subject, string body, ButtonDelegate okDel, string okBtnText = "Ok")
    {
        var obj = Instantiate(m_popupOkPrefab);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        var panels = obj.GetComponentsInChildren<UIPanel>();
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].depth = m_popupDepth + m_popupList.Count * m_popupDepthGap + i;
        }
        var popup = obj.GetComponent<Popup_Ok>();
        popup.SetPopup(subject, body, okDel, okBtnText);

        m_popupList.Add(obj);
    }
    public void ClosePopup()
    {
        if(m_popupList.Count > 0)
        {
            Destroy(m_popupList[m_popupList.Count - 1]);
            m_popupList.RemoveAt(m_popupList.Count - 1);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Open_PopupOkCancel("공지사항", "지금은 팝업 테스트 중입니다.\r\n수업이 어렵습니까?", null, null, "예", "아니오");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Open_PopupOk("Error", "더 이상 수업을 진행 할 수 없습니다.", null, "확인");
        }
    }
}
