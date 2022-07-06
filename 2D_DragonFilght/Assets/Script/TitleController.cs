using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    bool m_isToggleOn;
    bool m_isOpen;
    string m_id = "아이디를 입력하세요";
    string m_pass = string.Empty;
    int m_select;
    int m_height = 40;
    string[] m_weaponList = new string[] { "단검", "양손검", "양손도끼", "롱보우", "크로스보우", "저격총", "돌격소총" };    

    public void GoNextScene()
    {
        LoadSceneManager.Instance.LoadSceneAsync(LoadSceneManager.SceneState.Game);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect((Screen.width - 150) * 0.5f, (Screen.height - 50) * 0.5f, 200, 100), "START"))
        {
            GoNextScene();
        }
    }
    /*
    void OnGUI()
    {    
        if(GUI.Button(new Rect((Screen.width - 200) * 0.5f, (Screen.height - 100) * 0.5f, 200, 100), "START"))
        {
            Debug.Log("Start game");
        }
        GUILayout.BeginArea(new Rect(10, Screen.height - 400, 300, 400), "디버그 옵션", GUI.skin.window);
        GUILayout.Button("Start");
        m_isToggleOn = GUILayout.Toggle(m_isToggleOn, "무적모드");
        if(m_isToggleOn)
        {
            GUILayout.TextArea("무적모드 활성화");
        }
        m_id = GUILayout.TextField(m_id);
        m_pass = GUILayout.PasswordField(m_pass, '*', 12);
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(Screen.width - 10 - 300, Screen.height - m_height, 300, 400), GUI.skin.window);
        m_isOpen = GUILayout.Toggle(m_isOpen, "무기선택", GUI.skin.button);
        if (m_isOpen)
        {
            m_height = 400;
            m_select = GUILayout.SelectionGrid(m_select, m_weaponList, 1);
        }
        else
        {
            m_height = 40;
        }
        GUILayout.EndArea();
    }*/
}
