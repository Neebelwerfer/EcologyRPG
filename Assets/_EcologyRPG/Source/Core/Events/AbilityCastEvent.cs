using EcologyRPG.Core.Scripting;

namespace EcologyRPG.Core.Events
{
    public class AbilityCastEvent : EventData
    {
        public CastContext Caster;
        public int AbilityID;

        public AbilityCastEvent(CastContext caster, int abilityID)
        {
            Caster = caster;
            abilityID = AbilityID;
        }
    }
}