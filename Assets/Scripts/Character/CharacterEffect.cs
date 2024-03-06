using Character;
using Character.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Buff,
    Debuff
}

public abstract class CharacterEffect : ScriptableObject
{
    public string displayName;
    public EffectType type;
    [Min(0)] public float duration;
    [HideInInspector] public float remainingDuration;

    private void OnValidate()
    {
        if (remainingDuration != duration)
        {
            remainingDuration = duration;
        }
    }

    public abstract void OnApply(CasterInfo Caster, BaseCharacter target);

    public abstract void OnUpdate(BaseCharacter target, float deltaTime);

    public abstract void OnRemoved(BaseCharacter target);
}
