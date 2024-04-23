using EcologyRPG.AbilityScripting;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Scripting;
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
            Owner.Transform.LookAt(TargetPoint);
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
    }
}