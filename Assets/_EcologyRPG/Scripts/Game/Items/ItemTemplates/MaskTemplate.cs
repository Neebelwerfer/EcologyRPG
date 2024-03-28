using EcologyRPG.Core.Character;
using EcologyRPG.Core.Items;
using UnityEngine;

namespace EcologyRPG.Game.Items
{
    [CreateAssetMenu(fileName = "New Mask template", menuName = "Items/Templates/Mask")]
    public class MaskTemplate : EquipableItemTemplate
    {

        public MaskTemplate()
        {
            equipmentType = EquipmentType.Mask;
            Modifiers.Add(new Ranges { StatName = "armor", modType = StatModType.Flat, minValue = 0, maxValue = 0 });
        }

        public override InventoryItem GenerateItem(int level)
        {
            var item = CreateInstance<Mask>();
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