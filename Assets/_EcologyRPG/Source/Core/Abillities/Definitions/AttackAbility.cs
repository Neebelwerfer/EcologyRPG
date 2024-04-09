using EcologyRPG.Core.Abilities.AbilityComponents;
using EcologyRPG.Core.Character;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace EcologyRPG.Core.Abilities
{
    public abstract class AttackAbility : BaseAbility
    {
        [Header("Attack Ability")]
        [Tooltip("Use the mouse direction as the direction of the ability instead of the cast position. This is useful for abilities that are casted in the direction of the mouse instead of the position of the caster.")]
        public bool useMouseDirection = false;
        [Tooltip("Effects that will be applied to the first target when the ability hits")]
        public List<AbilityComponent> OnFirstHit = new();
        [Tooltip("Effects that will be applied to the target when the ability hits")]
        public List<AbilityComponent> OnHitEffects = new();

        public bool displayFirstHitEffects = true;
        protected bool firstHit = true;

        public override void CopyComponentsTo(BaseAbility ability)
        {
            base.CopyComponentsTo(ability);
            if (ability is AttackAbility attackAbility)
            {
                attackAbility.OnFirstHit = new List<AbilityComponent>();
                for (int i = 0; i < OnFirstHit.Count; i++)
                {
                    var newEffect = OnFirstHit[i].GetCopy(ability);
                    attackAbility.OnFirstHit.Add(newEffect);
                }

                attackAbility.OnHitEffects = new List<AbilityComponent>();
                for (int i = 0; i < OnHitEffects.Count; i++)
                {
                    var newEffect = OnHitEffects[i].GetCopy(ability);
                    attackAbility.OnHitEffects.Add(newEffect);
                }
            }
        }

        protected virtual Action<CastInfo, BaseCharacter> DefaultOnHitAction()
        {
            return (castInfo, target) =>
            {
                if (firstHit)
                {
                    firstHit = false;
                    foreach (var effect in OnFirstHit)
                    {
                        effect.ApplyEffect(castInfo, target);
                    }
                }
                foreach (var effect in OnHitEffects)
                {
                    effect.ApplyEffect(castInfo, target);
                }
            };
        }

        protected Vector3 GetDir(CastInfo castInfo)
        {
            if (castInfo.dir != Vector3.zero)
            {
                return castInfo.dir;
            }
            else
            {
                return castInfo.owner.Transform.Forward;
            }
        }

        public override void Delete()
        {
            foreach (var effect in OnFirstHit)
            {
                effect.Delete();
            }
            foreach (var effect in OnHitEffects)
            {
                effect.Delete();
            }
            base.Delete();
        }
    }
}
