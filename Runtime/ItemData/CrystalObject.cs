using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalObject : CollectibleItem
{
    private IUseable crystalItem;
    public override void Collect(Collector collector = null)
    {
        AddToInventory(collector);
        Destroy(gameObject);
    }
}
public class CrystalItem : IUseable
{
    private Collector c;
    public CrystalItem(Collector c)
    {
        this.c = c;
    }
    public void Use(InventorySlot slot)
    {
        slot.RemoveFromStack(1);
        Debug.Log("Crystal used");
    }
}
public interface IUseable
{
    void Use(InventorySlot slot);
}
