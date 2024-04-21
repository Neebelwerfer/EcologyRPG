using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.GameSystems.NPC
{
    [CreateAssetMenu(menuName = "Ability/NPC Ability Reference", fileName = "New NPC Ability Reference")]
    public class NPCAbilityReference : AbilityReference
    {
        public float Range = 5;
        [SerializeField] float minRange = 0;
        [Tooltip("The trigger to set in the animator when the ability is casted")]
        public string AnimationTrigger;

        public int TriggerHash { get; protected set; }

        public bool InMinRange(float distance) => distance >= minRange;


        public CastContext CreateCastContext(Vector3 targetPoint)
        {
            return new CastContext(Owner, new Vector3Context(Owner.CastPos), new Vector3Context(targetPoint - Owner.Transform.Position), new Vector3Context(targetPoint));
        }

        public void Cast(Vector3 TargetPoint)
        {
            var castContext = CreateCastContext(TargetPoint);
            behaviour.Globals["Context"] = castContext;
            if (CanActivate())
            {
                HandleCast(castContext);
            }
        }

        public override void Init(BaseCharacter owner)
        {
            TriggerHash = Animator.StringToHash(AnimationTrigger);
            base.Init(owner);
        }

        //public override void CastStarted(CastInfo caster)
        //{
        //    if (TriggerHash != 0)
        //    {
        //        caster.owner.Animator.SetTrigger(TriggerHash);
        //    }
        //    base.CastStarted(caster);
        //    caster.owner.Transform.LookAt(caster.targetPoint);
        //    Ability.Windup(caster, CastWindupTime);
        //}

        //public override void CastFinished(CastInfo caster)
        //{
        //    base.CastFinished(caster);
        //    caster.castPos = caster.owner.CastPos;
        //    caster.dir = caster.owner.Transform.Forward;
        //    Ability.Cast(caster);
        //}
    }
}