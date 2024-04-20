using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;

namespace EcologyRPG.AbilityTest
{
    public class CastContext
    {
        BaseCharacter owner;
        Vector3Context castPos;
        Vector3Context dir;
        Vector3Context targetPoint;

        public CastContext(CastInfo castInfo)
        {
            owner = castInfo.owner;
            castPos = new Vector3Context(castInfo.castPos);
            dir = new Vector3Context(castInfo.dir);
            targetPoint = new Vector3Context(castInfo.targetPoint);
        }

        public BaseCharacter GetOwner() => owner;
        public Vector3Context GetCastPos() => castPos;
        public Vector3Context GetCastDir() => dir;
        public Vector3Context GetTargetPoint() => targetPoint;
    }
}