using EcologyRPG.Core;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Systems;
using EcologyRPG.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                Debug.Log("Game Instance Created");
            }
            else
            {
                Destroy(gameObject);
            }
            Settings = Resources.Load<GameSettings>("Config/GameSettings");
            MoveToScene.TransitionSceneReference = Settings.TransitionScene;
            Characters.BaseMoveSpeed = Settings.BaseMoveSpeed;

#if UNITY_EDITOR
            Init();
#else
            var op = Settings.MainMenuScene.LoadSceneAsync(UnityEngine.SceneManagement.LoadSceneMode.Additive);
            op.completed += (p) =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(Settings.MainMenuScene.BuildIndex));
            };
#endif
        }

        void Init()
        {
            if(Player.Instance != null)
            {
                return;
            }
            TaskManager.Init();
            SystemManager.Init();
            Player.Init(Settings.playerSettings);
            Flags = new Flags();
        }

        public static void StartGame()
        {
            Instance.Init();
            MoveToScene.Load(Settings.MainLevel);
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

        public static void ExitGame()
        {
#if !UNITY_EDITOR
            Application.Quit();
#else
            UnityEditor.EditorApplication.isPlaying = false;
#endif
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
