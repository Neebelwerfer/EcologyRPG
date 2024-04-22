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
        public bool useMouseDirection;

        public override void Init(BaseCharacter owner)
        {
            base.Init(owner);
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
            var PayCost = behaviour.Globals.Get("PayCost");
            if(PayCost.IsNotNil()) behaviour.Call(PayCost);
            base.HandleCast(castContext);
        }
    }
}