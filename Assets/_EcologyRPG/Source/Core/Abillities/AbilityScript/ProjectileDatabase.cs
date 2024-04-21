using EcologyRPG.Core.Abilities;
using UnityEngine;

namespace EcologyRPG.AbilityScripting
{
    public class ProjectileDatabase : ScriptableObject
    {
        public static ProjectileDatabase Instance;
        public const string ResourcePath = "ProjectileDatabase";
        public const string ResourceFullPath = "Assets/_EcologyRPG/Resources/" + ResourcePath + ".asset";

        public Projectile[] projectiles = new Projectile[0];
        ProjectilePool[] pools;

        public void Init()
        {
            pools = new ProjectilePool[projectiles.Length];
            for (int i = 0; i < projectiles.Length; i++)
            {
                pools[i] = new ProjectilePool(projectiles[i]);
            }
        }

        public Projectile GetProjectile(int id)
        {
            return pools[id].GetProjectile();
        }

        public void ReturnProjectile(int id, Projectile projectile)
        {
            pools[id].ReturnProjectile(projectile);
        }

        public static void Load()
        {
            if(Instance == null)
            {
                Instance = Resources.Load<ProjectileDatabase>(ResourcePath);
            }
        }
    }
}