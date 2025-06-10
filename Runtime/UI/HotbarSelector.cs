using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventorySystem
{
    public class HotbarSelector : MonoBehaviour
    {
        [SerializeField] private UI_Inventory uiInventory;
        private int currentSlotIndex = 0;
        private int MaxSlotIndex => uiInventory.inventorySlots.Count - 1;
        private UI_InventorySlot currentSlot;

        [Header("Hotbar Input Bindings")] [SerializeField]
        private InputAction useItem;

        [SerializeField] private InputAction nextItem;
        [SerializeField] private InputAction prevItem;

        private void OnEnable()
        {
            useItem.performed += (ctx) => GetSelectedSlot().GetItem().UseItem();
            nextItem.performed += (ctx) => NextItem();
            prevItem.performed += (ctx) => PreviousItem();
            useItem.Enable();
            nextItem.Enable();
            prevItem.Enable();
        }

        private void OnDisable()
        {
            useItem.Disable();
            nextItem.Disable();
            prevItem.Disable();
        }

        public UI_InventorySlot GetSelectedSlot()
        {
            return currentSlot;
        }

        public void PreviousItem()
        {
            if (currentSlotIndex == 0)
            {
                ChangeIndex(MaxSlotIndex);
                return;
            }

            ChangeIndex(currentSlotIndex - 1);
        }

        public void NextItem()
        {
            if (currentSlotIndex == MaxSlotIndex)
            {
                ChangeIndex(0);
                return;
            }

            ChangeIndex(currentSlotIndex + 1);
        }

        private void ChangeIndex(int index)
        {
            currentSlotIndex = index;
            var slot = uiInventory.inventorySlots[currentSlotIndex];
            currentSlot = slot;
            transform.localPosition = slot.transform.localPosition;
            transform.GetComponent<RectTransform>().anchoredPosition =
                slot.GetComponent<RectTransform>().anchoredPosition;
        }
    }
}