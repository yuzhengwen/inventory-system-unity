using InventorySystem;
using UnityEngine;

public class CollectibleItem : MonoBehaviour, ICollectible
{
    public ItemDataSO itemData;
    public virtual void Collect(Collector collector)
    {
        AddToInventory(collector);
        Destroy(gameObject);
    }
    protected virtual void AddToInventory(Collector collector)
    {
        collector.Inventory.AddItem(itemData, 1);
        collector.OnItemCollected?.Invoke(itemData);
    }
}
