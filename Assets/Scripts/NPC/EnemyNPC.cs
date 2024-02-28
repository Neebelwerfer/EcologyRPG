using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNPC : BaseCharacter
{
    public NPCBehaviour behaviour;
    public NavMeshAgent Agent { get; private set; }
    EnemySpawner spawner;

    public override void Start()
    {
        base.Start();
        Agent = GetComponent<NavMeshAgent>();
        if(behaviour != null)
        {
            behaviour.Init(this);
        }
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
        if(behaviour != null)
        {
            behaviour.UpdateBehaviour(this);
        }
    }

    public EnemySpawner GetSpawner()
    {
        return spawner;
    }
}
