using UnityEngine.Events;
using UnityEngine;
using EcologyRPG.Core.Character;
using Cinemachine;

namespace EcologyRPG.Game.Player
{
    public class PlayerManager : MonoBehaviour
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

        public void Init()
        {
            if (Instance == null)
            {
                Instance = this;
                Debug.Log("PlayerManager created");
            }
            else
            {
                Destroy(this);
            }
            playerCharacter = new(playerSettings);
            SpawnPlayer();
        }

        public void SpawnPlayer()
        {
            var spawn = GameObject.FindGameObjectWithTag("PlayerSpawn");

            if (spawn != null)
            {
                Player = Instantiate(PlayerPrefab, spawn.transform.position, spawn.transform.rotation);
                var binding = Player.GetComponent<CharacterBinding>();
                playerCharacter.SetBinding(binding);

                PlayerCamera = Instantiate(PlayerCameraPrefab);
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

        private void Update()
        {
            if (Player != null)
                playerCharacter.Update();
        }

        private void FixedUpdate()
        {
            if (Player != null)
                playerCharacter.FixedUpdate();
        }

        private void LateUpdate()
        {
            if (Player != null)
                playerCharacter.LateUpdate();
        }
    }
}
