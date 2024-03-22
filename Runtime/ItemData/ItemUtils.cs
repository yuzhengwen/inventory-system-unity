using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemUtils : MonoBehaviour
{
    public ItemDB ItemDB;
    public static ItemUtils Instance { get; private set; }

    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public BaseInventoryItem GetItem(int id, GameObject owner)
    {
        if (id == ItemDB.CRYSTAL) return new CrystalItem(owner);
        else return null;
    }
    public ItemDataSO GetItemDataById(int id)
    {
        return ItemDB.items.FirstOrDefault(x => x.id == id);
    }
    public ItemDataSO GetItemDataByName(string displayName)
    {
        return ItemDB.items.FirstOrDefault(x => x.displayName == displayName);
    }
}
