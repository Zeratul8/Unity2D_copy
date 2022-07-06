using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
public class LoadSceneManager : DontDestroy<LoadSceneManager>
{
    public enum SceneState
    {
        None = -1,
        Title,
        Lobby,
        Game,
        Max
    }
    [SerializeField]
    GameObject m_loadObj;
    [SerializeField]
    UIProgressBar m_progressBar;
    [SerializeField]
    UILabel m_progressLabel;
    AsyncOperation m_loadingInfo;

    SceneState m_state = SceneState.Title;
    SceneState m_loadState = SceneState.None;
    public void LoadSceneAsync(SceneState scene)
    {
        if (m_loadState != SceneState.None) return;
        m_loadState = scene;
        m_loadingInfo = SceneManager.LoadSceneAsync((int)scene);
        m_loadObj.SetActive(true);
    }
    void HideUI()
    {
        m_loadObj.SetActive(false);
    }
    // Start is called before the first frame update
    protected override void OnStart()
    {
        m_loadObj.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (PopupManager.Instance.IsOpen)
                PopupManager.Instance.ClosePopup();
            else
            {
                switch(m_state)
                {
                    case SceneState.Title:
                        PopupManager.Instance.Open_PopupOkCancel("Notice", "정말로 게임을 종료하시겠습니까?", ()=> {
#if UNITY_EDITOR
                            EditorApplication.isPlaying = false;

#else
                            Application.Quit();
#endif
                        }, null, "예", "아니오");
                        break;
                    case SceneState.Lobby:
                        PopupManager.Instance.Open_PopupOkCancel("Notice", "타이틀 화면으로 돌아가시겠습니까?", ()=> {
                            LoadSceneAsync(SceneState.Title);
                            PopupManager.Instance.ClosePopup();
                        }, null, "예", "아니오");
                        break;
                    case SceneState.Game:
                        PopupManager.Instance.Open_PopupOkCancel("Notice", "게임을 종료하고 로비화면으로 돌아가시겠습니까?", ()=> {
                             LoadSceneAsync(SceneState.Lobby);
                             PopupManager.Instance.ClosePopup();                         
                        }, null, "예", "아니오");
                        break;
                }
            }
        }
        if (m_loadingInfo != null && m_loadState != SceneState.None)
        {
            if (m_loadingInfo.isDone)
            {
                m_loadingInfo = null;
                m_state = m_loadState;
                m_loadState = SceneState.None;
                m_progressBar.value = 1f;
                m_progressLabel.text = "100%";
                Invoke("HideUI", 1f);
            }
            else
            {
                m_progressBar.value = m_loadingInfo.progress;
                m_progressLabel.text = Mathf.CeilToInt(m_loadingInfo.progress * 100f) + "%";
            }
        }

    }
}
