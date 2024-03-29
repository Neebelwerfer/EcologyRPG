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

        public UnityEvent OnPlayerSpawned;

        GameObject Player;
        PlayerCharacter playerCharacter;

        GameObject PlayerCamera;
        CinemachineVirtualCamera playerCamera;

        PlayerManager(GameObject playerPrefab, GameObject playerCameraPrefab, PlayerSettings playerSettings)
        {
            PlayerPrefab = playerPrefab;
            PlayerCameraPrefab = playerCameraPrefab;
            this.playerSettings = playerSettings;
            playerCharacter = new(playerSettings);
        }

        public static PlayerManager Init(GameObject playerPrefab, GameObject playerCameraPrefab, PlayerSettings playerSettings)
        {
            Instance = new PlayerManager(playerPrefab, playerCameraPrefab, playerSettings);
            return Instance;
        }

        public void SpawnPlayer()
        {
            var spawn = GameObject.FindGameObjectWithTag("PlayerSpawn");

            if (spawn != null)
            {
                Player = Object.Instantiate(PlayerPrefab, spawn.transform.position, spawn.transform.rotation);
                var binding = Player.GetComponent<CharacterBinding>();
                playerCharacter.SetBinding(binding);

                PlayerCamera = Object.Instantiate(PlayerCameraPrefab);
                playerCamera = PlayerCamera.GetComponent<CinemachineVirtualCamera>();
                playerCamera.Follow = Player.transform;
                playerCamera.LookAt = Player.transform;
                OnPlayerSpawned?.Invoke();
            }
            else
            {
                Debug.LogError("No player spawn found in scene");
            }
        }

        public void PlayerDead()
        {
        }

        public GameObject GetPlayer()
        {
            return Player;
        }

        public PlayerCharacter GetPlayerCharacter()
        {
            return playerCharacter;
        }

        public void Update()
        {
            if (Player != null)
                playerCharacter.Update();
        }

        public void FixedUpdate()
        {
            if (Player != null)
                playerCharacter.FixedUpdate();
        }

        public void LateUpdate()
        {
            if (Player != null)
                playerCharacter.LateUpdate();
        }
    }
}
