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

            foreach (var mod in Modifiers)
            {
                mod.ApplyMod(level, item);
            }

            return new InventoryItem(item, 1);
        }
    }
}