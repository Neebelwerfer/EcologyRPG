using EcologyRPG.Core;
using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.GameSystems.NPC
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject[] enemyPrefabs;
        public NPCConfig config;
        public float spawnRadius = 5;
        public int numberOfEnemies = 5;

        EnemyNPC[] Enemies;
        int currentEnemies;
        bool canSpawn = true;
        bool taskRunning = false;

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
            var point = transform.position + Random.insideUnitSphere * spawnRadius;
            point.y = 10000;
            Vector3 spawnPoint = Vector3.zero;
            while (spawnPoint == Vector3.zero)
            {
                if (Physics.BoxCast(point, Vector3.one * 0.5f, Vector3.down, out var hit, Quaternion.identity, 10000, Game.Settings.WalkableGroundMask))
                {
                    spawnPoint = hit.point;
                }
                else
                {
                    point = transform.position + Random.insideUnitSphere * spawnRadius;
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
                var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                var spawnPoint = SpawnablePoint();
                var enemyObj = EnemyManager.Instance.NPCPool.GetGameObject(prefab, spawnPoint, Quaternion.identity);
                var enemy = new EnemyNPC(config);
                enemy.Transform.Position = spawnPoint;
                enemy.SetBinding(enemyObj.GetComponent<CharacterBinding>());
                enemies[i] = enemy;
                enemies[i].SetSpawner(this);
                enemy.Agent.enabled = true;
                EnemyManager.Instance.AddCharacter(enemy, prefab);
            }
            currentEnemies += amount;
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

            if(canSpawn == false && !taskRunning)
            {
                TaskManager.Add(this,
                    () =>
                        {
                            canSpawn = true;
                            taskRunning = false;

                        },
                5 * 60);
                taskRunning = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (canSpawn)
                {
                    var counter = 0;
                    var enemies = SpawnEnemies(numberOfEnemies - currentEnemies);
                    for (int i = 0; i < Enemies.Length; i++)
                    {
                        if (Enemies[i] == null)
                        {
                            Enemies[i] = enemies[counter];
                            counter++;
                        }
                    }
                    canSpawn = false;
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
}

