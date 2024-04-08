using EcologyRPG.Core;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Systems;
using EcologyRPG.GameSystems.PlayerSystems;
using UnityEngine;

namespace EcologyRPG.GameSystems
{
    public enum Game_State
    {
        Menu,
        Playing,
        Paused,
        DialoguePlaying,
        DialogueChoices,
    }

    [DefaultExecutionOrder(-1000)]
    public class Game : MonoBehaviour
    {
        public static Game Instance;
        public static GameSettings Settings;
        public static Flags Flags;

        public Game_State CurrentState = Game_State.Menu;

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
            Init();
        }

        void Init()
        {
            Settings = Resources.Load<GameSettings>("Config/GameSettings");
            Characters.BaseMoveSpeed = Settings.BaseMoveSpeed;
            TaskManager.Init();
            SystemManager.Init();
            Player.Init(Settings.playerSettings);
            Flags = new Flags();
        }

        public void Pause()
        {
            CurrentState = Game_State.Paused;
            Time.timeScale = 0;
        }

        public void Resume()
        {
            CurrentState = Game_State.Playing;
            Time.timeScale = 1;
        }

        public void Update()
        {
            if (CurrentState == Game_State.Playing)
            {
                EventManager.UpdateQueue();
                TaskManager.Update();
                SystemManager.Update();
            }
        }

        public void FixedUpdate()
        {
            if (CurrentState == Game_State.Playing)
            {
                SystemManager.FixedUpdate();
            }
        }

        public void LateUpdate()
        {
            if (CurrentState == Game_State.Playing)
            {
                SystemManager.LateUpdate();
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
                SystemManager.Instance.Dispose();
                Player.Instance.Dispose();
                TaskManager.Instance.Dispose();
            }            
        }
    }
}
