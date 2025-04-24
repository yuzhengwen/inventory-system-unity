using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventorySystem
{
    [RequireComponent(typeof(Collider2D))]
    public class Collector : MonoBehaviour
    {
        [SerializeField] private InventoryController inventoryController; 
        private void OnTriggerEnter2D(Collider2D collision)
        {
            ICollectible collectible = collision.GetComponent<ICollectible>();
            collectible?.Collect(this);
        }
        public void AddItemsToInventory(ItemDataSO itemData, int amt = 1)
        {
            inventoryController.AddItemsToInventory(itemData, amt);
        }
    }
}
