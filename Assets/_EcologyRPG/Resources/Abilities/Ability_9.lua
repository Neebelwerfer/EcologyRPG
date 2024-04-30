
function OnCast()
    local owner = Context.GetOwner()
    owner.ApplyCondition(Context, 2)
end
