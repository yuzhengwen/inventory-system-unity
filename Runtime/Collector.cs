using InventorySystem;
using System;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Collector : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;

    public event Action<ItemDataSO> OnItemCollected;
    private void Start()
    {
        inventory = GetComponent<Inventory>();
        uiInventory.AssignInventory(inventory);

    }
    public void AddCollectibleToInventory(ItemDataSO data)
    {
        inventory.AddItem(data, 1);
        OnItemCollected?.Invoke(data);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectible collectible = collision.GetComponent<ICollectible>();
        collectible?.Collect(this);
    }
}
