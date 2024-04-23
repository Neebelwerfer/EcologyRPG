using EcologyRPG.Core.Abilities;
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

        public bool InMinRange(float distance) => distance >= minRange;


        public CastContext CreateCastContext(Vector3 targetPoint)
        {
            return new CastContext(Owner, new Vector3Context(Owner.CastPos), new Vector3Context((targetPoint - Owner.Transform.Position).normalized), new Vector3Context(targetPoint));
        }

        public void Cast(Vector3 TargetPoint)
        {
            var castContext = CreateCastContext(TargetPoint);
            behaviour.Globals["Context"] = castContext;
            if (CanActivate())
            {
                Owner.Transform.LookAt(TargetPoint);
                HandleCast(castContext);
            }
        }

        public override void Init(BaseCharacter owner)
        {
            base.Init(owner);
        }
    }
}