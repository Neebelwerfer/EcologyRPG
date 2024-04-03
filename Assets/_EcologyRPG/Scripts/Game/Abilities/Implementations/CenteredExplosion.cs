using EcologyRPG._Core.Abilities.AbilityComponents;
using EcologyRPG._Core.Abilities;
using EcologyRPG._Core.Character;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG._Game.Abilities
{
    public class CenteredExplosion : BaseAbility
    {
        [Header("Centered Explosion")]
        [Tooltip("The layer mask of the targets that will be hit by the explosion")]
        public LayerMask targetMask;
        [Tooltip("The radius of the explosion")]
        public float Radius;
        [Tooltip("Debuffs that will be applied to the targets when the explosion hits")]
        public List<AbilityComponent> OnHitEffects = new();

        BaseCharacter[] targets;

        public override void Cast(CastInfo caster)
        {
            foreach (var effect in OnCastEffects)
            {
                effect.ApplyEffect(caster, null);
            }

            targets = TargetUtility.GetTargetsInRadius(caster.castPos, Radius, targetMask);

            if (targets != null && targets.Length > 0)
            {
                foreach (var t in targets)
                {
                    if (t == null) continue;
                    if (t.Faction == caster.owner.Faction) continue;


                    foreach (var effect in OnHitEffects)
                    {
                        effect.ApplyEffect(caster, t);
                    }
                }
            }
        }
    }
}
