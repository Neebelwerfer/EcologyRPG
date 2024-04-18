using EcologyRPG.Core.Character;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.Core.Abilities.AbilityData
{
    public class AttackAbilityDefinition : AbilityDefintion
    {
        public BaseAbility Ability;

        public override bool CanActivate(BaseCharacter caster)
        {
            if(!base.CanActivate(caster)) return false;
            return Ability.CanCast(caster);
        }

        public override void CastStarted(CastInfo castInfo)
        {
            base.CastStarted(castInfo);
            Ability.Windup(castInfo, CastWindupTime);
        }

        public override void CastCancelled(CastInfo caster)
        {
            base.CastCancelled(caster);

        }

        public override void CastFinished(CastInfo caster)
        {
            base.CastFinished(caster);
            if (caster.castPos == Vector3.zero) caster.castPos = caster.owner.CastPos;
            Ability.Cast(caster);
        }

#if UNITY_EDITOR

        public override AbilityDefintion GetCopy(Object owner)
        {
            var copy = base.GetCopy(owner) as AttackAbilityDefinition;
            copy.Ability = Ability.GetCopy(owner);
            return copy;
        }

        public override void Delete()
        {
            Ability.Delete();
            base.Delete();
        }
#endif
    }
}