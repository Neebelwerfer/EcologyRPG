using EcologyRPG.Core.Abilities;

namespace EcologyRPG.Core.Events
{
    public class AbilityCastEvent : EventData
    {
        public CastInfo Caster;
        public AbilityDefintion Ability;

        public AbilityCastEvent(CastInfo caster, AbilityDefintion ability)
        {
            Caster = caster;
            Ability = ability;
        }
    }
}