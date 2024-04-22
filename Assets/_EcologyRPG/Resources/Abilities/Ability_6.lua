
function CanActivate()
    local resource = Context.GetOwner().GetResource("stamina")
    return resource:HaveEnough(10)
end

function UseResource()
    local resource = Context.GetOwner().GetResource("stamina")
    resource:Consume(10)
end

function OnCancelledCast()
    Log("Ability Cast Cancelled")
end

function OnCast()
    local owner = Context.GetOwner()
    owner.TriggerAnimation("Water_SingleShot")
    Delay(1)
    local projectile = CreateCurvedProjectile(0, Context, 2, 65)
    projectile.SetOnHit(function()
        local targets = GetTargetsInRadius(CreateCastContext(Context.GetOwner(), projectile.GetPosition(), Context.dir), 5)
        for i, t in ipairs(targets) do
            t.ApplyDamage(20, Physical)
        end
    end)
    
end
