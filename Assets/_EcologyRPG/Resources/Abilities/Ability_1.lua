function CanActivate(CastInfo)
    local resource = Context.GetOwner().GetResource("stamina")
    return resource:HaveEnough(10)
end

function PayCost()
    local resource = Context.GetOwner().GetResource("stamina")
    resource:consume(10)
end
            
function OnCast()
    local owner = Context.GetOwner()
    owner.TriggerAnimation("Is_MeleeAttacking")
    owner.StopRotation()
    owner.SlowMovement()
    local indicator = CreateLineIndicator(Context, 2, 4)
    Delay(1)
    indicator.Destroy()
    local targets = GetTargetsInLine(Context, 2, 4)
    Log("Casting Ability")
    for i, t in ipairs(targets) do
        t.ApplyDamage(10, 2)
    end
    owner.RestoreMovement()
end