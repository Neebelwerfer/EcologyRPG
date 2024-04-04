using EcologyRPG.Core.Character;
using EcologyRPG.Core.Items;
using UnityEngine;

namespace EcologyRPG.GameSystems.Items
{
    [CreateAssetMenu(fileName = "New WaterTank template", menuName = "Items/Templates/WaterTank")]
    public class WaterTankTemplate : EquipableItemTemplate
    {
        public WaterTankTemplate()
        {
            equipmentType = EquipmentType.WaterTank;
            Modifiers.Add(new Ranges { StatName = "armor", modType = StatModType.Flat, minValue = 0, maxValue = 0 });
        }

        public override InventoryItem GenerateItem(int level)
        {
            var item = CreateInstance<WaterTank>();
            item.Name = Name;
            item.Description = Description;
            item.Icon = Icon;
            item.Weight = Weight;

            foreach (var mod in Modifiers)
            {
               mod.ApplyMod(level, item);
            }

            return new InventoryItem(item, 1);
        }
    }
}