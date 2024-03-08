using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class MouseDragItem : MonoBehaviour
    {
        private Image img;
        private TextMeshProUGUI stackDisplay;
        public UI_InventorySlot from;
        private void Awake()
        {
            img = GetComponentInChildren<Image>();
            stackDisplay = GetComponentInChildren<TextMeshProUGUI>();
        }
        public void SetItem(UI_InventorySlot from)
        {
            this.from = from;
            img.sprite = from.GetItem().itemData.sprite;
            int stackSize = from.GetItem().stackSize;
            stackDisplay.text = stackSize > 1 ? stackSize.ToString() : "";
        }
    }
}
