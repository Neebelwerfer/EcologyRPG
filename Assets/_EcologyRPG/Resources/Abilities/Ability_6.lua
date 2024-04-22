
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
