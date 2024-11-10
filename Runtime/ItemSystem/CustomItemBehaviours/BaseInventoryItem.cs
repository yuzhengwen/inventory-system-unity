using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventoryItem
{
    protected GameObject owner;
    protected BaseInventoryItem(GameObject owner)
    {
        this.owner = owner;
    }
}
public interface IUseable
{
    void Use(InventorySlot slot);
}
public interface IEquippable
{
    void Equip(InventorySlot slot);
    void Unequip(InventorySlot slot);
}
public interface  IDroppable
{
    void Drop();
}
