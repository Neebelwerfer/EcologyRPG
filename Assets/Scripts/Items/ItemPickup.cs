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

    Inventory inventory;
    Item Item;
    int Amount;
    GameObject PlayerObject;

    protected override void Start()
    {
        base.Start();
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        onClick.AddListener(OnClicked);

        var text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = Item.Name + " x" + Amount;
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
        if (inventory.AddItems(Item, Amount))
        {
            Destroy(transform.root.gameObject);
        }
    }

    public void Setup(Inventory inventory, Item item, int amount)
    {
        this.inventory = inventory;
        Item = item;
        Amount = amount;
    }
}
