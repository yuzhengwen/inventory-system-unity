using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace InventorySystem
{
    public class UI_Inventory : MonoBehaviour
    {
        [SerializeField] private GameObject inventorySlotPrefab;

        [Header("Inventory Slice Settings")] [SerializeField]
        private bool useSubset = false;

        [SerializeField] private int fromIndex;
        [SerializeField] private bool toEnd;
        [SerializeField] private int toIndex;

        // ordered list of slots
        public readonly List<UI_InventorySlot> inventorySlots = new();

        public event Action OnInventoryAssigned;

        /// <summary>
        /// Assigns the inventory to be displayed
        /// </summary>
        /// <param name="inventory"></param>
        public void AssignInventory(Inventory inventory)
        {
            if (toEnd)
                toIndex = inventory.GetItems().Count;
            AddSlots(useSubset
                ? inventory.GetItems().Skip(fromIndex).Take(toIndex - fromIndex).ToList()
                : inventory.GetItems());
            inventory.OnSlotCountChanged += UpdateSlots;
            OnInventoryAssigned?.Invoke();
        }

        private void AddSlots(List<InventorySlot> items)
        {
            foreach (var item in items)
            {
                GameObject slot = Instantiate(inventorySlotPrefab, transform);
                slot.SetActive(true);
                slot.transform.SetParent(transform, false);
                UI_InventorySlot uiSlot = slot.GetComponent<UI_InventorySlot>();
                inventorySlots.Add(uiSlot);
                uiSlot.AssignSlot(item);
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

            AddSlots(useSubset ? slots.Skip(fromIndex).Take(toIndex - fromIndex).ToList() : slots);

            // refresh ui: Why the fk is this necessary???
            foreach (var slot in inventorySlots)
            {
                slot.gameObject.SetActive(false);
                slot.gameObject.SetActive(true);
            }
        }
    }
}