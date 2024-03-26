using InventorySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuzuValen
{
    public class HotbarSelector : MonoBehaviour
    {
        [SerializeField] private UI_Inventory uiInventory;
        private int currentSlotIndex = 0;
        private int maxSlotIndex;
        private UI_InventorySlot currentSlot;
        Vector3 targetPos;
        private void Awake()
        {
            targetPos = transform.localPosition;
        }

        private void Update()
        {
            maxSlotIndex = uiInventory.inventorySlots.Count - 1;
            if (Input.GetKeyDown(KeyCode.K))
            {
                NextItem();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                PreviousItem();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                GetSelectedSlot().GetItem().UseItem();
            }
        }
        public UI_InventorySlot GetSelectedSlot()
        {
            return currentSlot;
        }
        public void PreviousItem()
        {
            if (currentSlotIndex == 0)
            {
                ChangeIndex(maxSlotIndex);
                return;
            }
            ChangeIndex(currentSlotIndex - 1);
        }

        public void NextItem()
        {
            if (currentSlotIndex == maxSlotIndex)
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
            transform.GetComponent<RectTransform>().anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
