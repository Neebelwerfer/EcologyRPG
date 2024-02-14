using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    public string displayName;
    public float duration;
    public float remainingDuration;

    public Buff(string name, float duration)
    {
        this.displayName = name;
        this.duration = duration;
        remainingDuration = duration;
    }

    public abstract void OnApply(BaseCharacter target);

    public abstract void OnUpdate(BaseCharacter target, float deltaTime);

    public abstract void OnRemoved(BaseCharacter target);
}
