using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : ScriptableObject
{
    public static ItemDataSO[] allItems;

    public const int COIN = 0, CRYSTAL = 1;

    public static IUseable GetUseable(int id, GameObject owner)
    {
        switch (id)
        {
            case CRYSTAL:
                return new CrystalItem(owner);
            default:
                return null;
        }
    }
    public static ItemDataSO GetItemData(int id)
    {
        return allItems[id];
    }
}

