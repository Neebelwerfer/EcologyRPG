using EcologyRPG.Core.Items;
using EcologyRPG.Core.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace EcologyRPG.Game.UI
{
    public class InventoryButton : Button, ITooltip
    {
        public Inventory inventory;
        InventoryItem item;
        Image Image;
        TextMeshProUGUI text;

        bool clickedOnce = false;

        public void Setup(InventoryItem item)
        {
            if (text == null)
                text = GetComponentInChildren<TextMeshProUGUI>();
            if (Image == null)
                Image = GetComponentInChildren<Image>();

            this.item = item;
            text.text = item.amount + "x ";
            Image.sprite = item.item.Icon;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            Tooltip.ShowTooltip(this);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.HideTooltip(this);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (!clickedOnce)
                {
                    clickedOnce = true;
                    StartCoroutine(ResetClick());
                    return;
                }
                if (clickedOnce)
                {
                    inventory.UseItem(item);
                    clickedOnce = false;
                    return;
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                ContextUI.Instance.Open();
                if (item.item is EquipableItem item1)
                {
                    ContextUI.Instance.CreateButton("Equip", () => inventory.EquipItem(item1));
                }
                else if (item.item is ConsumableItem item2)
                {
                    ContextUI.Instance.CreateButton("Use", () => inventory.ConsumeItem(item2));
                }
                ContextUI.Instance.CreateButton("Drop", () => inventory.DropItem(item.item, 1));
                ContextUI.Instance.CreateButton("Drop All", () => inventory.DropItem(item.item, item.amount));
            }
        }

        public IEnumerator ResetClick()
        {
            yield return new WaitForSeconds(0.5f);
            clickedOnce = false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Tooltip.HideTooltip(gameObject);
        }

        public TooltipData GetTooltipData()
        {
            return new TooltipData() { Title = item.item.Name, Icon = item.item.Icon, Description = item.item.GetDisplayString() };
        }
    }
}