using EcologyRPG.Core.Character;
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

        [ContextMenu("Delete")]
        protected override void Delete()
        {
            base.Delete();
            DestroyImmediate(Ability, true);
        }
    }
}