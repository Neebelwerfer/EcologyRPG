using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.AbilityScripting
{
    public class PlayerAbilityReference : AbilityReference
    {
        public bool useMouseDirection;
        

        public PlayerAbilityReference(AbilityData abilityData, BaseCharacter owner) : base(abilityData, owner)
        {

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
            behaviour.Call(PayCost);
            base.HandleCast(castContext);
        }
    }
}