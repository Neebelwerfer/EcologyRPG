using EcologyRPG.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EcologyRPG.Core.Items
{
    public class ItemPickup : Button, IPointerEnterHandler, IPointerExitHandler
    {
        public float pickupRadius = 5;

        InventoryItem InventoryItem;
        GameObject PlayerObject;

        protected override void Start()
        {
            base.Start();
            PlayerObject = GameObject.FindGameObjectWithTag("Player");
            onClick.AddListener(OnClicked);
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
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
            if (Vector3.Distance(PlayerObject.transform.position, transform.position) < pickupRadius)
            {
                base.OnPointerEnter(eventData);
            }
            Tooltip.ShowTooltip(gameObject, new TooltipData() { Title = InventoryItem.item.Name, Icon = InventoryItem.item.Icon, Description = InventoryItem.item.GetDisplayString() });
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (Vector3.Distance(PlayerObject.transform.position, transform.position) < pickupRadius)
            {
                base.OnPointerExit(eventData);
            }
            Tooltip.HideTooltip(gameObject);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Tooltip.HideTooltip(gameObject);
        }
    }
}
