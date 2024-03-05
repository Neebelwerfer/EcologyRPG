using Character;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ItemTemplates
{
    public abstract class EquipableItemTemplate : ItemTemplate
    {
        [Header("Equipable Item Properties")]
        public EquipmentType equipmentType;
        public List<Ranges> Modifiers = new();

    }
}