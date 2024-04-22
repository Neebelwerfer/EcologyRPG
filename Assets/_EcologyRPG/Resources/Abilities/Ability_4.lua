
function CanActivate()
    local resource = Context.GetOwner().GetResource("stamina")
    return resource:HaveEnough(10)
end

function PayCost()
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
    local projectile = CreateBasicProjectile(0, Context, 10, 7, true)
    projectile.SetOnHit(function (target) 
        target.ApplyDamage(20, Water)
        local _context = CreateCastContext(target, projectile.GetPosition(), Context.dir)
        CastAbility(5, _context)
        end)
    projectile.Fire()
end
