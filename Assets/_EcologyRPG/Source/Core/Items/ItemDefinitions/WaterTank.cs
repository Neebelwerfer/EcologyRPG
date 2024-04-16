using UnityEngine;

namespace EcologyRPG.Core.Items
{
    [CreateAssetMenu(fileName = "New Water Tank", menuName = "Items/Water Tank")]
    public class WaterTank : EquipableItem
    {
        WaterTank() : base()
        {
            equipmentType = EquipmentType.WaterTank;
        }
    }
}