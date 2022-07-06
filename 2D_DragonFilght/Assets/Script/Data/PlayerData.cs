using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HeroData
{
    public uint price;
    public string name;
    public string className;
}

[System.Serializable]
public struct HeroDataInfo
{
    public HeroData data;
    public bool isPlayable;
    public int lv;
}




public struct PlayerData
{
    public readonly static uint BasicGold = 1000;
    public readonly static uint BasicGem = 100;
    public uint bestRecord;
    public uint goldOwned;
    public uint gemOwend;
    public byte heroIndex;
    public List<HeroDataInfo> heroesList;

    public PlayerData(HeroData[] heroesData)
    {
        heroesData = TableHeroData.Instance.HeroesData;
        goldOwned = BasicGold;
        gemOwend = BasicGem;
        heroIndex = 0;
        bestRecord = 0;
        heroesList = new List<HeroDataInfo>();
        for (int i = 0; i < heroesData.Length; i++)
        {
            HeroDataInfo herodataInfo = new HeroDataInfo() { lv = 1, isPlayable = i == 0 ? true : false, data = heroesData[i] };
            heroesList.Add(herodataInfo);
        }
        
    }
}
