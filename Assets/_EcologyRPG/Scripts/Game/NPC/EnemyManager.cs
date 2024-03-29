using EcologyRPG.Core.Character;
using EcologyRPG.Game.Player;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace EcologyRPG.Game.NPC
{
    class NPCData
    {
        public EnemyNPC enemy;
        public GameObject prefab;
    }

    public class EnemyManager
    {
        public static EnemyManager instance;

        public NPCGameObjectPool NPCPool;

        public float maxDistance = 200f;
        public float activeEnemyUpdateRate = 0.2f;

        List<NPCData> characterList = new();

        PlayerCharacter player;
        PlayerCharacter Player { get 
            {
                player ??= PlayerManager.Instance.GetPlayerCharacter();
                return player;
            }
        }

        float timer = 0;
        readonly List<NPCData> activeEnemies = new List<NPCData>();

        EnemyManager(float maxDistance, float activeEnemyUpdateRate)
        {
            this.maxDistance = maxDistance;
            this.activeEnemyUpdateRate = activeEnemyUpdateRate;
            EventManager.AddListener("EnemyDeath", OnEnemyDeath);
            player = PlayerManager.Instance.GetPlayerCharacter();
            NPCPool = new NPCGameObjectPool();
        }

        public static EnemyManager Init(float maxDistance, float activeEnemyUpdateRate)
        {
            instance = new EnemyManager(maxDistance, activeEnemyUpdateRate);
            return instance;

        }

        private void OnEnemyDeath(EventData arg0)
        {
            if(arg0 is DefaultEventData data)
            {
                if(data.data is EnemyNPC enemy)
                {
                    NPCPool.ReturnGameObject(enemy.GameObject);
                    RemoveCharacter(enemy);
                }
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
                if (Vector3.Distance(Player.Transform.Position, character.Transform.Position) < maxDistance)
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

        public void Update()
        {
            timer += Time.deltaTime;
            if(timer > activeEnemyUpdateRate)
            {
                UpdateActiveEnemies();
                timer = 0;
            }

            foreach (var character in activeEnemies)
            {
                character.enemy.Update();
            }
        }

        public void LateUpdate()
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
            var data = characterList.Find(x => x.enemy == character);
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
    }
}