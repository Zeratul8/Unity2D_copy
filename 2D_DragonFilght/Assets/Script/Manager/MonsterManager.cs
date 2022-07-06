using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingletonMonoBehaviour<MonsterManager>
{
    public enum MonsterType
    {
        White,
        Yellow,
        Pink,
        Bomb,
        Max
    }
    GameObject[] m_monsterPrefabs;
    Dictionary<MonsterType, GameObjectPool<MonsterController>> m_monsterPool = new Dictionary<MonsterType, GameObjectPool<MonsterController>>();
    List<MonsterController> m_monsterList = new List<MonsterController>();
    

    Vector2 m_startPos = new Vector2(-3f, 4.5f);

    float m_posXGap = 1f;
    int m_curLine;
    float m_createInterval = 3f;
    float m_scale = 1f;

    public float SpeedScale { get { return m_scale; } }
    
    public void ResetCreateInterval(float scale = 1f)
    {
        m_scale = scale;
        
        RemoveMonsterAll();
        InvokeRepeating("CreateMonsters", m_createInterval / m_scale, m_createInterval / m_scale);
    }

    public void RemoveMonsterAll()
    {
        if (IsInvoking("CreateMonsters"))
            CancelInvoke("CreateMonsters");
    }
    public void RemoveMonster(MonsterController mon)
    {
        if(m_monsterList.Remove(mon))
        {
            mon.gameObject.SetActive(false);
            m_monsterPool[mon.Type].Set(mon);
        }
    }
    public void RemoveMonstersLine(int line)
    {
        for(int i =0; i < m_monsterList.Count; i++)
        {
            if(m_monsterList[i].Line == line)
            {
                m_monsterList[i].gameObject.SetActive(false);
                m_monsterList[i].SetDie();
            }
        }
        m_monsterList.RemoveAll(mon => 
        {
            if(mon.gameObject.activeSelf == false)
            {
                m_monsterPool[mon.Type].Set(mon);
                return true;
            }
            return false;
        });
    }

    void CreateMonsters()
    {
        MonsterType type;
        bool isBomb = false;
        bool isTry = false;
        bool isSame = false;
        int moncnt = 0;
        int[] monPos = new int[5];
        moncnt = Random.Range(1, 6);
        m_curLine++;
        for (int i = 0; i < moncnt; i++)
        {
            do
            {
                isSame = false;
                monPos[i] = Random.Range(1, 6);
                for (int j = 0; j < i; j++)
                {
                    if (monPos[i] == monPos[j])
                        isSame = true;
                }
            } while (isSame);
        }
        for (int i =0; i < moncnt; i++)
        {
            do
            {
                isTry = false;
                type = (MonsterType)Random.Range((int)MonsterType.White, (int)MonsterType.Max);
                if(type == MonsterType.Bomb && !isBomb)
                {
                    isBomb = true;
                    isTry = false;
                }
                else if(type == MonsterType.Bomb && isBomb)
                {
                    isTry = true;
                }
            } while (isTry);
            
            var mon = m_monsterPool[type].Get();
            mon.Setmonster();
            mon.gameObject.SetActive(true);
            mon.transform.position = m_startPos + Vector2.right * m_posXGap * monPos[i];
            mon.Line = m_curLine;
            m_monsterList.Add(mon);
        }
    }

    protected override void OnStart()
    {
        m_monsterPrefabs = Resources.LoadAll<GameObject>("Prefab/Monster/");
        for(int i =0; i<m_monsterPrefabs.Length; i++)
        {
            var results = m_monsterPrefabs[i].name.Split('.');
            int type = int.Parse(results[0]) - 1;
            var pool = new GameObjectPool<MonsterController>(3, () =>
            {
                var obj = Instantiate(m_monsterPrefabs[type]);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                var mon = obj.GetComponent<MonsterController>();
                mon.InitMonster((MonsterType)type);
                return mon;
            });
            m_monsterPool.Add((MonsterType)type, pool);
        }
        /*m_monsterPool = new GameObjectPool<MonsterController>(10, () =>
        {
            var obj = Instantiate(m_monsterPrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            var mon = obj.GetComponent<MonsterController>();

            return mon;
        });*/
        //인보크로 (함수, 대기시간, 반복시간)
        InvokeRepeating("CreateMonsters", 3f, m_createInterval * m_scale);
    }

    void Update()
    {
        for(int i = 0; i< m_monsterList.Count; i++)
        {
            m_monsterList[i].Move();
        }
    }
   
}
