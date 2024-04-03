using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using EcologyRPG.Utility;
using UnityEngine;

namespace EcologyRPG.GameSystems.NPC.Behaviours
{
    [CreateAssetMenu(fileName = "Basic Behaviour", menuName = "NPC/Behaviours/BasicBehaviour")]
    public class BasicBehaviour : NPCBehaviour
    {
        public NPCAbility attackAbilityReference;

        public float targetSearchCooldown = 0.5f;
        public float AggroRange;
        public float WanderRadius;
        public float MaxLeashRange;
        public LayerMask targetMask;

        BaseCharacter target;
        NPCAbility attackAbility;
        float targetSearchTimer = 0f;

        public override void Init(EnemyNPC character)
        {
            attackAbility = Instantiate(attackAbilityReference);
            attackAbility.Initialise();
            var aggroState = new State("Aggro");
            var passiveState = new State("Passive");

            #region Aggro State
            var aggrotree = new DecisionTree();
            aggroState.SetDecisionTree(aggrotree);

            aggroState.SetOnEnterAction((npc) =>
            {
            });

            var Chase = new ActionNode((npc) =>
            {
                var agent = npc.Agent;
                if (target == null) return;
                if (Vector3.Distance(npc.Transform.Position, target.Transform.Position) > MaxLeashRange)
                {
                    npc.behaviour.ChangeState(npc, passiveState);
                    return;
                }
                character.MoveTo(target.Transform.Position);
            });

            var Attack = new ActionNode((npc) =>
            {
                npc.Agent.ResetPath();
                if (attackAbility.state != AbilityStates.ready) return;
                npc.Transform.LookAt(target.Transform.Position);
                npc.CastAbility(attackAbility, target.Transform.Position);
            });

            var inAttackRange = new DecisionNode((npc) =>
            {
                var dist = Vector3.Distance(npc.Transform.Position, target.Transform.Position);
                return dist < attackAbility.Ability.Range;

            }, Attack, Chase);

            var stopChase = new ActionNode((npc) =>
            {
                npc.Agent.ResetPath();
                npc.behaviour.ChangeState(npc, passiveState);
            });

            var LeashRange = new DecisionNode((npc) =>
            {
                if(target.GameObject == null) return false;
                var distToSpawn = Vector3.Distance(npc.Transform.Position, npc.GetSpawner().transform.position);
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
            });

            var Wander = new ActionNode((npc) =>
            {
                var agent = npc.Agent;
                if (agent.remainingDistance > 0.1f) return;
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

                var col = Physics.OverlapSphere(npc.Transform.Position, AggroRange, targetMask);
                if (col.Length == 0) return false;

                foreach (var c in col)
                {
                    if (c.TryGetComponent<CharacterBinding>(out var characterBinding))
                    {
                        var baseCharacter = characterBinding.Character;
                        if (baseCharacter.Faction != Faction.player) continue;
                        if (Vector3.Distance(npc.GetSpawner().transform.position, c.transform.position) > MaxLeashRange) continue;
                        target = baseCharacter;
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
}