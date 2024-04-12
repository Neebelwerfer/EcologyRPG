using EcologyRPG.Core.Character;
using EcologyRPG.Core.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EcologyRPG.Core.Items
{
    public class ItemPickup : Button, IPointerEnterHandler, IPointerExitHandler, ITooltip
    {
        public float pickupRadius = 5;

        InventoryItem InventoryItem;

        protected override void Start()
        {
            base.Start();

            onClick.AddListener(OnClicked);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            var player = Characters.Instance.GetCharactersByTag("Player")[0];
            var PlayerObject = player.GameObject;
            if (Vector3.Distance(PlayerObject.transform.position, transform.position) < pickupRadius)
            {
                base.OnPointerClick(eventData);
            }
        }

        void OnClicked()
        {
            EventManager.Dispatch("ItemPickup", new ItemPickupEvent() { source = this, item = InventoryItem });
        }

        public void Setup(Item item, int amount)
        {
            InventoryItem = new InventoryItem(item, amount);

            var text = GetComponentInChildren<TextMeshProUGUI>();
            text.text = InventoryItem.item.Name + " x" + InventoryItem.amount;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            var player = Characters.Instance.GetCharactersByTag("Player")[0];
            var PlayerObject = player.GameObject;
            if (PlayerObject == null)
                return;
            if (Vector3.Distance(PlayerObject.transform.position, transform.position) < pickupRadius)
            {
                base.OnPointerEnter(eventData);
            }
            Tooltip.ShowTooltip(this);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            var player = Characters.Instance.GetCharactersByTag("Player")[0];
            var PlayerObject = player.GameObject;
            if (PlayerObject == null)
                return;
            if (Vector3.Distance(PlayerObject.transform.position, transform.position) < pickupRadius)
            {
                base.OnPointerExit(eventData);
            }
            Tooltip.HideTooltip(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Tooltip.HideTooltip(this);
        }

        public TooltipData GetTooltipData()
        {
            return new TooltipData() { Title = InventoryItem.item.Name, Icon = InventoryItem.item.Icon, Description = InventoryItem.item.GetDisplayString() };
        }
    }
}
