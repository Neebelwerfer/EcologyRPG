using Character.Abilities;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityHolder/AttackAbility", fileName = "New Attack Ability")]
public class AttackAbility : BaseAbility
{
    public AbilityEffect Ability;

    Vector3 MousePoint;

    public override void CastStarted(CastInfo caster)
    {
        base.CastStarted(caster);
        MousePoint = TargetUtility.GetMousePoint(Camera.main);
    }

    public override void CastEnded(CastInfo caster)
    {
        base.CastEnded(caster);
        caster.mousePoint = MousePoint;
        if(caster.castPos == Vector3.zero) caster.castPos = caster.owner.CastPos;
        Ability.Cast(caster);
    }
}

#if UNITY_EDITOR
#endif