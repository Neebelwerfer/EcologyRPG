using UnityEngine;

namespace Items.ItemTemplates
{
    [CreateAssetMenu(fileName = "Replenishing Potion", menuName = "Items/Templates/Replenishing Potion")]
    public class ReplenishingPotionTemplate : ConsumableItemTemplate
    {
        [Header("Replenishing Potion Properties")]
        public string ResourceName;
        public float minReplenishAmount;
        public float maxReplenishAmount;

        [Header("Growth Properties")]
        public float GrowthPerLevel;
        public GrowthType growthType;

        public override InventoryItem GenerateItem(int level)
        {
            var item = CreateInstance<ReplenishingPotion>();
            item.Name = Name;
            item.Description = Description;
            item.Icon = Icon;
            item.Weight = Weight;

            item.ResourceName = ResourceName;
            var value = GrowthPerLevel * level;
            float min;
            float max;
            if(growthType == GrowthType.Percentage)
            {
                min = minReplenishAmount + (minReplenishAmount * value);
                max = maxReplenishAmount + (maxReplenishAmount * value);
            }
            else
            {
                min = minReplenishAmount + value;
                max = maxReplenishAmount + value;
            }
            item.AmountToReplenish = Random.Range(min, max);

            return new InventoryItem(item, 1);
        }
    }
}