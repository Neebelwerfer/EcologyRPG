using Character.Abilities;
using UnityEngine;

public class AttackAbilityDefinition : AbilityDefintion
{
    public bool BlockMovementOnWindup = false;
    public BaseAbility Ability;

    Vector3 MousePoint;

    public override void CastStarted(CastInfo caster)
    {
        base.CastStarted(caster);
        if(BlockMovementOnWindup) caster.owner.StopMovement();
        MousePoint = TargetUtility.GetMousePoint(Camera.main);
    }

    public override void CastEnded(CastInfo caster)
    {
        base.CastEnded(caster);
        caster.mousePoint = MousePoint;
        if(caster.castPos == Vector3.zero) caster.castPos = caster.owner.CastPos;
        Ability.Cast(caster);
        if(BlockMovementOnWindup) caster.owner.StartMovement();
    }

    [ContextMenu("Delete")]
    protected override void Delete()
    {
        base.Delete();
        DestroyImmediate(Ability, true);
    }
}