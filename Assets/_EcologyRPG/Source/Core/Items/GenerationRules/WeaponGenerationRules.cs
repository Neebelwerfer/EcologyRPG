using EcologyRPG.Core.Character;

namespace EcologyRPG.Core.Items
{
    public class WeaponGenerationRules : EquipmentGenerationRules
    {
        WeaponGenerationRules()
        {
            this.Modifiers = new System.Collections.Generic.List<Ranges>
                {
                    new Ranges
                    {
                        StatName = "rawWeaponDamage",
                        type = ModType.Stat,
                        modType = StatModType.Flat,
                        minValue = 0,
                        maxValue = 10,
                        GrowthPerLevel = 1,
                        growthType = GrowthType.Flat
                    }
                };
        }
    }
}