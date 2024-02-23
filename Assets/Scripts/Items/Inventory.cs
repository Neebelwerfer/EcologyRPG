using Character;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Items
{
    public class InventoryItem
    {
        public Item item;
        public int amount;

        public InventoryItem(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }

    public class Inventory
    {
        public List<InventoryItem> items;

        Stat CarryWeight;
        BaseCharacter Owner;

        float currentWeight;
        public float CurrentWeight { get { return currentWeight; } }

        public Inventory(BaseCharacter Owner, Item[] startingItems)
        {
            this.Owner = Owner;
            CarryWeight = Owner.stats.GetStat("CarryWeight");
            items = new List<InventoryItem>();
            foreach (Item item in startingItems)
            {
                if(item != null)
                    AddItem(item);
            }
            Debug.Log("Inventory created");
        }

        public bool ContainsItem(Item item)
        {
            foreach (InventoryItem inventoryItem in items)
            {
                if (inventoryItem.item == item)
                {
                    return true;
                }
            }
            return false;
        }

        public InventoryItem GetInventoryItem(Item item)
        {
            foreach (InventoryItem inventoryItem in items)
            {
                if (inventoryItem.item == item)
                {
                    return inventoryItem;
                }
            }
            return null;
        }

        public bool AddItem(Item item)
        {
            if (item.Weight + currentWeight > CarryWeight.Value)
            {
                return false;
            }

            var getInventoryItem = GetInventoryItem(item);
            currentWeight += item.Weight;

            if (getInventoryItem != null)
            {
                GetInventoryItem(item).amount++;
                return true;
            }
            else
            {
                items.Add(new InventoryItem(item, 1));
                return true;
            }
        }

        public void RemoveItem(Item item)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].item == item)
                {
                    currentWeight -= item.Weight;
                    items.RemoveAt(i);
                }
            }
        }
    }
}

