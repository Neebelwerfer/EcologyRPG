using EcologyRPG.Core;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Systems;
using EcologyRPG.GameSystems.PlayerSystems;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.NPC
{
    class NPCData
    {
        public EnemyNPC enemy;
        public GameObject prefab;
    }

    public class EnemyManager : SystemBehavior, IUpdateSystem, IFixedUpdateSystem, ILateUpdateSystem
    {
        public static EnemyManager Instance;

        public NPCGameObjectPool NPCPool;

        public float maxDistance = 200f;
        public float activeEnemyUpdateRate = 0.2f;

        readonly List<NPCData> characterList = new();

        PlayerCharacter player;
        PlayerCharacter PlayerCharacter { get 
            {
                player ??= Player.PlayerCharacter;
                return player;
            }
        }

        public bool Enabled => true;

        readonly int AnimDieHash = Animator.StringToHash("Die");
        float timer = 0;
        readonly List<NPCData> activeEnemies = new();

        EnemyManager(float maxDistance, float activeEnemyUpdateRate) : base()
        {
            this.maxDistance = maxDistance;
            this.activeEnemyUpdateRate = activeEnemyUpdateRate;
            EventManager.AddListener("EnemyDeath", OnEnemyDeath);
            player = Player.PlayerCharacter;
            NPCPool = new NPCGameObjectPool();
        }

        public static void Init(float maxDistance, float activeEnemyUpdateRate)
        {
            if(Instance == null)
                Instance = new EnemyManager(maxDistance, activeEnemyUpdateRate);
        }

        private void OnEnemyDeath(EventData arg0)
        {
            if(arg0 is DefaultEventData data)
            {
                if(data.data is string GUID)
                {
                    var enemyData = characterList.Find(x => x.enemy.GUID == GUID);
                    if (enemyData == null)
                    {
                        Debug.LogWarning($"Enemy with GUID: {GUID} not found in EnemyManager");
                        return;
                    }
                    var enemy = enemyData.enemy;

                    enemy.Animator.SetTrigger(AnimDieHash);
                    enemy.Rigidbody.velocity = Vector3.zero;
                    enemy.Agent.velocity = Vector3.zero;
                    enemy.Agent.enabled = false;
                    RemoveCharacter(enemy);

                    enemy.Rigidbody.excludeLayers = Game.Settings.EntityMask;
                    var task = new RemoveEnemy() { GUID = GUID };
                    TaskManager.Add(this, task, 5);
                }
            }
        }

        struct RemoveEnemy : ITaskAction
        {
            public string GUID;
            public readonly void Execute()
            {
                if (Characters.Instance.GetCharacter(GUID) is not EnemyNPC enemy)
                    return;
                enemy.Rigidbody.excludeLayers = 0;
                Instance.NPCPool.ReturnGameObject(enemy.GameObject);
            }
        }

        void UpdateActiveEnemies()
        {
            if(characterList.Count == 0)
            {
                return;
            }

            foreach (var characterData in characterList)
            {
                var character = characterData.enemy;
                if (character.state != CharacterStates.dead && Vector3.Distance(PlayerCharacter.Transform.Position, character.Transform.Position) < maxDistance)
                {
                    if (!activeEnemies.Contains(characterData))
                    {
                        activeEnemies.Add(characterData);   
                        if(character.GameObject == null)
                        {
                            var obj = NPCPool.GetGameObject(characterData.prefab);
                            obj.transform.SetPositionAndRotation(character.Transform.Position, character.Transform.Rotation);
                            character.SetBinding(obj.GetComponent<CharacterBinding>());
                        }
                    }
                }
                else
                {
                    if (activeEnemies.Contains(characterData))
                    {
                        activeEnemies.Remove(characterData);
                        NPCPool.ReturnGameObject(character.GameObject);
                        character.RemoveBinding();
                    }
                }
            }
        }

        public EnemyNPC GetNPC(string guid)
        {
            var character = characterList.Find(x => x.enemy.GUID == guid);
            if (character == null)
            {
                Debug.LogWarning($"Character with GUID: {guid} not found");
                return null;
            }
            return character.enemy;
        }

        public void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer > activeEnemyUpdateRate)
            {
                UpdateActiveEnemies();
                timer = 0;
            }

            foreach (var character in activeEnemies)
            {
                character.enemy.Update();
            }
        }

        public void OnFixedUpdate()
        {
            foreach (var character in activeEnemies)
            {
                character.enemy.FixedUpdate();
            }
        }

        public void OnLateUpdate()
        {
            foreach (var character in activeEnemies)
            {
                character.enemy.LateUpdate();
            }
        }

        public void AddCharacter(EnemyNPC character, GameObject prefab)
        {
            var data = new NPCData { enemy = character, prefab = prefab };
            characterList.Add(data);
            activeEnemies.Add(data);
        }

        public void RemoveCharacter(EnemyNPC character)
        {
            var data = characterList.Find(x => x.enemy.GUID == character.GUID);
            characterList.Remove(data);
            if (activeEnemies.Contains(data))
            {
                activeEnemies.Remove(data);
            }
        }

        public void AddCharacters(EnemyNPC[] characters, GameObject prefab)
        {
            foreach (var character in characters)
            {
                AddCharacter(character, prefab);
            }
        }

        public void RemoveCharacters(EnemyNPC[] characters)
        {
            foreach (var character in characters)
            {
                RemoveCharacter(character);
            }
        }

        override public void Dispose()
        {
            base.Dispose();
            NPCPool.Dispose();
            characterList.Clear();
            EventManager.RemoveListener("EnemyDeath", OnEnemyDeath);
            Instance = null;
        }
    }
}