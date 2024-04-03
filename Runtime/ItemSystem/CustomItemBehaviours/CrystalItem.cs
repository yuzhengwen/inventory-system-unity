using InventorySystem;
using UnityEngine;

public class CrystalItem : BaseInventoryItem, IUseable
{
    private GameObject owner;
    public CrystalItem(GameObject owner)
    {
        this.owner = owner;
    }
    public void Use(InventorySlot slot)
    {
        owner.GetComponent<Inventory>().RemoveFromSlot(slot, 1);
        Debug.Log("Crystal used");
    }
}
