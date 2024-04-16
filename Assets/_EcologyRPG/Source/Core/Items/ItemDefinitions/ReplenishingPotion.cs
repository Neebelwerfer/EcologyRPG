using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.Core.Items
{
    [CreateAssetMenu(fileName = "replenishing Potion", menuName = "Items/Consumable/replenishingPotion")]
    public class ReplenishingPotion : ConsumableItem
    {
        [StatAttribute(StatType.Resource)]
        public string ResourceName;
        public float AmountToReplenish;

        public override void Use(BaseCharacter player)
        {
            base.Use(player);
            var resource = player.Stats.GetResource(ResourceName);
            resource.ModifyCurrentValue(resource.MaxValue * AmountToReplenish);
        }

        public override bool Equals(Item other)
        {
            if (!base.Equals(other)) return false;
            var consumable = other as ReplenishingPotion;
            if (consumable == null) return false;
            if (consumable.ResourceName != ResourceName) return false;
            if (consumable.AmountToReplenish != AmountToReplenish) return false;
            return true;
        }
    }
}