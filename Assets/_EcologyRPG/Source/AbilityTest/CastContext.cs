using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;

namespace EcologyRPG.AbilityScripting
{
    public class CastContext
    {
        readonly BaseCharacter owner;
        public Vector3Context castPos;
        public Vector3Context dir;
        public Vector3Context targetPoint;

        public BaseCharacter GetOwner() => owner;

        public CastContext(BaseCharacter baseCharacter, Vector3Context castPos, Vector3Context dir)
        {
            owner = baseCharacter;
            this.castPos = castPos;
            this.dir = dir;
        }

        public CastContext(BaseCharacter baseCharacter, Vector3Context castPos, Vector3Context dir, Vector3Context targetPoint)
        {
            owner = baseCharacter;
            this.castPos = castPos;
            this.dir = dir;
            this.targetPoint = targetPoint;
        }

    }
}