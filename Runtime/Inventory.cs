using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using YuzuValen.Utils;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [FormerlySerializedAs("startingInventory")] [Header("Starting Inventory Settings")] [SerializeField]
        InventoryItemListSO inventoryItemListSo;

        [Header("Inventory Settings")] [SerializeField]
        private int slotsCount = 10;

        [Header("View Inventory")]
        // ordered list of items
        [ReadOnlyInspector]
        [SerializeField]
        private List<InventorySlot> items = new();

        // dictionary for quick look up of items and their quantities
        private readonly Dictionary<ItemDataSO, int> itemQtys = new();

        //for showing dictioanry in inspector
        [Header("Item Quantity Display")] [ReadOnlyInspector] [SerializeField]
        private List<ItemDataSO> keys = new();

        [ReadOnlyInspector] [SerializeField] private List<int> values = new();

        // events
        public event Action<ItemDataSO, int> OnItemAdded;
        public event Action<ItemDataSO, int> OnItemRemoved;
        public event Action<InventorySlot> OnItemUsed;
        public event Action<List<InventorySlot>> OnSlotCountChanged;

        private void Awake()
        {
            // initialize empty inventory slots
            for (int i = 0; i < slotsCount; i++)
                items.Add(new InventorySlot(this));
            // subscribe to item used events
            foreach (var item in items)
                item.OnItemUsed += (i) => OnItemUsed?.Invoke(i);
        }

        private void Start()
        {
            // load starting inventory
            if (inventoryItemListSo != null)
                LoadInventoryFrom(inventoryItemListSo.startingItems);
        }

        private void OnEnable()
        {
            OnItemAdded += AddItemToDict;
            OnItemRemoved += RemoveItemFromDict;
        }

        private void OnDisable()
        {
            OnItemAdded -= AddItemToDict;
            OnItemRemoved -= RemoveItemFromDict;
#if UNITY_EDITOR
            keys.Clear();
            values.Clear();
#endif
        }

        [ContextMenu("Increase Slot Count")]
        public void IncreaseSlotCount()
        {
            slotsCount++;
            items.Add(new InventorySlot(this));
            OnSlotCountChanged?.Invoke(items);
        }

        /// <summary>
        /// Adds items from a list of InventorySlots
        /// </summary>
        /// <param name="items"></param>
        public void LoadInventoryFrom(List<InventorySlot> items)
        {
            ClearInventory();
            foreach (InventorySlot item in items)
            {
                AddItem(item);
            }
        }

        /// <summary>
        /// Loads inventory to be exactly the same as the list of InventorySlots passed in
        /// </summary>
        /// <param name="items"></param>
        public void LoadInventoryExact(List<InventorySlot> items)
        {
            if (items.Count != slotsCount)
            {
                Debug.LogError("Inventory size mismatch");
                return;
            }

            ClearInventory();
            for (int i = 0; i < slotsCount; i++)
            {
                if (items[i].IsOccupied())
                    this.items[i].SetItem(items[i].itemData, items[i].stackSize);
            }

            ManualUpdateDict();
        }

        public void AddItem(InventorySlot inventoryItem)
        {
            AddItem(inventoryItem.itemData, inventoryItem.stackSize);
        }

        public void AddItem(ItemDataSO itemData, int amount)
        {
            int initialAmt = amount;
            if (amount <= 0 || itemData == null)
            {
                Debug.LogError("Invalid amount or itemData");
                return;
            }

            // find ALL inventoryitems with this itemdata and add to stack if possible
            if (itemQtys.ContainsKey(itemData))
            {
                foreach (InventorySlot item in items)
                    if (item.itemData == itemData && !item.IsMaxStack())
                    {
                        amount = item.AddToStack(amount);
                        if (amount == 0)
                        {
                            OnItemAdded?.Invoke(itemData, initialAmt);
                            return;
                        }
                    }
            }

            // if we get here, we need to add new inventoryitem stacks
            // add as many maxstacksize items as needed
            while (amount / itemData.maxStackSize > 0)
            {
                AddNewItemInternal(itemData, itemData.maxStackSize);
                amount -= itemData.maxStackSize;
            }

            if (amount > 0)
            {
                // add remainder
                AddNewItemInternal(itemData, amount);
            }

            OnItemAdded?.Invoke(itemData, initialAmt);
        }

        private InventorySlot AddNewItemInternal(ItemDataSO itemData, int amount)
        {
            var emptySlot = GetNextEmptySlot();
            if (emptySlot == null)
            {
                Debug.LogError("No empty slots available");
                return null;
            }

            return emptySlot.SetItem(itemData, amount);
        }

        public void RemoveItem(ItemDataSO itemData, int amount)
        {
            int initialAmt = amount;
            if (amount <= 0 || itemData == null)
            {
                Debug.LogError("Invalid amount or itemData");
                return;
            }

            if (!itemQtys.ContainsKey(itemData))
            {
                Debug.LogError("Item not found in inventory!");
                return;
            }

            // Split into full and non-full stacks
            Stack<InventorySlot> fullStacks = new(), nonFullStacks = new();
            foreach (InventorySlot item in items)
            {
                if (item.itemData == itemData)
                    if (item.IsMaxStack())
                        fullStacks.Push(item);
                    else
                        nonFullStacks.Push(item);
            }

            // Remove from non-full stacks first
            InventorySlot itemToRemoveFrom = null;
            while (nonFullStacks.Count > 0 && amount != 0)
            {
                itemToRemoveFrom = nonFullStacks.Pop();
                amount = itemToRemoveFrom.RemoveFromStack(amount);
            }

            while (fullStacks.Count > 0 && amount != 0)
            {
                itemToRemoveFrom = fullStacks.Pop();
                amount = itemToRemoveFrom.RemoveFromStack(amount);
            }

            if (amount > 0)
                Debug.LogError($"{amount} of {itemData.displayName} couldn't be removed");
            OnItemRemoved?.Invoke(itemData, initialAmt - amount);
        }

        /// <summary>
        /// Closes up gaps in the inventory
        /// </summary>
        public void FillSpace()
        {
            InventorySlot[] filled = items.Where(item => item.IsOccupied()).ToArray();
            // if we simply store references to the inventoryslots, we will lose them when we clear the inventory
            filled = Array.ConvertAll(filled, i => i.Copy()); // creates copy of each item (not reference)
            ClearInventory();
            for (int i = 0; i < filled.Length; i++)
            {
                items[i].SetItem(filled[i]);
            }
        }

        public void ClearInventory()
        {
            foreach (InventorySlot item in items)
            {
                item.ClearSlot();
            }
        }

        public void PrintInventory()
        {
            foreach (InventorySlot item in items)
            {
                Debug.Log($"{item.itemData.displayName}: {item.stackSize}");
            }
        }

        public void RemoveFromSlot(InventorySlot slot, int amount)
        {
            OnItemRemoved?.Invoke(slot.itemData, amount);
            slot.RemoveFromStack(amount);
        }

        public List<InventorySlot> GetItems() => items;
        private InventorySlot GetNextEmptySlot() => items.FirstOrDefault(item => item.itemData == null);
        public bool IsFull() => items.All(item => item.IsOccupied());
        public bool IsEmpty() => items.All(item => !item.IsOccupied());
        public void SwapItems(InventorySlot slot1, InventorySlot slot2) => slot1.Swap(slot2);

        #region Item Quantity Dictionary Methods

        public bool Contains(ItemDataSO itemData) => itemQtys.ContainsKey(itemData);
        public int GetItemQuantity(ItemDataSO itemData) => itemQtys.ContainsKey(itemData) ? itemQtys[itemData] : 0;

        private void AddItemToDict(ItemDataSO itemData, int amount)
        {
            if (itemQtys.ContainsKey(itemData))
                itemQtys[itemData] += amount;
            else
                itemQtys[itemData] = amount;
#if UNITY_EDITOR
            SyncList();
#endif
        }

        private void RemoveItemFromDict(ItemDataSO itemData, int amount)
        {
            if (!itemQtys.ContainsKey(itemData)) return;

            itemQtys[itemData] = Mathf.Max(0, itemQtys[itemData] -= amount);
            if (itemQtys[itemData] == 0)
                itemQtys.Remove(itemData);
#if UNITY_EDITOR
            SyncList();
#endif
        }

        private void SyncList()
        {
            keys.Clear();
            values.Clear();
            foreach (var kvp in itemQtys)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        /// <summary>
        /// When SetItem is used directly instead of Add/RemoveItem, use this to update the dictionary
        /// </summary>
        private void ManualUpdateDict()
        {
            itemQtys.Clear();
            foreach (InventorySlot item in items)
                if (item.IsOccupied())
                    AddItemToDict(item.itemData, item.stackSize);
        }

        #endregion
    }
}