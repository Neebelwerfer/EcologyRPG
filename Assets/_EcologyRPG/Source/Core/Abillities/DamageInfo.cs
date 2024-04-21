namespace EcologyRPG.Core.Abilities
{
    public enum DamageType
    {
        Physical,
        Water,
        Toxic
    }

    public struct DamageInfo
    {
        public DamageType type;
        public float damage;
        public object source;
    }
}