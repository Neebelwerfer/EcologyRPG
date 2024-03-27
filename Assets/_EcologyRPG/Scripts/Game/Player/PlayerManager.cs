using UnityEngine.Events;
using UnityEngine;

namespace EcologyRPG.Game.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;

        public GameObject PlayerPrefab;

        public UnityEvent OnPlayerSpawned;

        GameObject Player;
        PlayerCharacter playerCharacter;

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
            SpawnPlayer();
        }

        public void SpawnPlayer()
        {
            var spawn = GameObject.FindGameObjectWithTag("PlayerSpawn");

            if (spawn != null)
            {
                Player = Instantiate(PlayerPrefab, spawn.transform.position, spawn.transform.rotation);
                playerCharacter = Player.GetComponent<PlayerCharacter>();
                OnPlayerSpawned?.Invoke();
            }
            else
            {
                Debug.LogError("No player spawn found in scene");
            }
        }

        public GameObject GetPlayer()
        {
            return Player;
        }

        public PlayerCharacter GetPlayerCharacter()
        {
            return playerCharacter;
        }
    }
}
