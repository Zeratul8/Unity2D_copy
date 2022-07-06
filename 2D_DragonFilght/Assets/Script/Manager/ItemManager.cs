using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
    public enum ItemType
    {
        None = -1,
        Coin,
        Gem_Red,
        Gem_Green,
        Gem_Blue,
        Invincible,
        Magnet,
        Max
    }
    [SerializeField]
    GameObject m_itemPrefab;
    GameObjectPool<ItemController> m_itemPool;
    [SerializeField]
    HeroController m_hero;
    [SerializeField]
    Sprite[] m_icons;
    [SerializeField]
    int[] m_itemTable = new int[6] { 90, 3, 2, 1, 1, 3 };

    public void CreateItem(Vector3 pos)
    {
        ItemType type;
        var item = m_itemPool.Get();
        //무적상태일때 무적아이템 안뜨게하고, GetPriority로 확률 적용하기
        do
        {
            type = (ItemType)Util.GetPriority(m_itemTable);
        } while (type == ItemType.Invincible && m_hero.curBuffSet == 2);
        //ItemType type = (ItemType)Random.Range((int)ItemType.Coin, (int)ItemType.Max);

        item.SetItem(type, pos, m_hero.transform.position, m_icons[(int)type]);
    }

    public void RemoveItem(ItemController item)
    {
        item.gameObject.SetActive(false);
    }


    protected override void OnStart()
    {
        m_icons = Resources.LoadAll<Sprite>("Images/Items");
        m_itemPool = new GameObjectPool<ItemController>(5, () =>
        {
            var obj = Instantiate(m_itemPrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            var item = obj.GetComponent<ItemController>();
            return item;
        });
    }
}
