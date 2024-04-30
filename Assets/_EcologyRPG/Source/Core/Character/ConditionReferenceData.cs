using EcologyRPG.Core.Abilities;
using UnityEngine;

namespace EcologyRPG.Core.Character
{
    public class ConditionReferenceData : ScriptableObject
    {
        public int ID;
        public int ConditionBehaviourID;
        public bool useFixedUpdate;
        public float duration;
        public GlobalVariable[] variableOverrides;
    }
}