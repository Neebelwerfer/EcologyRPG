using Character;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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
        public Equipment equipment;
        public UnityEvent InventoryChanged;
        public UnityEvent<Item> ItemRemoved;
        public UnityEvent<Item> ItemAdded;

        GameObject ItemPrefab;
        Stat CarryWeight;
        BaseCharacter Owner;

        float currentWeight;
        public float CurrentWeight { get { return currentWeight; } }

        public Inventory(BaseCharacter Owner, Item[] startingItems)
        {
            this.Owner = Owner;
            CarryWeight = Owner.Stats.GetStat("CarryWeight");
            items = new List<InventoryItem>();
            ItemPrefab = Resources.Load<GameObject>("Prefabs/ItemPrefab");

            foreach (Item item in startingItems)
            {
                if(item != null)
                    AddItem(item);
            }
            equipment = new Equipment(Owner);
            InventoryChanged = new UnityEvent();
            ItemRemoved = new UnityEvent<Item>();
            ItemAdded = new UnityEvent<Item>();
        }

        public bool CanCarry(Item item, int amount)
        {
            return item.Weight * amount + currentWeight <= CarryWeight.Value;
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

        public bool AddItems(Item item, int amount)
        {
            if (item.Weight * amount + currentWeight > CarryWeight.Value)
            {
                return false;
            }

            var getInventoryItem = GetInventoryItem(item);
            currentWeight += item.Weight * amount;

            if (getInventoryItem != null)
            {
                GetInventoryItem(item).amount += amount;
                InventoryChanged?.Invoke();
                return true;
            }
            else
            {
                items.Add(new InventoryItem(item, amount));
                ItemAdded?.Invoke(item);
                return true;
            }
        }

        public bool AddItem(Item item)
        {
            return AddItems(item, 1);
        }

        public void DropItem(Item item, int amount)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].item == item)
                {
                    if (items[i].amount < amount)
                    {
                        amount = items[i].amount;
                    }
                    currentWeight -= item.Weight * amount;
                    items[i].amount -= amount;
                    if (items[i].amount <= 0)
                    {
                        items.RemoveAt(i);
                        ItemRemoved?.Invoke(item);
                    } else InventoryChanged?.Invoke();
                    
                    Physics.Raycast(Owner.transform.position + new Vector3(0, 1, 0), -Owner.transform.up, out RaycastHit hit, 3, LayerMask.GetMask("Ground"));
                    GameObject droppedItem = Object.Instantiate(ItemPrefab, null);
                    droppedItem.transform.position = hit.point;
                    droppedItem.GetComponentInChildren<ItemPickup>().Setup(this, item, amount);
                    break;
                }
            }
        }

        public void DropItem(Item item) => DropItem(item, 1);

        public void RemoveItem(Item item)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].item == item)
                {
                    currentWeight -= item.Weight;
                    items[i].amount--;
                    if (items[i].amount <= 0)
                    {
                        items.RemoveAt(i);
                        ItemRemoved?.Invoke(item);
                        break;
                    }
                    else
                    {
                        InventoryChanged?.Invoke();
                        break;
                    }
                }
            }
        }

        public void EquipItem(EquipableItem equip)
        {
            if(equipment.GetEquipment(equip.equipmentType) != null)
            {
                UnequipItem(equipment.GetEquipment(equip.equipmentType));
            }
            RemoveItem(equip);
            currentWeight += equip.Weight;
            equipment.EquipItem(equip);
        }

        public void UnequipItem(EquipableItem equip)
        {
            equipment.UnequipItem(equip);
            currentWeight -= equip.Weight;
            AddItem(equip);
        }

        public void ConsumeItem(ConsumableItem consumable)
        {
            consumable.Use(Owner);
            RemoveItem(consumable);
        }
    }
}

