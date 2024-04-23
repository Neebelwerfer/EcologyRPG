      
function OnCast()
    local owner = Context.GetOwner()
    owner.TriggerAnimation(Animation)
    owner.RotateTowards(Context.dir)
    owner.StopRotation()
    owner.SlowMovement()
    CreateLineIndicator(Context, Width, Range, Windup)
    Delay(Windup)
    local targets = GetTargetsInLine(Context, Width, Range)
    for i, t in ipairs(targets) do
        t.ApplyDamage(BaseDamage, DamageType)
    end
    owner.RestoreMovement()
end