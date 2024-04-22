using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.AbilityScripting
{
    [CreateAssetMenu(menuName = "Abilities/Player Ability Reference")]
    public class PlayerAbilityReference : AbilityReference
    {
        public string Name;
        public string Description;
        public Sprite Icon;

        [StatAttribute(StatType.Resource)]
        public string ResourceName = "Stamina";
        public float ResourceCost;

        public bool useMouseDirection;

        public override void Init(BaseCharacter owner)
        {
            base.Init(owner);
        }

        public override bool CanActivate()
        {
            if (!base.CanActivate()) return false;
            if (Owner.Stats.GetResource(ResourceName).CurrentValue < ResourceCost) return false;
            return true;
        }

        public string GetDescription()
        {
            return $"{Name}\nCooldown: {Cooldown}\n{ResourceName}: {ResourceCost}\n{Description}";
        }

        public override CastContext CreateCastContext()
        {
            Vector3Context dir;
            var Mousepoint = TargetUtility.GetMousePoint(Camera.main);
            if (useMouseDirection)
            {
                var mouse = Mousepoint;
                mouse.y = Owner.Transform.Position.y;
                dir = new Vector3Context((mouse - Owner.Transform.Position).normalized);
            } 
            else
            {
                dir = new Vector3Context(Owner.Transform.Forward);
            }
            return new CastContext(Owner, new Vector3Context(Owner.CastPos), dir, new Vector3Context(Mousepoint));
        }

        void UseResource()
        {
            var resource = Owner.Stats.GetResource(ResourceName);
            resource.ModifyCurrentValue(-ResourceCost);
        }

        public override void HandleCast(CastContext castContext)
        {
            UseResource();
            base.HandleCast(castContext);
        }
    }
}