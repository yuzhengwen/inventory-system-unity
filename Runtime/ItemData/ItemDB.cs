using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : ScriptableObject
{
    public static ItemDataSO[] allItems;

    public const int COIN = 0, CRYSTAL = 1;

    public static IUseable GetUseable(int id, Collector c)
    {
        switch (id)
        {
            case CRYSTAL:
                return new CrystalItem(c);
            default:
                return null;
        }
    }
}

