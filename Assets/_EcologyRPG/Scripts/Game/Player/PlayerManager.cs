using UnityEngine.Events;
using UnityEngine;
using EcologyRPG.Core.Character;
using Cinemachine;
using EcologyRPG.Core.Items;

namespace EcologyRPG.Game.Player
{
    public class PlayerManager
    {
        public static PlayerManager Instance;

        public GameObject PlayerPrefab;
        public GameObject PlayerCameraPrefab;
        public PlayerSettings playerSettings;

        GameObject PlayerObject;
        readonly PlayerCharacter playerCharacter;
        readonly PlayerAbilities playerAbilities;
        readonly Inventory Inventory;

        GameObject PlayerCamera;
        CinemachineVirtualCamera playerCamera;

        public static PlayerCharacter Player => Instance.playerCharacter;
        public static PlayerAbilities PlayerAbilities => Instance.playerAbilities;
        public static Inventory PlayerInventory => Instance.Inventory;
        public static bool IsPlayerAlive => Instance.PlayerObject != null;


        PlayerManager(PlayerSettings playerSettings)
        {
            PlayerPrefab = playerSettings.PlayerModel;
            PlayerCameraPrefab = playerSettings.Camera;
            this.playerSettings = playerSettings;
            playerCharacter = new(playerSettings);
            Inventory = new(playerCharacter, playerSettings.StartingItems);
            playerAbilities = new(playerCharacter, Inventory, playerSettings);
        }

        public static PlayerManager Init(PlayerSettings playerSettings)
        {
            Instance = new PlayerManager(playerSettings);
            return Instance;
        }

        public void SpawnPlayer()
        {
            var spawn = GameObject.FindGameObjectWithTag("PlayerSpawn");

            if (spawn != null)
            {
                PlayerObject = Object.Instantiate(PlayerPrefab, spawn.transform.position, spawn.transform.rotation);
                var binding = PlayerObject.GetComponent<CharacterBinding>();
                playerCharacter.SetBinding(binding);

                PlayerCamera = Object.Instantiate(PlayerCameraPrefab);
                playerCamera = PlayerCamera.GetComponent<CinemachineVirtualCamera>();
                playerCamera.Follow = PlayerObject.transform;
                playerCamera.LookAt = PlayerObject.transform;
                EventManager.Defer("PlayerSpawn", new DefaultEventData() { data = playerCharacter, source = this });
            }
            else
            {
                Debug.LogError("No player spawn found in scene");
            }
        }

        public void PlayerDead()
        {
        }

        public void Update()
        {
            if (PlayerObject != null)
                playerCharacter.Update();
        }

        public void FixedUpdate()
        {
            if (PlayerObject != null)
                playerCharacter.FixedUpdate();
        }

        public void LateUpdate()
        {
            if (PlayerObject != null)
                playerCharacter.LateUpdate();
        }       
    }
}
