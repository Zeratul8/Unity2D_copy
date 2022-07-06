using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//유니티 엔진에서 적용하는 제이슨
using UnityEngine.Serialization;
using JsonFx.Json;

public class PlayerDataManager : DontDestroy<PlayerDataManager>
{
    PlayerData? m_myData;

    public uint IncreaseGold(uint gold)
    {
        PlayerData data = m_myData.Value;
        data.goldOwned += gold;
        return m_myData.Value.goldOwned;
    }
    public bool DecreaseGold(uint gold)
    {
        PlayerData data = m_myData.Value;
        if (data.goldOwned - gold < 0)
            return false;
        data.goldOwned -= gold;
        m_myData = data;
        return true;
    }

    public uint GetBestRecord()
    {
        return m_myData.Value.bestRecord;
    }
    public void SetBestRecord(uint score)
    {
        PlayerData data = m_myData.Value;
        data.bestRecord = score;
        m_myData = data;
    }
    public int GetHeroIndex()
    {
        return m_myData.Value.heroIndex;
    }
    public void SetHeroIndex(int index)
    {
        PlayerData data = m_myData.Value;
        data.heroIndex = (byte)index;
    }
    public void Save()
    {
        //PlayerPrefs.SetInt("GoldOwned", (int)m_myData.Value.goldOwned);
        //var jsonData = JsonWriter.Serialize(m_myData);
        //유니티 제이슨
        var jsonData = JsonUtility.ToJson(m_myData);
        PlayerPrefs.SetString("PlayerData", jsonData);
        PlayerPrefs.Save();
    }
    public PlayerData? Load()
    {
        var jsonData = PlayerPrefs.GetString("PlayerData", null);
        if(!string.IsNullOrEmpty(jsonData))
            //return m_myData = JsonReader.Deserialize<PlayerData?>(jsonData);
            //유니티 제이슨
            return m_myData = JsonUtility.FromJson<PlayerData?>(jsonData);
        return null;
        
    }

    protected override void OnStart()
    {
        //테스트용 저장된거 지우고 다시할때
        //PlayerPrefs.DeleteAll();
        m_myData = Load();
        if(m_myData == null)
        {
            m_myData = new PlayerData(TableHeroData.Instance.HeroesData);
            Save();
        }
    }
}
