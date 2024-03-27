using Character.Abilities;
using UnityEngine;

public class AttackAbilityDefinition : AbilityDefintion
{
    public bool BlockMovementOnWindup = false;
    public bool RotatePlayerTowardsMouse = true;
    public bool BlockRotationOnWindup = true;
    public BaseAbility Ability;

    Vector3 MousePoint;

    public override void CastStarted(CastInfo caster)
    {
        base.CastStarted(caster);
        if(BlockRotationOnWindup) caster.owner.StopRotation();
        if(BlockMovementOnWindup) caster.owner.StopMovement();
        var res = TargetUtility.GetMousePoint(Camera.main);
        MousePoint = res;
        if(!RotatePlayerTowardsMouse) return;
        res.y = caster.owner.Transform.position.y;
        caster.owner.Transform.LookAt(res);

    }

    public override void CastEnded(CastInfo caster)
    {
        base.CastEnded(caster);
        caster.mousePoint = MousePoint;
        if(caster.castPos == Vector3.zero) caster.castPos = caster.owner.CastPos;
        Ability.Cast(caster);
        if(BlockMovementOnWindup) caster.owner.StartMovement();
        if(BlockRotationOnWindup) caster.owner.StartRotation();
    }

    [ContextMenu("Delete")]
    protected override void Delete()
    {
        base.Delete();
        DestroyImmediate(Ability, true);
    }
}