using EcologyRPG.Core.Character;

namespace EcologyRPG.Core.Scripting
{
    public class StatModifierContext
    {
        public StatModification Modifier { get; private set; }
        public StatModifierContext(StatModification modifier)
        {
            Modifier = modifier;
        }

        public void SetValue(float value)
        {
            Modifier.Value = value;
        }

        public void SetModType(int modType)
        {
            Modifier.ModType = (StatModType)modType;
        }
    }
}