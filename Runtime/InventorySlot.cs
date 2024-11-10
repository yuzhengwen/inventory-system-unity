using System;
using System.Reflection;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class InventorySlot
    {
        [NonSerialized] public Inventory parentInventory;
        public ItemDataSO itemData; // stores static shared data
        public int stackSize;
        public BaseInventoryItem item; // stores custom behaviours, can be null if no custom behaviour

        #region Events

        /// <summary>
        /// Triggered when only stack size changes (but not the item)
        /// Passes the new stack size & change amount
        /// </summary>
        public event Action<int, int> OnStackChanged;

        /// <summary>
        /// Triggered when item is changed in the slot
        /// Passes the slot after the change
        /// </summary>
        public event Action<InventorySlot> OnItemChanged;

        /// <summary>
        /// Triggered when slot is cleared
        /// </summary>
        public event Action OnSlotCleared;

        public event Action<InventorySlot> OnItemUsed;

        #endregion

        public InventorySlot(Inventory parentInventory)
        {
            this.parentInventory = parentInventory;
        }

        public void ClearSlot()
        {
            OnSlotCleared?.Invoke();
            itemData = null;
            stackSize = 0;
            item = null;
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
                ClearSlot();
                return this;
            }

            return SetItem(slot.itemData, slot.stackSize, slot.item);
        }

        public InventorySlot SetItem(ItemDataSO itemData, int stackSize, BaseInventoryItem itemObj = null)
        {
            // allows auto-clearing of slot if itemData is null or stackSize is 0 instead of throwing an error
            if (itemData == null || stackSize <= 0)
            {
                ClearSlot();
                OnItemChanged?.Invoke(this);
                return this;
            }

            if (stackSize > itemData.maxStackSize)
            {
                Debug.LogWarning("Setting slot with stack size greater than max stack size");
                stackSize = itemData.maxStackSize;
            }

            this.itemData = itemData;
            this.stackSize = stackSize;
            // if custom behaviour already exists, use that, otherwise create a new instance
            if (itemObj == null)
                this.item = (BaseInventoryItem)Activator.CreateInstance(
                    itemData.customItemBehaviour.Type, new object[] { parentInventory.gameObject }
                );
            else
                this.item = itemObj;

            OnItemChanged?.Invoke(this);
            return this;
        }

        /// <summary>
        /// Increases stack size
        /// NOTE: Please call this through Inventory class to ensure events are triggered
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
        /// NOTE: Please call this through Inventory class to ensure events are triggered
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
            return new InventorySlot(parentInventory) { itemData = itemData, stackSize = stackSize, item = item };
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

        /// <summary>
        /// 2 ways to use an item: <br/> 
        /// 1) Use the item directly (if it has a custom behaviour) <br/>
        /// 2) Subscribe to the OnItemUsed event and handle the behaviour in the subscriber
        /// </summary>
        public void UseItem()
        {
            Debug.Log("Use Item");
            OnItemUsed?.Invoke(this);
            (item as IUseable)?.Use(this);
        }

        public bool IsMaxStack() => itemData && stackSize == itemData.maxStackSize;
        public bool IsOccupied() => itemData != null && stackSize > 0;
        public bool IsSameItem(InventorySlot slot) => itemData == slot.itemData;
    }
}