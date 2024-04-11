using EcologyRPG.Core.Abilities;
using UnityEngine;

namespace EcologyRPG.GameSystems.NPC
{
    [CreateAssetMenu(menuName = "Ability/NPC Ability Data", fileName = "New NPC Ability Data")]
    public class NPCAbility : AbilityDefintion
    {
        [SerializeField] float minRange;
        [Tooltip("The trigger to set in the animator when the ability is casted")]
        public string AnimationTrigger;
        public BaseAbility Ability;

        int triggerHash;

        private void OnValidate()
        {
            if (minRange > Ability.Range)
            {
                minRange = Ability.Range;
            }
        }

        public bool InMinRange(float distance) => distance >= minRange;

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
            caster.owner.Transform.LookAt(caster.targetPoint);
        }

        public override void CastFinished(CastInfo caster)
        {
            base.CastFinished(caster);
            caster.castPos = caster.owner.CastPos;
            caster.dir = caster.owner.Transform.Forward;
            Ability.Cast(caster);
        }
    }
}