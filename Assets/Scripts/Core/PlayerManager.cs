using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public GameObject PlayerPrefab;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("PlayerManager created");
        }
        else
        {
            Destroy(this);
        }
    }

    public void SpawnPlayer()
    {
        var spawn = GameObject.FindGameObjectWithTag("PlayerSpawn");

        if (spawn != null)
        {
            Instantiate(PlayerPrefab, spawn.transform.position, spawn.transform.rotation);
        }
        else
        {
            Debug.LogError("No player spawn found in scene");
        }
    }
}
