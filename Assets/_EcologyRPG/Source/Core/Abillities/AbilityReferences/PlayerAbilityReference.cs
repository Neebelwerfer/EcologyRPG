using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Scripting;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    [CreateAssetMenu(menuName = "Ability/Player Ability Reference")]
    public class PlayerAbilityReference : AbilityReference
    {
        public string Name;
        public string Description;
        public Sprite Icon;
        public bool useMouseDirection;

        [StatAttribute(StatType.Resource)]
        public string ResourceName = "stamina";
        public float ResourceCost = 0;
        public bool customResourceUsage = false;
        [TextArea(3, 10)]
        public string UseResourceString = 
@"function UseResource()
    local Owner = Context.GetOwner()
    local Resource = Owner.GetResource(ResourceName)
    Resource.Consume(ResourceCost)
end
";

        public override void Init(BaseCharacter owner)
        {
            base.Init(owner);
            behaviour.Globals["ResourceCost"] = ResourceCost;
            behaviour.Globals["ResourceName"] = ResourceName;
        }

        public string GetDescription()
        {
            return $"{Name}\nCooldown: {Cooldown}\n{Description}";
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

        public override void HandleCast(CastContext castContext)
        {
            if (customResourceUsage)
            {
                var UseResource = behaviour.Globals.Get("UseResource");
                if (UseResource.IsNotNil()) behaviour.Call(UseResource);
            }
            else
            {
                var resource = Owner.Stats.GetResource(ResourceName);
                resource.ModifyCurrentValue(-ResourceCost);
            }

            base.HandleCast(castContext);
        }
    }
}