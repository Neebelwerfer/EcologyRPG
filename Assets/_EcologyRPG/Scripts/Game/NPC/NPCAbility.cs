using EcologyRPG._Core.Abilities;
using UnityEngine;

namespace EcologyRPG._Game.NPC
{
    [CreateAssetMenu(menuName = "Ability/NPC Ability Data", fileName = "New NPC Ability Data")]
    public class NPCAbility : AbilityDefintion
    {
        [Tooltip("The trigger to set in the animator when the ability is casted")]
        public string AnimationTrigger;
        public BaseAbility Ability;

        int triggerHash;

        public void Initialise()
        {
            triggerHash = Animator.StringToHash(AnimationTrigger);
        }

        public override void CastStarted(CastInfo caster)
        {
            if (triggerHash != 0)
            {
                caster.owner.Animator.SetTrigger(triggerHash);
            }
            base.CastStarted(caster);
        }

        public override void CastEnded(CastInfo caster)
        {
            base.CastEnded(caster);
            caster.castPos = caster.owner.CastPos;
            caster.dir = caster.owner.Transform.Forward;
            Ability.Cast(caster);
        }
    }
}