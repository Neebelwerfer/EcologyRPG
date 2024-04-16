using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.Core.Items
{
    [CreateAssetMenu(fileName = "New Armour", menuName = "Items/Armour")]
    public class Armour : EquipableItem
    {

        public Armour() : base()
        {
            equipmentType = EquipmentType.Armour;
            modifications.Add(new EquipmentModification
            {
                StatName = "armor",
                type = ModType.Stat,
                modType = StatModType.Flat,
                Value = 1
            });
            
        }
    }
}