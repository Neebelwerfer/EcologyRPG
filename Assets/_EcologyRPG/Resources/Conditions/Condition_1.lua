function OnApply()
    SlowMod = StatModifier(Context, "movementSpeed", -SlowAmount, 300)
    Target.ApplyUniqueStatModifier(SlowMod, true)
end

function OnReapply()
    SetRemainingDuration(Duration)
end

function OnUpdate(deltaTime)

end

function OnRemoved()
    Target.RemoveUniqueStatModifier(SlowMod)
end
