using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "NewItemDatabase", menuName = "Item System/New Item Database", order = 2)]
    public class ItemDB : ScriptableObject
    {
        public ItemDataSO[] items;
        [Header("Item IDs (must be same as ItemDataSO.id)")]
        public int COIN = 0;
        public int CRYSTAL = 1;
    }
}
