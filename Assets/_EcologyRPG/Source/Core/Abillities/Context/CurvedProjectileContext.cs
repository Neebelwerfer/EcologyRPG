using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;

namespace EcologyRPG.AbilityScripting
{
    public class CurvedProjectileContext
    {
        public CurvedProjectileBehaviour behaviour;

        public CurvedProjectileContext(CurvedProjectileBehaviour behaviour)
        {
            this.behaviour = behaviour;
        }

        public Vector3Context GetPosition()
        {
            return new Vector3Context(behaviour.GetPosition());
        }

        public void Fire()
        {
            behaviour.Fire();
        }

        public void SetOnHit(Closure onhitAction)
        {
            behaviour.OnGroundHit = () =>
            {
                onhitAction.Call();
            };
        }

    }
}