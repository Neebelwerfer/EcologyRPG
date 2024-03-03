using Character;
using UnityEngine;

namespace Items.ItemTemplates
{
    [CreateAssetMenu(fileName = "New Weapon template", menuName = "Items/Templates/Weapon")]
    public class WeaponTemplate : EquipableItemTemplate
    {
        [Header("Weapon Properties")]
        public float minDamage;
        public float maxDamage;

        public WeaponTemplate()
        {
            equipmentType = EquipmentType.Weapon;
        }

        public override InventoryItem GenerateItem(int level)
        {
            var item = new Weapon();
            item.Name = Name;
            item.Description = Description;
            item.Icon = Icon;
            item.Weight = Weight;
            item.minDamage = minDamage;
            item.maxDamage = maxDamage;

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