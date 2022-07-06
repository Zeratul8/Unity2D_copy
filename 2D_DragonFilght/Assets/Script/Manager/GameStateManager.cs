using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{
    [SerializeField]
    HeroController m_hero;
    [SerializeField]
    BgController m_bgCtr;
    [SerializeField]
    UI_Result m_result;

    float m_mul = 6f;

    public enum GameState
    {
        Normal,
        Invincible,
        Result,
        Max
    }
    GameState m_state;

    public void State()
    {
        SceneManager.LoadScene("Title");
    }
    public void SetState(GameState state)
    {
        if (m_state == state)
            return;
        m_state = state;
        switch(m_state)
        {
            case GameState.Normal:
                m_hero.ResetHero();
                m_bgCtr.SetSpeed(1f);
                break;
            case GameState.Invincible:
                m_hero.SetFxInvincible();
                m_bgCtr.SetSpeed(m_mul);
                MonsterManager.Instance.ResetCreateInterval(m_mul);
                break;
            case GameState.Result:
                m_hero.Setdie();
                MonsterManager.Instance.RemoveMonsterAll();
                m_result.SetResult();
                break;
        }
    }
    protected override void OnStart()
    {
        
    }

   
}
