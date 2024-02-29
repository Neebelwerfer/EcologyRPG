using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRadius = 5;
    public int numberOfEnemies = 5;
    public LayerMask GroundMask;

    EnemyNPC[] Enemies;
    int currentEnemies;

    Collider col;
    void Start()
    {
        Enemies = new EnemyNPC[numberOfEnemies];
        currentEnemies = 0;
        col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    Vector3 SpawnablePoint()
    {
        var point = transform.position + UnityEngine.Random.insideUnitSphere * spawnRadius;
        point.y = 10000;
        Vector3 spawnPoint = Vector3.zero;
        while (spawnPoint == Vector3.zero)
        {
            if (Physics.BoxCast(point, Vector3.one * 0.5f, Vector3.down, out var hit, Quaternion.identity, 10000, GroundMask))
            {
                spawnPoint = hit.point;
                spawnPoint.y += 0.5f;
            }
            else
            {
                point = transform.position + UnityEngine.Random.insideUnitSphere * spawnRadius;
                point.y = 10000;
            }
        }
        return spawnPoint;
    }

    public EnemyNPC[] SpawnEnemies(int amount)
    {
        EnemyNPC[] enemies = new EnemyNPC[amount];
        for (int i = 0; i < amount; i++)
        {
            var spawnPoint = SpawnablePoint();
            var enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            enemies[i] = enemy.GetComponent<EnemyNPC>();
            enemies[i].SetSpawner(this);
        }
        currentEnemies += amount;
        EnemyManager.instance.AddCharacters(enemies);
        return enemies;
    }

    public void RemoveEnemy(EnemyNPC enemy)
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] == enemy)
            {
                Enemies[i] = null;
                currentEnemies--;
                break;
            }
        }
        EnemyManager.instance.RemoveCharacter(enemy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(currentEnemies != numberOfEnemies)
            {
                var counter = 0;
                var enemies = SpawnEnemies(numberOfEnemies - currentEnemies);
                for(int i = 0; i < Enemies.Length; i++)
                {
                    if (Enemies[i] == null)
                    {
                        Enemies[i] = enemies[counter];
                        counter++;
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
#endif
}
