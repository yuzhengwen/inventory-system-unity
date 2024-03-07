using InventorySystem;
using System;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Collector : MonoBehaviour
{
    public Inventory Inventory { get; private set; }
    [SerializeField] private UI_Inventory uiInventory;

    public Action<ItemDataSO> OnItemCollected;
    private void Start()
    {
        Inventory = GetComponent<Inventory>();
        uiInventory.AssignInventory(Inventory);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectible collectible = collision.GetComponent<ICollectible>();
        collectible?.Collect(this);
    }
}
