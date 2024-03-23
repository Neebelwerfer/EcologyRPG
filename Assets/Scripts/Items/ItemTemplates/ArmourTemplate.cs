using Character;
using UnityEngine;

namespace Items.ItemTemplates
{

    [CreateAssetMenu(fileName = "New Armour template", menuName = "Items/Templates/Armour")]
    public class ArmourTemplate : EquipableItemTemplate
    {

        public ArmourTemplate()
        {
            equipmentType = EquipmentType.Armour;
            Modifiers.Add(new Ranges { StatName = "armor", modType = StatModType.Flat, minValue = 0, maxValue = 0 });
        }

        public override InventoryItem GenerateItem(int level)
        {
            var item = new Armour();
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