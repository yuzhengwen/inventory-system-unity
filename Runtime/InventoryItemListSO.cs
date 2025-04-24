using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "StartingInventory", menuName = "ScriptableObjects/StartingInventory", order = 1)]
    public class InventoryItemListSO : ScriptableObject
    {
        public List<InventorySlot> startingItems = new();
    }
}
