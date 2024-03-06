using Items;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPickup : Button
{
    public float pickupRadius = 5;

    InventoryItem InventoryItem;
    GameObject PlayerObject;

    protected override void Start()
    {
        base.Start();
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        onClick.AddListener(OnClicked);

        var text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = InventoryItem.item.Name + " x" + InventoryItem.amount;
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
    }
}
