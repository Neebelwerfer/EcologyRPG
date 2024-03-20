using Character;
using System;
using UnityEngine;
using Character.Abilities;

public static class ProjectileUtility
{
    public static void CreateProjectile(GameObject prefab, Vector3 Origin, Vector3 targetPos, float speed, bool destroyOnhit, LayerMask mask, BaseCharacter owner, Action<BaseCharacter> onHit)
    {
        GameObject projectileObj = ProjectilePoolHandler.Instance.GetProjectile(prefab);
        projectileObj.transform.SetPositionAndRotation(Origin, Quaternion.identity);
        if (projectileObj.TryGetComponent<ProjectileBehaviour>(out var projectile))
        {
            projectile.Init(new Vector3[]
            {
                targetPos
            }, speed, destroyOnhit, mask, owner, onHit);
        }
        else
        {
            projectile = projectileObj.AddComponent<ProjectileBehaviour>();
            projectile.Init(new Vector3[]
            {
                targetPos
            }, speed, destroyOnhit, mask, owner, onHit);
        }
        projectileObj.SetActive(true);
    }

    public static void CreateProjectile(GameObject prefab, Vector3 targetPos, float speed, bool destroyOnhit, LayerMask mask, BaseCharacter owner, Action<BaseCharacter> onHit)
    {
        GameObject projectileObj = ProjectilePoolHandler.Instance.GetProjectile(prefab);
        projectileObj.transform.SetPositionAndRotation(owner.AbilityPoint.transform.position, Quaternion.identity); 
        if(projectileObj.TryGetComponent<ProjectileBehaviour>(out var projectile))
        {
            projectile.Init(new Vector3[]
            {
                targetPos
            }, speed, destroyOnhit, mask, owner, onHit);
        }
        else
        {
            projectile = projectileObj.AddComponent<ProjectileBehaviour>();
            projectile.Init(new Vector3[]
            {
                targetPos
            }, speed, destroyOnhit, mask, owner, onHit);
        }
        projectileObj.SetActive(true);
    }

    public static void CreateProjectile(GameObject prefab, Vector3[] path, float speed, bool destroyOnhit, LayerMask mask, BaseCharacter owner, Action<BaseCharacter> onHit)
    {
        GameObject projectileObj = ProjectilePoolHandler.Instance.GetProjectile(prefab);
        projectileObj.transform.SetPositionAndRotation(owner.AbilityPoint.transform.position, Quaternion.identity);
        if (projectileObj.TryGetComponent<ProjectileBehaviour>(out var projectile))
        {
            projectile.Init(path, speed, destroyOnhit, mask, owner, onHit);

        }
        else
        {
            projectile = projectileObj.AddComponent<ProjectileBehaviour>();
            projectile.Init(path, speed, destroyOnhit, mask, owner, onHit);

        }
        projectileObj.SetActive(true);
    }

    public static void CreateCurvedProjectile(GameObject prefab, Vector3 targetPos, float time, float angle, LayerMask IgnoreMask, BaseCharacter owner, Action<GameObject> OnGroundHit)
    {
        GameObject projectileObj = ProjectilePoolHandler.Instance.GetProjectile(prefab);
        projectileObj.transform.SetPositionAndRotation(owner.AbilityPoint.transform.position, Quaternion.identity);

        if(projectileObj.TryGetComponent<CurvedProjectileBehaviour>(out var projectile))
        {
            projectile.Init(targetPos, time, angle, IgnoreMask, owner, OnGroundHit);
        }
        else
        {
            projectile = projectileObj.AddComponent<CurvedProjectileBehaviour>();
            projectile.Init(targetPos, time, angle, IgnoreMask, owner, OnGroundHit);
        }
        projectileObj.SetActive(true);
    }
}