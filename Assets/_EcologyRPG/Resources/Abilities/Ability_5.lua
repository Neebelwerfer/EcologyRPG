function OnCast()
    local projectile = CreateBasicProjectile(0, Context, 10, 7, false)
    projectile.SetOnHit(function (target) 
        target.ApplyDamage(20, Water)
        end)
    projectile.Fire()
end
