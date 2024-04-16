using EcologyRPG.Core.Items;
using System;
using UnityEngine;

namespace EcologyRPG.GameSystems.Crafting
{
    [Serializable]
    public class Recipe : ScriptableObject
    {
        public string Name;
        public string Description;
        public bool OneTimeCraft;
        public RequiredItem[] RequiredItems = new RequiredItem[0];

        public bool CanCraft()
        {
            var inventory = Player.PlayerInventory;
            foreach (var requiredItem in RequiredItems)
            {
                var item = inventory.GetInventoryItem(requiredItem.ItemRef.Get());
                if (item == null || item.amount < requiredItem.Amount)
                {
                    return false;
                }
            }
            return true;
        }

        public string GetRequiredItemsString()
        {
            string result = "Need:\n";
            foreach (var requiredItem in RequiredItems)
            {
                result += $"{requiredItem.Amount}x {requiredItem.ItemRef.Get().Name}\n";
            }
            return result;
        }

        public virtual void Craft()
        {
            if (!CanCraft()) return;
            var inventory = Player.PlayerInventory;
            foreach (var requiredItem in RequiredItems)
            {
                var inventoryItem = inventory.GetInventoryItem(requiredItem.ItemRef.Get().Name);
                inventory.RemoveItem(inventoryItem.item, requiredItem.Amount);
            }
        }
    }

    [Serializable]  
    public struct RequiredItem
    {
        public ItemReference ItemRef;
        public int Amount;
    }
}