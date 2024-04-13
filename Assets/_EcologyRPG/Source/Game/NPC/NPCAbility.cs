using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.GameSystems.NPC
{
    [CreateAssetMenu(menuName = "Ability/NPC Ability Data", fileName = "New NPC Ability Data")]
    public class NPCAbility : AbilityDefintion
    {
        [SerializeField] float minRange = 0;
        [Tooltip("The trigger to set in the animator when the ability is casted")]
        public string AnimationTrigger;
        public BaseAbility Ability;

        public int TriggerHash { get; protected set; }

        private void OnValidate()
        {
            if (Ability == null) return;
            if (minRange > Ability.Range)
            {
                minRange = Ability.Range;
            }
        }

        public bool InMinRange(float distance) => distance >= minRange;

        public override void Initialize(BaseCharacter owner, AbilityDefintion prefabAbility)
        {
            TriggerHash = Animator.StringToHash(AnimationTrigger);
            base.Initialize(owner, prefabAbility);
        }

        public override void CastStarted(CastInfo caster)
        {
            if (TriggerHash != 0)
            {
                caster.owner.Animator.SetTrigger(TriggerHash);
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