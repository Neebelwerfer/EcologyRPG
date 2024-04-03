using EcologyRPG._Core.Character;
using System.Collections;
using UnityEngine;

namespace EcologyRPG._Core.Abilities.AbilityData
{
    [CreateAssetMenu(fileName = "new Player ability data", menuName = "Ability/Player Ability Data")]
    public class PlayerAbilityDefinition : AttackAbilityDefinition
    {
        [Header("Resources")]
        [Tooltip("The resource that get used for the ability cost"), StatAttribute(StatType.Resource)]
        public string ResourceName;
        [Tooltip("The resource cost of this ability")]
        public float ResourceCost = 0;
        [Tooltip("The description of the ability"), TextArea(1, 5)]
        public string Description;
        [Tooltip("The trigger to set in the animator when the ability is casted")]
        public string AnimationTrigger;

        int triggerHash;

        public override void Initialize(BaseCharacter owner)
        {
            base.Initialize(owner);
            triggerHash = Animator.StringToHash(AnimationTrigger);
        }

        public override bool CanActivate(BaseCharacter caster)
        {
            if (!base.CanActivate(caster)) return false;
            if (ResourceName != "" && caster.Stats.GetResource(ResourceName) < ResourceCost)
            {
                return false;
            }
            return true;
        }

        public override IEnumerator HandleCast(CastInfo caster)
        {
            if (ResourceCost > 0)
            {
                InitialCastCost(caster);
            }
            return base.HandleCast(caster);
        }

        public override void CastStarted(CastInfo caster)
        {
            if (triggerHash != 0)
            {
                caster.owner.Animator.SetTrigger(triggerHash);
            }
            base.CastStarted(caster);
        }

        /// <summary>
        /// Called when the cast is started to deduct the resource cost
        /// </summary>
        /// <param name="caster"></param>
        protected virtual void InitialCastCost(CastInfo caster)
        {
            var resource = caster.owner.Stats.GetResource(ResourceName);
            resource -= ResourceCost;
        }
    }
}