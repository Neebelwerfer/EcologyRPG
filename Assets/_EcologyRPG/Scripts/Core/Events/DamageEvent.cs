using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.Core.Events
{
    public class DamageEvent : EventData
    {
        public BaseCharacter target;
        public new BaseCharacter source;
        public Vector3 Point;
        public DamageType damageType;
        public float premitigationDamage;
        public float damageTaken;
    }

}
