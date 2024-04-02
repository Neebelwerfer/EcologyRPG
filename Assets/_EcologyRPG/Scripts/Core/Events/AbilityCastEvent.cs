using EcologyRPG._Core.Abilities;

namespace EcologyRPG._Core.Events
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