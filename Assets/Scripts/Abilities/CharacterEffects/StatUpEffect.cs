using Character;
using Character.Abilities;
using UnityEngine;

[CreateAssetMenu(fileName = "StatUpEffect", menuName = BuffPath + "Stat Up Effect")]
public class StatUpEffect : BuffEffect
{

    public string StatName;
    public StatModType ModType;
    public float Value;

    static UniqueStatModificationHandler UniqueStatModHandler;

    public override void OnApply(CastInfo Caster, BaseCharacter target)
    {
        UniqueStatModHandler = new UniqueStatModificationHandler(StatName, ModType, true);
        UniqueStatModHandler.AddValue(target, this, Value);
    }

    public override void OnReapply(BaseCharacter target)
    {

    }

    public override void OnRemoved(BaseCharacter target)
    {
        UniqueStatModHandler.RemoveValue(target, this);
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {

    }
}