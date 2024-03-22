using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDatabase", menuName = "Item System/New Item Database", order = 2)]
public class ItemDB : ScriptableObject
{
    public ItemDataSO[] items;
    public int COIN = 0, CRYSTAL = 1;
}
