using EcologyRPG._Core.Abilities;
using EcologyRPG._Core.Character;
using UnityEngine;

namespace EcologyRPG._Core.Events
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
