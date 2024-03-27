using EcologyRPG.Core.Character;
using EcologyRPG.Core.Items;
using UnityEngine.AI;
using UnityEngine;

namespace EcologyRPG.Game.NPC
{
    public enum EnemyQuality
    {
        Normal,
        Elite,
        Boss
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyNPC : BaseCharacter
    {
        [SerializeField] NPCBehaviour behaviourReference;
        [SerializeField] float XpOnDeath = 10;

        public EnemyQuality Quality { get; private set; }
        public NavMeshAgent Agent { get; private set; }

        EnemySpawner spawner;
        [HideInInspector] public NPCBehaviour behaviour = null;

        public override void Start()
        {
            base.Start();
            Agent = GetComponent<NavMeshAgent>();
            if (behaviourReference != null)
            {
                behaviour = Instantiate(behaviourReference);
                behaviour.Init(this);
            }
            Agent.speed = Stats.GetStat("movementSpeed").Value;
        }

        public override void Die()
        {
            base.Die();
            spawner.RemoveEnemy(this);
            LootGenerator.Instance.GenerateLootOnKill(this);
            EventManager.Defer("XP", new DefaultEventData { data = XpOnDeath, source = this });
            gameObject.SetActive(false);
            Destroy(gameObject, 1);
        }

        public void SetSpawner(EnemySpawner spawner)
        {
            this.spawner = spawner;
        }

        public void UpdateBehaviour()
        {
            if (behaviour != null && canMove && (state != CharacterStates.disabled && state != CharacterStates.dead))
            {
                Agent.isStopped = false;
                Agent.speed = Stats.GetStat("movementSpeed").Value;
                behaviour.UpdateBehaviour(this);
            }
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
    }
}
