using UnityEngine;

namespace EcologyRPG._Core.Character
{
    public enum StatType
    {
        Stat,
        Attribute,
        Resource,
    }
    public class StatAttribute : PropertyAttribute
    {
        public StatType StatType;
        public StatAttribute(StatType statType)
        {
            StatType = statType;
        }
    }
}