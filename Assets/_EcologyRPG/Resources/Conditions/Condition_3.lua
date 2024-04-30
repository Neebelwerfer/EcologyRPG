function OnCollision(hit)
    Log("Test")
    hit.ApplyDamage(5, 2)
    if StopOnHit then
        SetRemainingDuration(0)
    end
end

function OnApply()
    dir = Context.dir
    hitCharacters = {}
    length = 0
    timer = 0
    Target.StopMovement()
    Target.IgnoreCollision()
end

function OnReapply()
    SetRemainingDuration(Duration)
end

function OnUpdate(deltaTime)
    if Target.IsLegalMove(dir, Strength * deltaTime) then
        local velocity = dir.Multiply(Strength)
        Target.SetVelocity(velocity)
    else 
        Target.SetVelocity(Vector3.Zero)
    end

    if(timer <= 0) then
        timer = 0.1
        local hits = Physics.OverlapSphere(Context, Target.GetPosition(), 1)
        if hits != nil then
            IterateHits(hits)
        end
    else 
        timer = timer - deltaTime
    end
end

function IterateHits(hits)
    for i, t in ipairs(hits) do
        if not HasHit(t) then
            OnCollision(t)
        end
    end
end

function HasHit(target)
    if length == 0  then
        length = length + 1
        hitCharacters[length] = target
        return false
    else
        Log("Checking old targets")
        for i = 1, length, 1 do
            if target.Compare(hitCharacters[i]) then
                return true
            end
        end

        hitCharacters[length + 1] = target
        length = length + 1
    end
    return false
end

function OnRemoved()
    Target.SetVelocity(Vector3.Zero)
    Target.RestoreMovement()
    Target.RestoreCollision()
end
