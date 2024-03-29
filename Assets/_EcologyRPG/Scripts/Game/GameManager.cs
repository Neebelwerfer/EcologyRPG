using EcologyRPG.Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Player Settings")]
        public GameObject PlayerPrefab;
        public GameObject PlayerCameraPrefab;
        public PlayerSettings playerSettings;


        public Game_State CurrentState = Game_State.Menu;

        PlayerManager PlayerManager;

        // Start is called before the first frame update

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
            PlayerManager = PlayerManager.Init(PlayerPrefab, PlayerCameraPrefab, playerSettings);
        }

        public void Update()
        {
            if (CurrentState == Game_State.Playing)
            {
                EventManager.UpdateQueue();
                PlayerManager.Update();
            }
        }

        public void FixedUpdate()
        {
            if (CurrentState == Game_State.Playing)
            {
                PlayerManager.FixedUpdate();
            }
        }

        public void LateUpdate()
        {
            if (CurrentState == Game_State.Playing)
            {
                PlayerManager.LateUpdate();
            }
        }
    }
}
