using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNPC : BaseCharacter
{
    [SerializeField] NPCBehaviour behaviourReference;
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
        Agent.speed = stats.GetStat("movementSpeed").Value;
        stats.GetStat("movementSpeed").OnStatChanged.AddListener((value) => Agent.speed = value);
    }
    
    public override void Die()
    {
        base.Die();
        spawner.RemoveEnemy(this);
        Destroy(gameObject);
    }

    public void SetSpawner(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    public void UpdateBehaviour()
    {
        if (behaviour != null && (state != CharacterStates.disabled || state != CharacterStates.dead))
        {
            behaviour.UpdateBehaviour(this);
        }
    }

    public EnemySpawner GetSpawner()
    {
        return spawner;
    }
}
