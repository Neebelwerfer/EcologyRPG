using Character;
using Character.Abilities;
using UnityEditor;
using UnityEngine.AI;

public class Stun : DebuffCondition
{
    public override void OnApply(CastInfo Caster, BaseCharacter target)
    {
        target.state = CharacterStates.disabled;
        if(target.TryGetComponent(out NavMeshAgent agent))
        {
            agent.isStopped = true;
        }
    }

    public override void OnReapply(BaseCharacter target)
    {
        remainingDuration = duration;
    }

    public override void OnRemoved(BaseCharacter target)
    {
        target.state = CharacterStates.active;
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Stun))]
public class StunEditor : ConditionEditor
{

}
#endif