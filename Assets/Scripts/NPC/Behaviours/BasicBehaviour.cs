using Character;
using UnityEngine;
using Character.Abilities;
using Utility;

[CreateAssetMenu(fileName = "Basic Behaviour", menuName = "NPC/Behaviours/BasicBehaviour")]
public class BasicBehaviour : NPCBehaviour
{
    public AttackAbility attackAbilityReference;

    public float targetSearchCooldown = 0.5f;
    public float AggroRange;
    public float WanderRadius;
    public float MaxLeashRange;
    public LayerMask targetMask;

    BaseCharacter target;
    AttackAbility attackAbility;
    float targetSearchTimer = 0f;
    
    public override void Init(EnemyNPC character)
    {
        attackAbility = Instantiate(attackAbilityReference);
        var aggroState = new State("Aggro");
        var passiveState = new State("Passive");

        #region Aggro State
        var aggrotree = new DecisionTree();
        aggroState.SetDecisionTree(aggrotree);

        aggroState.SetOnEnterAction((npc) =>
        {
            Debug.Log(npc.name + " has found target");
        });

        var Chase = new ActionNode((npc) =>
        {
            var agent = npc.Agent;
            if (target == null) return;
            if (Vector3.Distance(npc.transform.position, target.transform.position) > MaxLeashRange)
            {
                npc.behaviour.ChangeState(npc, passiveState);
                return;
            }
            agent.SetDestination(target.transform.position);
        });

        var Attack = new ActionNode((npc) =>
        {
            if (attackAbility.state != AbilityStates.ready) return;
            npc.Agent.ResetPath();
            npc.transform.LookAt(target.transform);
            attackAbility.Activate(new CasterInfo { activationInput = null, castPos = npc.AbilityPoint.transform.position, owner = npc});
        });

        var inAttackRange = new DecisionNode((npc) =>
        {
            var dist = Vector3.Distance(npc.transform.position, target.transform.position);
            return dist < attackAbility.attackRange;

        }, Attack, Chase);

        var stopChase = new ActionNode((npc) =>
        {
            npc.Agent.ResetPath();
            npc.behaviour.ChangeState(npc, passiveState);
        });

        var LeashRange = new DecisionNode((npc) =>
        {
            var distToSpawn = Vector3.Distance(npc.transform.position, npc.GetSpawner().transform.position);
            return !(distToSpawn > MaxLeashRange);
        }, inAttackRange, stopChase);

        aggrotree.SetRootNode(LeashRange);
        #endregion

        #region Passive State
        var passiveTree = new DecisionTree();
        passiveState.SetDecisionTree(passiveTree);

        passiveState.SetOnEnterAction((npc) =>
        {
            target = null;
            npc.Agent.ResetPath();
            Debug.Log(npc.name + " has lost target");
        });

        var Wander = new ActionNode((npc) =>
        {
            var agent = npc.Agent;
            if (agent.remainingDistance > 0.1f) return;
            Debug.Log("Wandering");
            var wanderPos = NPCUtility.GetRandomPointInRadius(npc.GetSpawner().transform.position, WanderRadius);
            agent.SetDestination(wanderPos);
        });


        var setAggro = new ActionNode((npc) =>
        {
            npc.behaviour.ChangeState(npc, aggroState);
        });

        var NearbyTarget = new DecisionNode((npc) =>
        {
            targetSearchTimer += TimeManager.IngameDeltaTime;
            if (targetSearchTimer < targetSearchCooldown) return false;
            targetSearchTimer = 0;

            var col = Physics.OverlapSphere(npc.transform.position, AggroRange, targetMask);
            if (col.Length == 0) return false;

            foreach (var c in col)
            {
                if (c.TryGetComponent<BaseCharacter>(out var baseCharacter))
                {
                    if (baseCharacter.Faction != Faction.player) continue;
                    target = c.GetComponent<BaseCharacter>();
                    return true;
                }
            }
            return false;
        }, setAggro, Wander);
        passiveTree.SetRootNode(NearbyTarget);
        #endregion

        ChangeState(character, passiveState);
    }
}