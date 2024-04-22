
function CanActivate()
    local resource = Context.GetOwner().GetResource("stamina")
    return resource:HaveEnough(10)
end

function UseResource()
    local resource = Context.GetOwner().GetResource("stamina")
    resource:Consume(10)
end

function OnCastCancelled()

end

function OnCast()
    local projectile = CreateBasicProjectile(0, Context, 10, 7, false)
    projectile.SetOnHit(function (target) 
        target.ApplyDamage(20, Water)
        end)
    projectile.Fire()
end
