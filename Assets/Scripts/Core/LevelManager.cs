using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public UnityEvent OnLevelStart;

    public UnityEvent OnLevelAwake;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (GameManager.Instance == null)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Additive);
        }

        OnLevelStart.Invoke();
    }

    void Awake()
    {
        OnLevelAwake.Invoke();
    }
}
