using EcologyRPG.Core;
using EcologyRPG.Core.Character;
using EcologyRPG.Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using PlayerSettings = EcologyRPG.Game.Player.PlayerSettings;

namespace EcologyRPG.Game
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
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public static GameSettings Settings;


        public Game_State CurrentState = Game_State.Menu;

        // Start is called before the first frame update

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Load()
        {
            Settings = Resources.Load<GameSettings>("Config/GameSettings");
            PlayerManager.Init(Settings.playerSettings);
            Flags.Init();
            SceneManager.LoadScene(0, LoadSceneMode.Additive);
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
        }

        public void Update()
        {
            if (CurrentState == Game_State.Playing)
            {
                EventManager.UpdateQueue();
                PlayerManager.Instance.Update();
                
            }
        }

        public void FixedUpdate()
        {
            if (CurrentState == Game_State.Playing)
            {
                PlayerManager.Instance.FixedUpdate();
            }
        }

        public void LateUpdate()
        {
            if (CurrentState == Game_State.Playing)
            {
                PlayerManager.Instance.LateUpdate();
            }
        }
    }
}
