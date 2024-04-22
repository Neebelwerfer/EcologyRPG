
function CanActivate()
end

function OnCancelledCast()
    Log("Ability Cast Cancelled")
end


function OnCast()
    local owner = Context.GetOwner()
    owner.TriggerAnimation(Animation)
    Delay(1)
    local projectile = CreateBasicProjectile(0, Context, Range, Speed, true)
    projectile.SetOnHit(function (target) 
        target.ApplyDamage(20, Water)
        local _context = CreateCastContext(target, projectile.GetPosition(), Context.dir)
        CastAbility(5, _context)
        end)
    projectile.Fire()
end
