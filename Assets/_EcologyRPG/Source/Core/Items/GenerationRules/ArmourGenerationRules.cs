using EcologyRPG.Core.Character;

namespace EcologyRPG.Core.Items
{
    public class ArmourGenerationRules : EquipmentGenerationRules
    {
        ArmourGenerationRules()
        {
            this.Modifiers = new System.Collections.Generic.List<Ranges>
                {
                    new Ranges
                    {
                        StatName = "armor",
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