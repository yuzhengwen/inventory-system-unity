using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class UI_Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject inventorySlotPrefab;

        [Header("Hotbar Settings")]
        [SerializeField] private bool hotbar = false;
        [SerializeField] private int noOfHotbarSlots = 10;

        // ordered list of slots
        public readonly List<UI_InventorySlot> inventorySlots = new();

        /// <summary>
        /// Assigns the inventory to be displayed
        /// </summary>
        /// <param name="inventory"></param>
        public void AssignInventory(Inventory inventory)
        {
            InventorySlot[] items = inventory.GetItems().ToArray();
            if (hotbar)
                items = items.Take(noOfHotbarSlots).ToArray();
            AddSlots(items);
        }
        private void AddSlots(InventorySlot[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                GameObject slot = Instantiate(inventorySlotPrefab, transform);
                slot.SetActive(true);
                slot.transform.SetParent(transform, false);
                UI_InventorySlot uiSlot = slot.GetComponent<UI_InventorySlot>();
                inventorySlots.Add(uiSlot);
                uiSlot.AssignSlot(items[i]);
            }
        }
    }
}