using Character;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public enum EquipmentType
    {
        Armour,
        Weapon,
    }
    [CreateAssetMenu(fileName = "New Equipable Item", menuName = "Items/Equipable Item")]
    public class EquipableItem : Item
    {
        [Header("Equipable Item Properties")]
        public EquipmentType equipmentType;
        public List<StatModification> statModifiers;
    }
}