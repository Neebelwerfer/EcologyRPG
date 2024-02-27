using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCharacter : BaseCharacter
{
    EnemySpawner spawner;
    
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
}
