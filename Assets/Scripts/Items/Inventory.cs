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

    public class ItemPickupEvent: EventData
    {
        public InventoryItem item;
    }

    public class Inventory
    {
        public List<InventoryItem> items;
        public Equipment equipment;
        public UnityEvent InventoryChanged;
        public UnityEvent<Item> ItemRemoved;
        public UnityEvent<Item> ItemAdded;

        public float MaxCarryWeight { get { return CarryWeight.Value; } }

        Stat CarryWeight;
        BaseCharacter Owner;

        float currentWeight;
        public float CurrentWeight { get { return currentWeight; } }

        public Inventory(BaseCharacter Owner, Item[] startingItems)
        {
            this.Owner = Owner;
            CarryWeight = Owner.Stats.GetStat("CarryWeight");
            items = new List<InventoryItem>();

            foreach (Item item in startingItems)
            {
                if(item != null)
                    AddItem(item);
            }
            equipment = new Equipment(Owner);
            InventoryChanged = new UnityEvent();
            ItemRemoved = new UnityEvent<Item>();
            ItemAdded = new UnityEvent<Item>();

            EventManager.AddListener("ItemPickup", OnItemPickup);
        }

        private void OnItemPickup(EventData data)
        {
            if (data is ItemPickupEvent ipEvent)
            {
                var item = ipEvent.item;
                if (AddItems(item.item, item.amount))
                {
                    ItemDisplayHandler.Instance.RemoveItemPickup((data.source as ItemPickup));
                }
            }
        }

        public bool CanCarry(Item item, int amount)
        {
            return item.Weight * amount + currentWeight <= CarryWeight.Value;
        }

        public bool ContainsItem(Item item)
        {
            foreach (InventoryItem inventoryItem in items)
            {
                if (inventoryItem.item.Equals(item))
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
                if (inventoryItem.item.Equals(item))
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

            currentWeight += item.Weight * amount;

            if (item is EquipableItem)
            {
                for (int i = 0; i < amount; i++)
                {
                    items.Add(new InventoryItem(item, 1));
                }
                ItemAdded?.Invoke(item);
                return true;
            } 
            else
            {
                var getInventoryItem = GetInventoryItem(item);

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
            
        }

        public bool AddItem(Item item)
        {
            return AddItems(item, 1);
        }

        public void DropItem(Item item, int amount)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].item.Equals(item))
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
                    ItemDisplayHandler.Instance.SpawnItem(item, amount, hit.point + new Vector3(0, 1, 0));
                    break;
                }
            }
        }

        public void DropItem(Item item) => DropItem(item, 1);

        public void RemoveItem(Item item)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].item.Equals(item))
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

        public void UseItem(InventoryItem item)
        {
            if(item.item is ConsumableItem consumable)
            {
                ConsumeItem(consumable);
            }
            else if(item.item is EquipableItem equipable)
            {
                EquipItem(equipable);
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

