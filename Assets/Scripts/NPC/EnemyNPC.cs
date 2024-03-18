using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [HideInInspector]public NPCBehaviour behaviour = null;

    public override void Start()
    {
        base.Start();
        Agent = GetComponent<NavMeshAgent>();
        if(behaviourReference != null)
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
        EventManager.Defer("XP", new DefaultEventData { data = XpOnDeath, source = this }, DeferredEventType.Update);
        gameObject.SetActive(false);
        Destroy(gameObject, 1);
    }

    public void SetSpawner(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    public void UpdateBehaviour()
    {
        if (behaviour != null && (state != CharacterStates.disabled || state != CharacterStates.dead))
        {
            Agent.speed = Stats.GetStat("movementSpeed").Value;
            behaviour.UpdateBehaviour(this);
        }
    }

    public EnemySpawner GetSpawner()
    {
        return spawner;
    }
}
