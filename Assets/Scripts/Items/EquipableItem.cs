using Character;
using Character.Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public enum EquipmentType
    {
        Armour,
        Weapon,
    }
    public abstract class EquipableItem : Item
    {
        [Header("Equipable Item Properties")]
        public EquipmentType equipmentType;
        public List<StatModification> statModifiers;

        public virtual void Equip(BaseCharacter character)
        {
            foreach (StatModification mod in statModifiers)
            {
                character.stats.AddStatModifier(mod);
            }
        }
    }

    [CreateAssetMenu(fileName = "New Armour", menuName = "Items/Armour")]
    public class Armour : EquipableItem
    {
        public float armourValue;

        StatModification armourMod;

        public Armour()
        {
            equipmentType = EquipmentType.Armour;
            armourMod = new StatModification("armor", armourValue, StatModType.Flat, this);
            
        }
    }

    [CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
    public class Weapon : EquipableItem
    {
        public float damage;

        StatModification damageMod;
        BaseAbility WeaponAbility;

        public Weapon()
        {
            equipmentType = EquipmentType.Weapon;
            damageMod = new StatModification("weaponDamage", damage, StatModType.Flat, this);
        }
    }
}