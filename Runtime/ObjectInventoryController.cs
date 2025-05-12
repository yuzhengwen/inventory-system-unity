using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventorySystem
{
    public class ObjectInventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryGroup inventoryGroup;
        [SerializeField] private UI_InventoryGroup uiGroup;

        public InventoryUIState state = InventoryUIState.Closed;

        private void Start()
        {
            uiGroup.uiInventory.AssignInventory(uiGroup.inventoryToAssign);
            uiGroup.Toggle(false);
        }

        #region Opening Inventory

        public void ToggleInventory()
        {
            if (state == InventoryUIState.OpenInteractable)
                ToggleInventory(false);
            else if (state == InventoryUIState.Closed)
                ToggleInventory(true);
        }
        public void ToggleInventory(bool open)
        {
            uiGroup.Toggle(open);
            state = open ? InventoryUIState.OpenInteractable : InventoryUIState.Closed;
        }

        #endregion
    }
}