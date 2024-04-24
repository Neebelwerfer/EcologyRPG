
SlowMod = nil

function OnApply()
    SlowMod = StatModifier(Context, "movementSpeed", -SlowAmount, 300)
    local owner = Context.GetOwner()
    owner.ApplyUniqueStatModifier(SlowMod, true)
end

function OnReapply()

end

function OnUpdate()

end

function OnRemove()
    local owner = Context.GetOwner()
    owner.RemoveUniqueStatModifier(SlowMod)
end
