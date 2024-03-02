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
        Mask,
        WaterTank
    }
    public abstract class EquipableItem : Item
    {
        [Header("Equipable Item Properties")]
        public EquipmentType equipmentType;
        public List<StatModification> statModifiers = new List<StatModification>();
        public virtual void Equip(BaseCharacter character)
        {
            if(statModifiers == null)
            {
                return;
            }
            foreach (StatModification mod in statModifiers)
            {
                character.Stats.AddStatModifier(mod);
            }
        }

        public virtual void Unequip(BaseCharacter character)
        {
            character.Stats.RemoveStatModifiersFromSource(this);
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
        public float minDamage;
        public float maxDamage;

        BaseAbility WeaponAbility;

        public Weapon()
        {
            equipmentType = EquipmentType.Weapon;
        }
    }

    [CreateAssetMenu(fileName = "New Mask", menuName = "Items/Mask")]
    public class Mask :EquipableItem
    {
        
    }

    [CreateAssetMenu(fileName = "New Water Tank", menuName = "Items/Water Tank")]
    public class WaterTank : EquipableItem
    {

    }
}