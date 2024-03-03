using Character;
using UnityEngine;

namespace Items.ItemTemplates
{
    [CreateAssetMenu(fileName = "New WaterTank template", menuName = "Items/Templates/WaterTank")]
    public class WaterTankTemplate : EquipableItemTemplate
    {
        public WaterTankTemplate()
        {
            equipmentType = EquipmentType.WaterTank;
            statModifiers.Add(new StatRanges { statName = "armor", modType = StatModType.Flat, minValue = 0, maxValue = 0 });
        }

        public override InventoryItem GenerateItem(int level)
        {
            var item = new WaterTank();
            item.Name = Name;
            item.Description = Description;
            item.Icon = Icon;
            item.Weight = Weight;

            foreach (var mod in statModifiers)
            {
                var value = mod.GrowthPerLevel * level;
                float min;
                float max;
                if (mod.growthType == GrowthType.Percentage)
                {
                    value /= 100;
                    min = mod.minValue + mod.minValue * value;
                    max = mod.maxValue + mod.maxValue * value;
                }
                else
                {
                    min = mod.minValue + value;
                    max = mod.maxValue + value;
                }

                item.statModifiers.Add(new StatModification(mod.statName, Random.Range(min, max), mod.modType, item));
            }

            return new InventoryItem(item, 1);
        }
    }
}