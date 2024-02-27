using Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Buff,
    Debuff
}

public abstract class CharacterEffect
{
    public string displayName;
    public EffectType type;
    public float duration;
    public float remainingDuration;

    public CharacterEffect(string name, float duration, EffectType type)
    {
        this.displayName = name;
        this.duration = duration;
        remainingDuration = duration;
        this.type = type;
    }

    public abstract void OnApply(BaseCharacter target);

    public abstract void OnUpdate(BaseCharacter target, float deltaTime);

    public abstract void OnRemoved(BaseCharacter target);
}
