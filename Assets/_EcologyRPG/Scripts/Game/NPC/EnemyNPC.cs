using EcologyRPG.Core.Character;
using EcologyRPG.Core.Items;
using UnityEngine.AI;
using UnityEngine;

namespace EcologyRPG.Game.NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyNPC : BaseCharacter
    {
        [SerializeField] NPCBehaviour behaviourReference;
        [SerializeField] float XpOnDeath = 10;

        public NavMeshAgent Agent { get; private set; }

        EnemySpawner spawner;
        [HideInInspector] public NPCBehaviour behaviour = null;

        public EnemyNPC(NPCConfig config) : base()
        {
            behaviourReference = config.NPCBehaviour;
            level = (int)config.Level;
            XpOnDeath = config.xp;
            faction = Faction.enemy;
            Tags = config.tags;
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
            Agent.speed = Stats.GetStat("movementSpeed").Value;

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

            if (behaviour != null && canMove && (state != CharacterStates.disabled && state != CharacterStates.dead))
            {
                Agent.isStopped = false;
                Agent.speed = Stats.GetStat("movementSpeed").Value;
                behaviour.UpdateBehaviour(this);
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
