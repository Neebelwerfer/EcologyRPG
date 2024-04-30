using EcologyRPG.AbilityScripting;
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
        public List<NPCAbilityReference> abilities = new();

        public float targetSearchCooldown = 0.5f;
        public float AggroRange;
        public float WanderRadius;
        public float MaxLeashRange;

        public float wanderSpeedModifer = -0.5f;
        public float chaseSpeedModifier = 0.5f;
        public float ResetSpeedModifier = 1f;

        BaseCharacter target;
        NPCAbilityReference attackAbility;
        float targetSearchTimer = 0f;
        NPCAbilityReference[] initialisedAbilities;

        StatModification speedMod;

        public override void Init(EnemyNPC character)
        {
            speedMod = new StatModification("movementSpeed", wanderSpeedModifer, StatModType.PercentMult, this);
            character.Stats.AddStatModifier(speedMod);
            initialisedAbilities = new NPCAbilityReference[abilities.Count];
            for (int i = 0; i < abilities.Count; i++)
            {
                initialisedAbilities[i] = Instantiate(abilities[i]);
                initialisedAbilities[i].Init(character);
            }
            Debug.Log($"Initialised {initialisedAbilities.Length} abilities");

            var aggroState = new State("Aggro");
            var passiveState = new State("Passive");
            var resetState = new State("Reset");

            #region Aggro State
            var aggrotree = new DecisionTree();
            aggroState.SetDecisionTree(aggrotree);

            aggroState.SetOnEnterAction((npc) =>
            {
                speedMod.Value = chaseSpeedModifier;
            });

            var Chase = new ActionNode((npc) =>
            {
                var agent = npc.Agent;
                if (target == null) return;
                if (Vector3.Distance(npc.Transform.Position, target.Transform.Position) > MaxLeashRange)
                {
                    npc.behaviour.ChangeState(npc, resetState);
                    return;
                }
                character.MoveTo(target.Transform.Position);
            });

            var Attack = new ActionNode((npc) =>
            {
                npc.Agent.ResetPath();
                if (attackAbility.State != CastState.Ready) return;
                npc.Agent.velocity = Vector3.zero;
                CastAbility(npc, attackAbility, target.Transform.Position + (target.Velocity) * Time.deltaTime);
            });

            var inAttackRange = new DecisionNode((npc) =>
            {
                var dist = Vector3.Distance(npc.Transform.Position, target.Transform.Position);
                GetAbility(dist);
                var range = attackAbility.Range;
                return dist < (range < 10 ? range : range * 0.85f);

            }, Attack, Chase);

            var stopChase = new ActionNode((npc) =>
            {
                npc.Agent.ResetPath();
                npc.behaviour.ChangeState(npc, resetState);
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
                speedMod.Value = wanderSpeedModifer;
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

            #region Reset State
            var resetTree = new DecisionTree();
            resetState.SetDecisionTree(resetTree);

            resetState.SetOnEnterAction((npc) =>
            {
                target = null;
                npc.Agent.ResetPath();
                speedMod.Value = ResetSpeedModifier;
                var health = npc.Stats.GetResource("health");
                health.CurrentValue = health.MaxValue;
                npc.Invunerable = true;
                Debug.Log("Resetting");
            });

            var resetAction = new ActionNode((npc) =>
            {
                if (Vector3.Distance(npc.Transform.Position, npc.GetSpawner().transform.position) < 2f)
                {
                    npc.Invunerable = false;
                    npc.behaviour.ChangeState(npc, passiveState);
                }
                else if (!npc.Agent.hasPath)
                {
                    npc.MoveTo(npc.GetSpawner().transform.position);
                }
            });

            resetTree.SetRootNode(resetAction);

            #endregion

            currState = passiveState;
        }

        void GetAbility(float dist)
        {
            foreach (var a in initialisedAbilities)
            {
                if (a.State == CastState.Ready && a.InMinRange(dist))
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

        void CastAbility(EnemyNPC npc, NPCAbilityReference ability, Vector3 target)
        {
            npc.CastAbility(ability, target);
            foreach (var a in initialisedAbilities)
            {
                if (a == ability || a.State != CastState.Ready) continue;
                a.StartCooldown(1f);
            }
        }
    }
}