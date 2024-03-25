using System;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class InventorySlot
    {
        public ItemDataSO itemData; // stores static shared data
        public int stackSize;
        public BaseInventoryItem item; // stores custom behaviours, can be null if no custom behaviour

        #region Events
        public event Action<int, int> OnStackChanged;
        public event Action<InventorySlot> OnItemChanged;
        public event Action OnSlotCleared;
        #endregion

        public void ClearSlot()
        {
            itemData = null;
            stackSize = 0;
            item = null;
            OnSlotCleared?.Invoke();
        }
        /// <summary>
        /// Wrapper method for SetItem(ItemDataSO, int, BaseInventoryItem)
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public InventorySlot SetItem(InventorySlot slot)
        {
            if (slot == null)
            {
                ClearSlot(); return this;
            }
            return SetItem(slot.itemData, slot.stackSize, slot.item);
        }
        public InventorySlot SetItem(ItemDataSO itemData, int stackSize, BaseInventoryItem itemObj = null)
        {
            if (itemData == null || stackSize <= 0)
                ClearSlot();
            this.itemData = itemData;
            this.stackSize = stackSize;
            this.item = itemObj;
            OnItemChanged?.Invoke(this);
            return this;
        }
        /// <summary>
        /// Increases stack size
        /// </summary>
        /// <param name="amount">Amount to add to stack</param>
        /// <returns>Amount of items unable to be added (overflow)</returns>
        public int AddToStack(int amount)
        {
            if (IsMaxStack())
            {
                return amount;
            }
            else
            {
                stackSize += amount;
                int overflow = 0;
                if (stackSize > itemData.maxStackSize)
                {
                    overflow = stackSize - itemData.maxStackSize;
                    stackSize = itemData.maxStackSize;
                }
                OnStackChanged?.Invoke(stackSize, amount);
                return overflow;
            }
        }
        /// <summary>
        /// Decreases the stack size 
        /// </summary>
        /// <param name="amount">Amount to remove from stack</param>
        /// <returns>Amount of items that still need to be removed</returns>
        public int RemoveFromStack(int amount)
        {
            stackSize -= amount;
            int excess = 0;
            if (stackSize <= 0)
            {
                excess = -stackSize;
                ClearSlot();
            }
            OnStackChanged?.Invoke(stackSize, -amount);
            return excess;
        }
        /// <summary>
        /// Creates a full copy of the slot (not a reference)
        /// </summary>
        /// <returns></returns>
        public InventorySlot Copy()
        {
            return new InventorySlot { itemData = itemData, stackSize = stackSize };
        }
        /// <summary>
        /// Swaps the data between the two slots
        /// If slot passed is null, the slot is cleared
        /// </summary>
        public void Swap(InventorySlot slot)
        {
            // due to pass by reference, we need to create a full copy of the slot being swapped to
            // If we use temp = slot, then changes to slot will also change temp
            InventorySlot temp = slot.Copy();
            slot.SetItem(this);
            SetItem(temp);
        }

        public void UseItem()
        {
            Debug.Log("Use Item");
            (item as IUseable)?.Use(this);
        }
        public bool IsMaxStack() => itemData ? stackSize == itemData.maxStackSize : false;
        public bool IsOccupied() => itemData != null && stackSize > 0;
        public bool IsSameItem(InventorySlot slot) => itemData == slot.itemData;
    }
}
