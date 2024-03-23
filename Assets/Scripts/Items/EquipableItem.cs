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

        public virtual void Equip(BaseCharacter character)
        {
            if(statModifiers == null)
            {
                return;
            }
            foreach (StatModification mod in statModifiers)
            {
                if(string.IsNullOrEmpty(mod.StatName)) continue;
                character.Stats.AddStatModifier(mod);
            }

            foreach (AttributeModification mod in attributeModifiers)
            {
                if(string.IsNullOrEmpty(mod.AttributeName)) continue;
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
                var mods = GetModifiers();
                DisplayString = desc + mods;
                return DisplayString;
            }
            else
            {
                return DisplayString;
            }
        }

        protected string GetModifiers()
        {
            var mods = "";
            foreach (var mod in statModifiers)
            {
                if (string.IsNullOrEmpty(mod.StatName)) continue;
                var stat = Stats.StatsData.Stats.Find(x => x.name == mod.StatName);
                if (stat.ShowOptions == ShowOptions.Never) continue;
                var displayName = stat.displayName;
                mods += displayName + ": " + GetStatModValue(mod) + "\n";
            }
            foreach (var mod in attributeModifiers)
            {
                if (string.IsNullOrEmpty(mod.AttributeName)) continue;
                var attr = Stats.StatsData.Attributes.Find(x => x.name == mod.AttributeName);
                var displayName = attr.displayName;
                mods += displayName + ": " + mod.Value + "\n";
            }
            return mods;
        }

        string GetStatModValue(StatModification mod)
        {
            if (mod.ModType == StatModType.Flat)
            {
                return ((int)mod.Value).ToString();
            }
            else
            {
                return ((int)(mod.Value * 100)).ToString() + "%";
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

        public override string GetDisplayString()
        {
            if (string.IsNullOrEmpty(DisplayString))
            {
                var desc = Description + "\n";
                var weaponDamage = (int)statModifiers.Find(x => x.StatName == "rawWeaponDamage").Value;
                desc += "Damage: " + weaponDamage + "\n";
                var mods = GetModifiers();
                DisplayString = desc + mods;
                return DisplayString;
            }
            else
            {
                return DisplayString;
            }
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