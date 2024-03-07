## How to use
### Backend
Add 'Inventory' MonoBehaviour to Game Object. This has the benefit of being able to inspect inventory slots in the editor in runtime.
To use, simply get a reference to the inventory object.

#### Public Methods
- `AddItem()`
- `RemoveItem()`
- `GetItems()`
- `FillSpace()` 
- `ClearInventory()`
- `PrintInventory()`
- `IsEmpty()`
- `IsFull()`
#### Events
- `OnItemAdded`
- `OnItemRemoved`
### UI 
![[Pasted image 20240305015015.png|400]]
After that, call the `AssignInventory()` method to assign an inventory backend to display.
### Adding a Controller
The demo 'Collector' class:
```cs
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
```