
function OnApply()
timer = 0
BaseDamagePerTick = Basedamage
TickRate = 1 / Frequency
end

function OnReapply()
    local remainingTicks = RemainingDuration / TickRate
    local RolloverDamage = (remainingTicks * BaseDamagePerTick) * (TickRate / Duration)
    BaseDamagePerTick = BaseDamagePerTick + RolloverDamage
    SetRemainingDuration(Duration)
end

function OnUpdate(deltaTime)
    timer = timer - deltaTime
    if(timer <= 0) then
        local damage = CalculateDamage(Context.GetOwner(), BaseDamagePerTick)
        Target.ApplyDamage(damage, DamageType)
        timer = TickRate
    end
end

function OnRemoved()

end
