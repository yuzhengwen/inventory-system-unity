using InventorySystem;
using UnityEngine;

public class CrystalItem : BaseInventoryItem, IUseable
{
    public CrystalItem(GameObject owner) : base(owner)
    {
        Debug.Log("Crystal created");
    }

    public void Use(InventorySlot slot)
    {
        owner.GetComponent<Inventory>().RemoveFromSlot(slot, 1);
        Debug.Log("Crystal used");
    }
}

public class NoneItem : BaseInventoryItem
{
    public NoneItem(GameObject owner) : base(owner)
    {
    }
}