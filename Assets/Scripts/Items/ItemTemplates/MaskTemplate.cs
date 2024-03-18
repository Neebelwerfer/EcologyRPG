using Character;
using UnityEngine;

namespace Items.ItemTemplates
{
    [CreateAssetMenu(fileName = "New Mask template", menuName = "Items/Templates/Mask")]
    public class MaskTemplate : EquipableItemTemplate
    {

        public MaskTemplate()
        {
            equipmentType = EquipmentType.Mask;
            Modifiers.Add(new Ranges { name = "armor", modType = StatModType.Flat, minValue = 0, maxValue = 0 });
        }

        public override InventoryItem GenerateItem(int level)
        {
            var item = new Mask();
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