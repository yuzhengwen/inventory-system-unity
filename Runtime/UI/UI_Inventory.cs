using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class UI_Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject inventorySlotPrefab;

        [Header("Hotbar Settings")] [SerializeField]
        private bool hotbar = false;

        [SerializeField] private int noOfHotbarSlots = 10;

        // ordered list of slots
        public readonly List<UI_InventorySlot> inventorySlots = new();

        public event Action OnInventoryAssigned;

        /// <summary>
        /// Assigns the inventory to be displayed
        /// </summary>
        /// <param name="inventory"></param>
        public void AssignInventory(Inventory inventory)
        {
            AddSlots(hotbar ? inventory.GetItems().Take(noOfHotbarSlots).ToList() : inventory.GetItems());
            inventory.OnSlotCountChanged += UpdateSlots;
            OnInventoryAssigned?.Invoke();
        }

        private void AddSlots(List<InventorySlot> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                GameObject slot = Instantiate(inventorySlotPrefab, transform);
                slot.SetActive(true);
                slot.transform.SetParent(transform, false);
                UI_InventorySlot uiSlot = slot.GetComponent<UI_InventorySlot>();
                inventorySlots.Add(uiSlot);
                uiSlot.AssignSlot(items[i]);
            }
        }

        // this runs even when ui is not active
        private void UpdateSlots(List<InventorySlot> slots)
        {
            // deleted all the active ui slots
            foreach (var slot in inventorySlots)
            {
                if (slot != null)
                    Destroy(slot.gameObject);
            }

            inventorySlots.Clear();

            AddSlots(hotbar ? slots.Take(noOfHotbarSlots).ToList() : slots);
            
            // refresh ui: Why the fk is this necessary???
            foreach (var slot in inventorySlots)
            {
                slot.gameObject.SetActive(false);
                slot.gameObject.SetActive(true);
            }
        }
    }
}