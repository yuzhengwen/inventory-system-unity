using InventorySystem;
using System;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Collector : MonoBehaviour
{
    [Header("Inventory Model and List of views connected")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private UI_Inventory[] uiInventories;

    public event Action<ItemDataSO> OnItemCollected;
    private void Start()
    {
        foreach (UI_Inventory uiInventory in uiInventories)
            uiInventory.AssignInventory(inventory);
        uiInventories[1].gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectible collectible = collision.GetComponent<ICollectible>();
        collectible?.Collect(this);
    }
    public void AddItemsToInventory(ItemDataSO itemData, int amt = 1)
    {
        inventory.AddItem(itemData, amt);
        OnItemCollected?.Invoke(itemData);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleFullInventory();
        }
    }

    private void ToggleFullInventory()
    {
        foreach (UI_Inventory uiInventory in uiInventories)
        {
            uiInventory.gameObject.SetActive(!uiInventory.gameObject.activeSelf);
        }
    }
}
