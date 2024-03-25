﻿using InventorySystem;
using UnityEngine;

public class CollectibleObject : MonoBehaviour, ICollectible
{
    [SerializeField] private ItemDataSO itemData;
    [SerializeField] private int amount = 1;
    public virtual void Collect(Collector collector)
    {
        collector.AddItemsToInventory(itemData, amount);
        Destroy(gameObject);
    }
}
