using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;
using System;

namespace EcologyRPG.AbilityScripting
{
    public class BasicProjectileContext
    {
        readonly BasicProjectileBehaviour behaviour;

        public BasicProjectileContext(BasicProjectileBehaviour behaviour)
        {
            this.behaviour = behaviour;
        }

        public void SetOnHit(Closure onhitAction)
        {
            behaviour.OnHit = (BaseCharacter character) =>
            {
                onhitAction.Call(character);
            };
        }

        public void Fire()
        {
            behaviour.Fire();
        }

        public Vector3Context GetPosition()
        {
            return new Vector3Context(behaviour.GetPosition());
        }
    }
}