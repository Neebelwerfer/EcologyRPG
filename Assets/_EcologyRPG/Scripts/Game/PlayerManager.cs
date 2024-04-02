using UnityEngine.Events;
using UnityEngine;
using EcologyRPG._Core.Character;
using Cinemachine;
using EcologyRPG._Core.Items;
using EcologyRPG._Core.Systems;
using System;
using Object = UnityEngine.Object;

namespace EcologyRPG._Game.Player
{
    public class PlayerManager : SystemBehavior, IUpdateSystem, IFixedUpdateSystem
    {
        public static PlayerManager Instance;

        public GameObject PlayerPrefab;
        public GameObject PlayerCameraPrefab;
        public PlayerSettings playerSettings;

        GameObject PlayerObject;
        readonly PlayerCharacter playerCharacter;
        readonly PlayerAbilities playerAbilities;
        readonly PlayerResourceManager playerResourceManager;
        readonly PlayerMovement playerMovement;

        readonly Inventory Inventory;

        GameObject PlayerCamera;
        CinemachineVirtualCamera playerCamera;

        public static PlayerCharacter PlayerCharacter => Instance.playerCharacter;
        public static PlayerAbilities PlayerAbilities => Instance.playerAbilities;
        public static Inventory PlayerInventory => Instance.Inventory;
        public static bool IsPlayerAlive => Instance.isPlayerSpawned;

        public bool Enabled { get => IsPlayerAlive; }

        bool isPlayerSpawned = false;

        PlayerManager(PlayerSettings playerSettings)
        {
            PlayerPrefab = playerSettings.PlayerModel;
            PlayerCameraPrefab = playerSettings.Camera;
            this.playerSettings = playerSettings;
            playerCharacter = new(playerSettings);
            Inventory = new(playerCharacter, playerSettings.StartingItems);
            playerAbilities = new(playerCharacter, Inventory, playerSettings);
            playerResourceManager = new(playerCharacter);
            playerMovement = new PlayerMovement(playerCharacter);
        }

        public static PlayerManager Init(PlayerSettings playerSettings)
        {
            Instance = new PlayerManager(playerSettings);
            return Instance;
        }

        void Spawn(Transform spawn)
        {
            PlayerObject = Object.Instantiate(PlayerPrefab, spawn.position, spawn.rotation);
            var binding = PlayerObject.GetComponent<CharacterBinding>();
            playerCharacter.SetBinding(binding);

            PlayerCamera = Object.Instantiate(PlayerCameraPrefab);
            playerCamera = PlayerCamera.GetComponent<CinemachineVirtualCamera>();
            playerCamera.Follow = PlayerObject.transform;
            playerCamera.LookAt = PlayerObject.transform;
            isPlayerSpawned = true;
            EventManager.Dispatch("PlayerSpawn", new DefaultEventData() { data = playerCharacter, source = this });
        }

        public void SpawnPlayer()
        {
            var spawn = Level.Instance.playerStartSpawnPoint;

            if (spawn != null)
            {
                Spawn(spawn);
            }
            else
            {
                Debug.LogError("No player spawn found in scene");
            }
        }

        public void RespawnPlayer()
        {
            var spawn = ClosestSpawn();
            if (spawn != null)
            {
                Spawn(spawn);
                playerCharacter.Respawn();
            }
            else
            {
                Debug.LogError("No player spawn found in scene");
            }
        }

        Transform ClosestSpawn()
        {
            Transform closest = null;
            float distance = float.MaxValue;
            foreach (var point in Level.Instance.respawnPoints)
            {
                var dist = Vector3.Distance(point.transform.position, playerCharacter.Transform.Position);
                if (dist < distance)
                {
                    distance = dist;
                    closest = point.transform;
                }
            }
            return closest;
        }

        public void PlayerDead()
        {
            isPlayerSpawned = false;
            playerCharacter.RemoveBinding();
            Object.Destroy(PlayerObject);
            EventManager.Defer("PlayerDeath", new DefaultEventData() { data = playerCharacter, source = this });
        }

        public void OnUpdate()
        {
            playerCharacter.Update();
            playerResourceManager.Update();
        }

        public void OnFixedUpdate()
        {
            playerMovement.FixedUpdate();
        }
    }
}
