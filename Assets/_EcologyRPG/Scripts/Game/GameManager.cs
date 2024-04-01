using EcologyRPG.Core;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Systems;
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
            SystemManager.Init();
            PlayerManager.Init(Settings.playerSettings);
            Flags.Init();
        }

        public void Update()
        {
            if (CurrentState == Game_State.Playing)
            {
                EventManager.UpdateQueue();
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
                PlayerManager.Instance.Dispose();
            }            
        }
    }
}
