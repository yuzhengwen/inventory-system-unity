using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class ItemUtils : MonoBehaviour
    {
        public ItemDB ItemDB;
        public Dictionary<int, ItemDataSO> ItemDataDict = new();
        public static ItemUtils Instance { get; private set; }

        public virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            foreach (ItemDataSO item in ItemDB.items)
            {
                ItemDataDict.Add(item.id, item);
            }
        }

        /// <summary>
        /// Factory method to create an instance of an item (Item Class contains custom behaviours)
        /// Returns null if no custom Item Class defined for that item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public BaseInventoryItem CreateItemInstance(int id, GameObject owner)
        {
            if (id == ItemDB.CRYSTAL) return new CrystalItem(owner);
            return null;
        }
        public ItemDataSO GetItemDataById(int id)
        {
            return ItemDataDict[id];
        }
        public ItemDataSO GetItemDataByName(string displayName)
        {
            return ItemDB.items.FirstOrDefault(x => x.displayName == displayName);
        }
    }
}