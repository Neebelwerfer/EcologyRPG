using Items;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility.Events;

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
        if(Vector3.Distance(PlayerObject.transform.position, transform.position) < pickupRadius)
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
        EventManager.Defer("TooltipEntered", new TooltipEvent() { source = this, Title = InventoryItem.item.Name, Icon = InventoryItem.item.Icon, Description = InventoryItem.item.GetDisplayString() }, Utility.Collections.Priority.VeryLow);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (Vector3.Distance(PlayerObject.transform.position, transform.position) < pickupRadius)
        {
            base.OnPointerExit(eventData);
        }
        EventManager.Defer("TooltipExited", new TooltipEvent() { source = this }, Utility.Collections.Priority.VeryLow);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.Defer("TooltipExited", new TooltipEvent() { source = this }, Utility.Collections.Priority.VeryLow);
    }
}
