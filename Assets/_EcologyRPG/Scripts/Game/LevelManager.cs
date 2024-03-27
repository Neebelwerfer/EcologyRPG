using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace EcologyRPG.Game
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        public UnityEvent OnLevelStart;

        public UnityEvent OnLevelAwake;

        void Start()
        {
            OnLevelStart.Invoke();
            GameManager.Instance.CurrentState = Game_State.Playing;
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
    }
}
