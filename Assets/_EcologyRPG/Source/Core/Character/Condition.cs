using EcologyRPG.Core.Abilities;
using EcologyRPG.Utility;
using System;
using UnityEngine;


namespace EcologyRPG.Core.Character
{
    public abstract class Condition : ScriptableObject
    {
        protected const string CharacterEffectPath = "Condition/";

        [ReadOnlyString]
        public string ID;
        [Min(0)] public float duration;
        [HideInInspector] public float remainingDuration;
        [HideInInspector] public BaseCharacter Owner;

        public Condition()
        {
            ID = Guid.NewGuid().ToString();
        }

        public abstract void OnApply(CastInfo Caster, BaseCharacter target);

        public abstract void OnReapply(BaseCharacter target);

        public abstract void OnUpdate(BaseCharacter target, float deltaTime);

        public abstract void OnRemoved(BaseCharacter target);

        protected static DamageInfo CalculateDamage(BaseCharacter Owner, DamageType type, float damage, bool allowVariance = false) => BaseAbility.CalculateDamage(Owner, type, damage, allowVariance);

        [ContextMenu("Delete")]
        protected virtual void Delete()
        {
            DestroyImmediate(this, true);
        }
    }

    public interface IUpdateCondition
    {
    }

    public interface IFixedUpdateCondition
    {
    }

#if UNITY_EDITOR
#endif
}