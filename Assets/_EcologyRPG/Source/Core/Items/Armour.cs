using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.Core.Items
{
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
}