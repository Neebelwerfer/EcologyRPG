using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    
    [SerializeField] List<EnemySpawner> spawners = new List<EnemySpawner>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddSpawner(EnemySpawner spawner)
    {
        spawners.Add(spawner);
    }
}
