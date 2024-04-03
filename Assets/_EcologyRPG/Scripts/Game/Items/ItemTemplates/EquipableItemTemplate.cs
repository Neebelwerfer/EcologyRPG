using EcologyRPG.Core.Items;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Items
{
    public abstract class EquipableItemTemplate : ItemTemplate
    {
        [Header("Equipable Item Properties")]
        public EquipmentType equipmentType;
        public List<Ranges> Modifiers = new();

    }
}