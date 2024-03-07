using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace InventorySystem.Tests
{
    public class InventoryOperationsTest 
    {
        public ItemDataSO coin, crystal;

        [OneTimeSetUp]
        public void Setup()
        {
            coin = ScriptableObject.CreateInstance<ItemDataSO>();
            coin.maxStackSize = 10;
            crystal = ScriptableObject.CreateInstance<ItemDataSO>();
            crystal.maxStackSize = 1;
        }

        [UnityTest]
        public IEnumerator AddItemsTest()
        {
            var go = new GameObject();
            var inventory = go.AddComponent<Inventory>();
            inventory.AddItem(coin, 15);
            inventory.AddItem(crystal, 2);
            // Use yield to skip a frame.
            yield return null;

            List<InventorySlot> items = inventory.GetItems();
            Assert.AreEqual(items[0].stackSize, 10);
            Assert.AreEqual(items[1].stackSize, 5);
            Assert.AreEqual(items[2].stackSize, 1);
            Assert.AreEqual(items[3].stackSize, 1);
        }
    }
}