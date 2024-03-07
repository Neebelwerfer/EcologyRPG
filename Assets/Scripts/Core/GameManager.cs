using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public Game_State CurrentState = Game_State.Menu;
        
    // Start is called before the first frame update
    
    void Awake()
    {
        if(Instance == null)
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
        if(CurrentState == Game_State.Playing)
        {
            EventManager.UpdateQueue();
        }
    }

    public void FixedUpdate()
    {
        if(CurrentState == Game_State.Playing)
        {
            EventManager.FixedUpdateQueue();
        }
    }

    public void LateUpdate()
    {
        if(CurrentState == Game_State.Playing)
        {
            EventManager.LateUpdateQueue();
        }
    }
}
