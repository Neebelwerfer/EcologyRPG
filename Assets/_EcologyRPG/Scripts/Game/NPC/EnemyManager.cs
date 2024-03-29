using EcologyRPG.Core.Character;
using EcologyRPG.Game.Player;
using System;
using System.Collections.Generic;
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

    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance;
        public float maxDistance = 200f;

        [SerializeField] List<NPCData> characterList = new List<NPCData>();

        PlayerCharacter player;
        PlayerCharacter Player { get 
            {
                player ??= PlayerManager.Instance.GetPlayerCharacter();
                return player;
            }
        }

        readonly List<NPCData> activeEnemies = new List<NPCData>();

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
            EventManager.AddListener("EnemyDeath", OnEnemyDeath);
            InvokeRepeating(nameof(UpdateActiveEnemies), 0, 0.5f);
            player = PlayerManager.Instance.GetPlayerCharacter();
        }

        private void OnEnemyDeath(EventData arg0)
        {
            if(arg0 is DefaultEventData data)
            {
                if(data.data is EnemyNPC enemy)
                {
                    NPCGameObjectPool.Instance.ReturnGameObject(enemy.GameObject);
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
                            var obj = NPCGameObjectPool.Instance.GetGameObject(characterData.prefab);
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
                        NPCGameObjectPool.Instance.ReturnGameObject(character.GameObject);
                        character.RemoveBinding();
                    }
                }
            }
        }

        private void Update()
        {
            foreach (var character in activeEnemies)
            {
                character.enemy.Update();
            }
        }

        private void LateUpdate()
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