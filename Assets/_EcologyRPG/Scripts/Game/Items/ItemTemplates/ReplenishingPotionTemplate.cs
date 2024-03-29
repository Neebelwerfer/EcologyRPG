using EcologyRPG.Core.Items;
using UnityEngine;

namespace EcologyRPG.Game.Items
{
    [CreateAssetMenu(fileName = "Replenishing Potion", menuName = "Items/Templates/Replenishing Potion")]
    public class ReplenishingPotionTemplate : ConsumableItemTemplate
    {
        [Header("Replenishing Potion Properties")]
        public string ResourceName;
        public float AmountToReplenish;

        public override InventoryItem GenerateItem(int level)
        {
            var item = CreateInstance<ReplenishingPotion>();
            item.Name = Name;
            item.Description = Description;
            item.Icon = Icon;
            item.Weight = Weight;

            item.ResourceName = ResourceName;
            item.AmountToReplenish = AmountToReplenish;

            return new InventoryItem(item, 1);
        }
    }
}