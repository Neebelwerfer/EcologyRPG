using Character;
using System;
using UnityEngine;
using Character.Abilities;

public static class ProjectileUtility
{
    public static void CreateProjectile(GameObject prefab, Vector3 Origin, Vector3 targetPos, float speed, bool destroyOnhit, LayerMask mask, BaseCharacter owner, Action<BaseCharacter> onHit)
    {
        GameObject projectileObj = GameObject.Instantiate(prefab, Origin, Quaternion.identity);
        ProjectileBehaviour projectile = projectileObj.AddComponent<ProjectileBehaviour>();
        projectile.path = new Vector3[]
        {
            targetPos
        };
        projectile.speed = speed;
        projectile.DestroyOnCollision = destroyOnhit;
        projectile.layerMask = mask;
        projectile.owner = owner;
        projectile.OnHit = onHit;
    }

    public static void CreateProjectile(GameObject prefab, Vector3 targetPos, float speed, bool destroyOnhit, LayerMask mask, BaseCharacter owner, Action<BaseCharacter> onHit)
    {
        GameObject projectileObj = GameObject.Instantiate(prefab, owner.AbilityPoint.transform.position, Quaternion.identity);
        ProjectileBehaviour projectile = projectileObj.AddComponent<ProjectileBehaviour>();
        projectile.path = new Vector3[]
        {
            targetPos
        };
        projectile.speed = speed;
        projectile.DestroyOnCollision = destroyOnhit;
        projectile.layerMask = mask;
        projectile.owner = owner;
        projectile.OnHit = onHit;
    }

    public static void CreateProjectile(GameObject prefab, Vector3[] path, float speed, bool destroyOnhit, LayerMask mask, BaseCharacter owner, Action<BaseCharacter> onHit)
    {
        GameObject projectileObj = GameObject.Instantiate(prefab, owner.AbilityPoint.transform.position, Quaternion.identity);
        ProjectileBehaviour projectile = projectileObj.AddComponent<ProjectileBehaviour>();
        projectile.path = path;
        projectile.speed = speed;
        projectile.DestroyOnCollision = destroyOnhit;
        projectile.layerMask = mask;
        projectile.owner = owner;
        projectile.OnHit = onHit;
    }

    public static void CreateCurvedProjectile(GameObject prefab, Vector3 targetPos, float time, float angle, LayerMask IgnoreMask, BaseCharacter owner, Action<GameObject> OnGroundHit)
    {
        GameObject projectileObj = GameObject.Instantiate(prefab, owner.AbilityPoint.transform.position, Quaternion.identity);
        CurvedProjectileBehaviour projectile = projectileObj.AddComponent<CurvedProjectileBehaviour>();
        projectile.target = targetPos;
        projectile.time = time;
        projectile.angle = angle;
        projectile.IgnoreMask = IgnoreMask;
        projectile.owner = owner;
        projectile.OnGroundHit = OnGroundHit;
    }
}