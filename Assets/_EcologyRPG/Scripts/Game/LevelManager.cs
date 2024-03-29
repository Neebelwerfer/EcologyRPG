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

        public UnityEvent OnLevelStart;
        public UnityEvent OnLevelAwake;

        EnemyManager enemyManager;

        void Start()
        {
            OnLevelStart.Invoke();
            enemyManager = EnemyManager.Init(maxDistance, activeEnemyUpdateRate);
            AbilityManager.Init();

            GameManager.Instance.CurrentState = Game_State.Playing;
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            PlayerManager.Instance.SpawnPlayer();
        }

        void Awake()
        {
            if (GameManager.Instance == null)
            {
                SceneManager.LoadScene(0, LoadSceneMode.Additive);
            }
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            OnLevelAwake.Invoke();

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
