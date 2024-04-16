using UnityEngine;

namespace EcologyRPG.Core.Items
{
    [CreateAssetMenu(fileName = "New Mask", menuName = "Items/Mask")]
    public class Mask :EquipableItem
    {
        Mask() : base()
        {
            equipmentType = EquipmentType.Mask;
        }
    }
}