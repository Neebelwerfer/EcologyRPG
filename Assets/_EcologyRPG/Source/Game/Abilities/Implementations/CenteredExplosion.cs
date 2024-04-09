using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EcologyRPG.GameSystems.Abilities
{
    public class CenteredExplosion : BaseAbility
    {
        [Header("Centered Explosion")]
        [Tooltip("The radius of the explosion")]
        public float Radius;
        [Tooltip("Debuffs that will be applied to the targets when the explosion hits")]
        public List<AbilityComponent> OnHitEffects = new();

        BaseCharacter[] targets;

        public override void CopyComponentsTo(BaseAbility ability)
        {
            base.CopyComponentsTo(ability);
            if (ability is CenteredExplosion explosionAbility)
            {
                explosionAbility.OnHitEffects = new List<AbilityComponent>();
                for (int i = 0; i < OnHitEffects.Count; i++)
                {
                    var newEffect = OnHitEffects[i].GetCopy(ability);
                    explosionAbility.OnHitEffects.Add(newEffect);
                }
            }
        }

        public override void Cast(CastInfo caster)
        {
            foreach (var effect in OnCastEffects)
            {
                effect.ApplyEffect(caster, null);
            }

            targets = TargetUtility.GetTargetsInRadius(caster.castPos, Radius, Game.Settings.EntityMask);

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
