using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utility;

public class ProjectilePoolHandler
{
    static ProjectilePoolHandler instance;
    public static ProjectilePoolHandler Instance
    {
        get
        {
            instance ??= new ProjectilePoolHandler();
            return instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Setup()
    {
        instance = new ProjectilePoolHandler();
    }

    private readonly Dictionary<string, GameObjectPool> projectileMap;

    private ProjectilePoolHandler()
    {
        projectileMap = new Dictionary<string, GameObjectPool>();
    }

    public GameObject GetProjectile(GameObject projectilePrefab)
    {
        if (!projectileMap.ContainsKey(projectilePrefab.name))
        {
            projectileMap.Add(projectilePrefab.name, new GameObjectPool(projectilePrefab));
            projectileMap[projectilePrefab.name].Preload(10);
        }

        return projectileMap[projectilePrefab.name].GetObject();
    }

    public void ReturnProjectile(GameObject projectile)
    {
        if (!projectileMap.ContainsKey(projectile.name))
        {
            Debug.LogError("ProjectilePoolHandler: ReturnProjectile: Projectile not found in pool");
            return;
        }

        projectileMap[projectile.name].ReturnObject(projectile);
    }
}