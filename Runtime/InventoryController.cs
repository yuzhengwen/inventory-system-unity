using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace InventorySystem
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private int interactableInvIndex = 0;

        [Header("List of Inventory Models & Accepted Item Types")] [SerializeField]
        private InventoryGroup[] inventoryGroups;

        [Header("List of Inventory Views and Model to bind")] [SerializeField]
        private UI_InventoryGroup[] uiGroups;

        public InventoryUIState state = InventoryUIState.Closed;

        [Header("Keybinds")] [SerializeField] private InputAction toggleInventoryAction;
        [SerializeField] private InputAction closeAction;
        [SerializeField] private InputAction interactAction;

        public event Action<ItemDataSO> OnItemCollected;

        private void Start()
        {
            foreach (var uiGroup in uiGroups)
            {
                uiGroup.uiInventory.AssignInventory(uiGroup.inventoryToAssign);
                uiGroup.Toggle(uiGroup.defaultOpen);
            }

            if (toggleInventoryAction != null)
            {
                toggleInventoryAction.performed += (ctx) => ToggleInventoryByInput();
                toggleInventoryAction.Enable();
            }

            if (closeAction != null)
            {
                closeAction = new("Close Inventory", InputActionType.Button, "<Keyboard>/escape");
                closeAction.performed += (ctx) => CloseInventory();
                closeAction.Enable();
            }

            if (interactAction != null)
            {
                interactAction.performed += (ctx) => InteractWithOtherInventory();
                interactAction.Enable();
            }
        }

        public void AddItemsToInventory(ItemDataSO itemData, int amt = 1)
        {
            foreach (var invGroup in inventoryGroups)
            {
                if (invGroup.acceptAllTypes)
                {
                    invGroup.inventory.AddItem(itemData, amt);
                    continue;
                }

                if (invGroup.acceptedTypes.Any(type => itemData.itemType == type))
                {
                    invGroup.inventory.AddItem(itemData, amt);
                }
            }

            OnItemCollected?.Invoke(itemData);
        }

        private void OnDisable()
        {
            toggleInventoryAction.Disable();
            closeAction.Disable();
        }

        [SerializeField] private ObjectInventoryController interactableController = null;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ObjectInventoryController controller))
            {
                interactableController = controller;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out ObjectInventoryController controller))
            {
                interactableController = null;
            }
        }

        public void InteractWithOtherInventory()
        {
            if (interactableController != null)
            {
                ToggleInventoryWithInteractable();
            }
        }

        #region Opening Inventory

        public void CloseInventory()
        {
            if (state == InventoryUIState.OpenInteractable) ToggleInventoryWithInteractable(false);
            else if (state == InventoryUIState.OpenByInput) ToggleInventoryByInput(false);
        }

        public void ToggleInventoryByInput(bool toggle)
        {
            foreach (var uiGroup in uiGroups)
            {
                if (uiGroup.toggledByInput)
                    uiGroup.Toggle(toggle);
            }

            state = toggle ? InventoryUIState.OpenByInput : InventoryUIState.Closed;
        }

        public void ToggleInventoryByInput()
        {
            switch (state)
            {
                case InventoryUIState.OpenInteractable:
                    ToggleInventoryWithInteractable(false);
                    break;
                case InventoryUIState.OpenByInput:
                    ToggleInventoryByInput(false);
                    break;
                default:
                    ToggleInventoryByInput(true);
                    break;
            }
        }

        public void ToggleInventoryWithInteractable(bool toggle)
        {
            interactableController.ToggleInventory(toggle);
            uiGroups[interactableInvIndex].Toggle(toggle);

            state = toggle ? InventoryUIState.OpenInteractable : InventoryUIState.Closed;
        }

        public void ToggleInventoryWithInteractable()
        {
            interactableController.ToggleInventory();
            ToggleInventoryWithInteractable(!uiGroups[interactableInvIndex].uiInventory.gameObject.activeSelf);
        }

        #endregion
    }

    [System.Serializable]
    public class UI_InventoryGroup
    {
        public UI_Inventory uiInventory;
        public Inventory inventoryToAssign;
        public bool canToggle = true;
        public bool toggledByInput;
        public bool defaultOpen = false;

        public void Toggle(bool open)
        {
            if (canToggle)
            {
                uiInventory.gameObject.SetActive(open);
            }
        }
    }

    [System.Serializable]
    public class InventoryGroup
    {
        public Inventory inventory;
        public ItemType[] acceptedTypes;
        public bool acceptAllTypes;
    }

    public enum InventoryUIState
    {
        OpenByInput,
        OpenInteractable,
        Closed
    }
}