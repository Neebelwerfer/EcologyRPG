
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
    CreateLineIndicator(Context, 4, 4, 1)
    Delay(1)
    local targets = GetTargetsInLine(Context, 4, 4)
    Log("Casting Ability")
    for i, t in ipairs(targets) do
        t.ApplyDamage(10, 2)
    end
    owner.RestoreMovement()
end