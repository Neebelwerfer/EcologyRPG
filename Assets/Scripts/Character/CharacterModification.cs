using Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterModificationType
{
    Buff,
    Debuff
}

public abstract class CharacterModification
{
    public string displayName;
    public CharacterModificationType type;
    public float duration;
    public float remainingDuration;

    public CharacterModification(string name, float duration, CharacterModificationType type)
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
