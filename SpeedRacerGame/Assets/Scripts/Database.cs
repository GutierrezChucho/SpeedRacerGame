using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public ItemDatabase items;

    #region Singleton Pattern
    private static Database _instance;
    public static Database Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public static Item GetItemByID(int ID)
    {
        foreach (Item item in _instance.items.itemList)
        {
            if (item.itemID == ID)
                return item;
        }

        return null;
    }
}
