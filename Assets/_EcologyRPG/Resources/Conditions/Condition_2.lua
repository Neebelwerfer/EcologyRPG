sprint = nil

function OnApply()
    sprint = StatModifier(Context, "movementSpeed", modifier, 300)
    Target.ApplyUniqueStatModifier(sprint, false)
end

function OnReapply()
    SetRemainingDuration(Duration)
end

function OnUpdate(deltaTime)

end

function OnRemoved()
    Target.RemoveUniqueStatModifier(sprint)
end
