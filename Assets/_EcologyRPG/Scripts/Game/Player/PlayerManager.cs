using UnityEngine.Events;
using UnityEngine;
using EcologyRPG.Core.Character;
using Cinemachine;

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

        GameObject PlayerCamera;
        CinemachineVirtualCamera playerCamera;

        PlayerManager(PlayerSettings playerSettings)
        {
            PlayerPrefab = playerSettings.PlayerModel;
            PlayerCameraPrefab = playerSettings.Camera;
            this.playerSettings = playerSettings;
            playerCharacter = new(playerSettings);
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

        public static bool IsPlayerAlive => Instance.PlayerObject != null;

        public static PlayerCharacter GetPlayer()
        {
            return Instance.playerCharacter;
        }

       
    }
}
