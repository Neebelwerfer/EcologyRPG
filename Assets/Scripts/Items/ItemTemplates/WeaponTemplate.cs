using Character;
using Character.Abilities;
using UnityEngine;

namespace Items.ItemTemplates
{
    [CreateAssetMenu(fileName = "New Weapon template", menuName = "Items/Templates/Weapon")]
    public class WeaponTemplate : EquipableItemTemplate
    {
        [Header("Weapon Properties")]
        public float minDamage;
        public float maxDamage;

        public PlayerAbility WeaponAttackAbility;

        public WeaponTemplate()
        {
            equipmentType = EquipmentType.Weapon;
        }

        public override InventoryItem GenerateItem(int level)
        {
            var item = new Weapon
            {
                Name = Name,
                Description = Description,
                Icon = Icon,
                Weight = Weight
            };

            var ability = Instantiate(WeaponAttackAbility);
            //ability.BaseDamage = Random.Range(minDamage, maxDamage);
            item.WeaponAbility = ability;

            foreach (var mod in Modifiers)
            {
                mod.ApplyMod(level, item);
            }

            return new InventoryItem(item, 1);
        }
    }
}