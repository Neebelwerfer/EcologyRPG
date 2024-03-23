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
        public List<AttributeModification> attributeModifiers = new List<AttributeModification>();

        string DisplayString;

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

            foreach (AttributeModification mod in attributeModifiers)
            {
                character.Stats.AddAttributeModifier(mod);
            }
        }

        public virtual void Unequip(BaseCharacter character)
        {
            character.Stats.RemoveStatModifiersFromSource(this);
            character.Stats.RemoveAttributeModifiersFromSource(this);
        }

        public override string GetDisplayString()
        {
            if (string.IsNullOrEmpty(DisplayString))
            {
                var desc = Description + "\n";
                var mods = "\n";
                foreach (var mod in statModifiers)
                {
                    mods += mod.StatName + ": " + GetStatModValue(mod) + "\n";
                }
                foreach (var mod in attributeModifiers)
                {
                    mods += mod.name + ": " + mod.Value + "\n";
                }
                DisplayString = desc + mods;
                return DisplayString;
            }
            else
            {
                return DisplayString;
            }
        }

        string GetStatModValue(StatModification mod)
        {
            if (mod.ModType == StatModType.Flat)
            {
                return mod.Value.ToString();
            }
            else
            {
                return (mod.Value * 100).ToString() + "%";
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
        public PlayerAbilityDefinition WeaponAbility;

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