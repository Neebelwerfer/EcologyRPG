using EcologyRPG.Core.Character;
using EcologyRPG.Core.Items;
using UnityEngine.AI;
using UnityEngine;
using EcologyRPG.Core.Abilities;
using log4net;

namespace EcologyRPG.GameSystems.NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyNPC : BaseCharacter
    {
        [SerializeField] NPCBehaviour behaviourReference;
        [SerializeField] float XpOnDeath = 10;

        public NavMeshAgent Agent { get; private set; }

        EnemySpawner spawner;
        [HideInInspector] public NPCBehaviour behaviour = null;
        readonly int movingHash;

        public EnemyNPC(NPCConfig config) : base()
        {
            behaviourReference = config.NPCBehaviour;
            level = (int)config.Level;
            XpOnDeath = config.xp;
            faction = Faction.enemy;
            Tags = config.tags;
            movingHash = Animator.StringToHash("Moving");
        }

        ~EnemyNPC()
        {
            if (behaviour != null)
            {
                Object.Destroy(behaviour);
            }
            spawner.RemoveEnemy(this);
        }

        public override void SetBinding(CharacterBinding binding)
        {
            base.SetBinding(binding);
            var npcBinding = binding as NPCBinding;
            Agent = npcBinding.Agent;
            Agent.speed = Stats.GetStat("movementSpeed").Value * Characters.BaseMoveSpeed;

            if (behaviourReference != null)
            {
                behaviour = Object.Instantiate(behaviourReference);
                behaviour.Init(this);
            }
        }

        public override void RemoveBinding()
        {
            Agent = null;
            base.RemoveBinding();
        }

        public void CastAbility(AbilityDefintion ability)
        {
            CastAbility(ability, Vector3.zero);
        }

        public void CastAbility(AbilityDefintion ability, Vector3 target)
        {
            ability.Activate(new CastInfo { activationInput = null, castPos = CastPos, owner = this, dir = target - transform.Position, targetPoint = target});
        }

        public override void Die()
        {
            base.Die();
            spawner.RemoveEnemy(this);
            LootGenerator.Instance.GenerateLootOnKill(this);
            EventManager.Defer("XP", new DefaultEventData { data = XpOnDeath, source = this });
            EventManager.Defer("EnemyDeath", new DefaultEventData { data = this, source = this });
        }

        public void SetSpawner(EnemySpawner spawner)
        {
            this.spawner = spawner;
        }

        public override void Update()
        {
            base.Update();
            if (IsPaused) return;

            if (behaviour != null && canMove && (state == CharacterStates.active))
            {
                Agent.isStopped = false;
                Agent.speed = Stats.GetStat("movementSpeed").Value * Characters.BaseMoveSpeed;
                behaviour.UpdateBehaviour(this);
                Animator.SetBool(movingHash, Agent.hasPath);

            }
        }

        public void MoveTo(Vector3 position)
        {
            Agent.SetDestination(position);
        }

        public EnemySpawner GetSpawner()
        {
            return spawner;
        }

        public override void StopMovement()
        {
            base.StopMovement();
            Agent.isStopped = true;
        }

        public void LateUpdate()
        {
            if (IsPaused) return;
            Transform.Position = Agent.transform.position;
            Transform.Rotation = Agent.transform.rotation;
        }
    }
}
