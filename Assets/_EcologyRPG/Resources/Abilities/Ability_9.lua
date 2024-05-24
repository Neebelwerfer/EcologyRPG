
function OnCast()
    local owner = Context.GetOwner()
    owner.ApplyCondition(Context, Sprint)
end
