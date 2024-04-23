indicator = nil
function OnCastCancelled()
    if indicator != nil then
        indicator.Destroy()
    end
end
            
function OnCast()
    local owner = Context.GetOwner()
    owner.TriggerAnimation(Animation)
    owner.RotateTowards(Context.dir)
    owner.StopRotation()
    owner.SlowMovement()
    indicator = CreateLineIndicator(Context, Width, Range)
    Delay(1)
    indicator.Destroy()
    indicator = nil
    local targets = GetTargetsInLine(Context, Width, Range)
    for i, t in ipairs(targets) do
        t.ApplyDamage(BaseDamage, DamageType)
        SpawnVFXOnTarget(0, t, 2)
    end
    owner.RestoreMovement()
end