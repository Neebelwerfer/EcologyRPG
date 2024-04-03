using EcologyRPG.Core;
using EcologyRPG.Core.Abilities;
using EcologyRPG.GameSystems.NPC;
using EcologyRPG.GameSystems.PlayerSystems;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcologyRPG.GameSystems
{
    public class Level : MonoBehaviour
    {
        public static Level Instance;

        [Header("Spawn Points")]
        public Transform playerStartSpawnPoint;
        [HideInInspector] public GameObject[] respawnPoints;

        [Header("NPC Settings")]
        public float maxDistance = 200f;
        public float activeEnemyUpdateRate = 0.2f;

        [Header("Day Night Cycle")]
        public GameObject day;
        public GameObject night;
        public float cycleDuration = 60f;

        DayNightCycle dayNightCycle;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if(playerStartSpawnPoint == null)
            {
                Debug.LogError("Player Start Spawn Point not set in Level Manager");
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
            }
            respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

            EnemyManager.Init(maxDistance, activeEnemyUpdateRate);
            AbilityManager.Init();
            ProjectileSystem.Init();
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            Player.Instance.SpawnPlayer();
            Game.Instance.CurrentState = Game_State.Playing;
            if (day != null && night != null) dayNightCycle = new DayNightCycle(day, night, cycleDuration);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
            ProjectileSystem.Instance.Dispose();
            AbilityManager.Instance.Dispose();
            EnemyManager.Instance.Dispose();
            dayNightCycle.Dispose();
            dayNightCycle = null;
            SceneManager.UnloadSceneAsync(1);
        }
    }
}
