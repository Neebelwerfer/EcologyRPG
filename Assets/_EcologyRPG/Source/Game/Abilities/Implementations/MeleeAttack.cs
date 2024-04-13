using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities
{
    public class MeleeAttack : WeaponAttack
    {
        public float width;
        public float angle = 45;
        BaseCharacter[] targets;

        public override void Cast(CastInfo caster)
        {

            var dir = GetDir(caster);

            if (targetType == TargetType.Cone)
                targets = TargetUtility.GetTargetsInCone(caster.castPos, dir, angle, Range, AbilityManager.TargetMask);
            else if (targetType == TargetType.Line)
                targets = TargetUtility.GetTargetsInLine(caster.castPos, dir, new Vector3(width / 2, 2, Range / 2), AbilityManager.TargetMask);
            else if (targetType == TargetType.Circular)
                targets = TargetUtility.GetTargetsInRadius(caster.castPos, Range, AbilityManager.TargetMask);

            foreach (var target in targets)
            {
                if (target != null && target.Faction != caster.owner.Faction)
                {
                    DefaultOnHitAction()(caster, target);
                }
            }
        }
    }
}
