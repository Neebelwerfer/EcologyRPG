using EcologyRPG.Core.Abilities;
using EcologyRPG.Game.NPC;
using EcologyRPG.Game.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace EcologyRPG.Game
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        [Header("NPC Settings")]
        public float maxDistance = 200f;
        public float activeEnemyUpdateRate = 0.2f;

        EnemyManager enemyManager;

        void Start()
        {
            GameManager.Instance.CurrentState = Game_State.Playing;
        }

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

            enemyManager = EnemyManager.Init(maxDistance, activeEnemyUpdateRate);
            AbilityManager.Init();
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            Player.PlayerManager.Instance.SpawnPlayer();

        }

        public void Update()
        {
            if (GameManager.Instance.CurrentState == Game_State.Playing)
            {
                enemyManager.Update();
                AbilityManager.instance.Update();
            }
        }

        public void LateUpdate()
        {
            if (GameManager.Instance.CurrentState == Game_State.Playing)
            {
                enemyManager.LateUpdate();
            }
        }
    }
}
