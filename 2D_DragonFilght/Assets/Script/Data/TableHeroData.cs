using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableHeroData : DontDestroy<TableHeroData>
{
    [SerializeField]
    HeroData[] m_HeroesData;

    public HeroData[] HeroesData { get { return m_HeroesData; } }
}
