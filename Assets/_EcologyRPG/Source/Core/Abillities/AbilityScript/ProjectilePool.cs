using EcologyRPG.Core.Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.AbilityScripting
{
    internal class ProjectilePool
    {
        readonly Stack<Projectile> pool = new Stack<Projectile>();
        readonly Projectile prefab;

        public ProjectilePool(Projectile prefab) 
        { 
            this.prefab = prefab;
        }

        public Projectile GetProjectile()
        {
            if (pool.Count > 0)
            {
                return pool.Pop();
            }
            else
            {
                return Object.Instantiate(prefab);
            }
        }

        public void ReturnProjectile(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            pool.Push(projectile);
        }
    }
}