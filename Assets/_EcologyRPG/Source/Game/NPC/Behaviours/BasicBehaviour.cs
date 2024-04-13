using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using EcologyRPG.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.NPC.Behaviours
{
    [CreateAssetMenu(fileName = "Basic Behaviour", menuName = "NPC/Behaviours/BasicBehaviour")]
    public class BasicBehaviour : NPCBehaviour
    {
        public List<NPCAbility> abilities = new();

        public float targetSearchCooldown = 0.5f;
        public float AggroRange;
        public float WanderRadius;
        public float MaxLeashRange;

        BaseCharacter target;
        NPCAbility attackAbility;
        float targetSearchTimer = 0f;
        NPCAbility[] initialisedAbilities;

        public override void Init(EnemyNPC character)
        {
            initialisedAbilities = new NPCAbility[abilities.Count];
            for (int i = 0; i < abilities.Count; i++)
            {
                initialisedAbilities[i] = Instantiate(abilities[i]);
                initialisedAbilities[i].Initialize(character, abilities[i]);
            }

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
                npc.Agent.velocity = Vector3.zero;
                CastAbility(npc, attackAbility, target.Transform.Position + (target.Velocity * attackAbility.CastWindupTime));
            });

            var inAttackRange = new DecisionNode((npc) =>
            {
                var dist = Vector3.Distance(npc.Transform.Position, target.Transform.Position);
                GetAbility(dist);
                return dist < attackAbility.Ability.Range * 0.75;

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

                var col = Physics.OverlapSphere(npc.Transform.Position, AggroRange, Game.Settings.EntityMask);
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

            currState = passiveState;
        }

        void GetAbility(float dist)
        {
            foreach (var a in initialisedAbilities)
            {
                if (a.state == AbilityStates.ready && a.InMinRange(dist))
                {
                    attackAbility = a;
                    return;
                }
            }
            if (attackAbility == null)
            {
                attackAbility = initialisedAbilities[0];
            }
        }

        void CastAbility(EnemyNPC npc, NPCAbility ability, Vector3 target)
        {
            npc.CastAbility(ability, target);
            foreach (var a in initialisedAbilities)
            {
                if (a == ability || a.state != AbilityStates.ready) continue;
                a.PutOnCooldown(1f);
            }
        }
    }
}