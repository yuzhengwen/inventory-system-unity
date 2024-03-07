using UnityEngine;

public class CollectibleItem : MonoBehaviour, ICollectible
{
    public ItemDataSO itemData;
    public void Collect(Collector collector)
    {
        collector.AddCollectibleToInventory(itemData);
        Destroy(gameObject);
    }
}
