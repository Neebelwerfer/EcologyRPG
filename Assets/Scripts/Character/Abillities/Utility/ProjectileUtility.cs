using Character;
using Codice.CM.Common;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.UI;

public static class ProjectileUtility
{
    public static readonly Action<BaseCharacter, BaseCharacter, float> DefaultOnHit = (Castee, target, damage) =>
    {
        target.ApplyDamage(Castee, damage);
    };

    public static void CreateProjectile(GameObject prefab, Vector3 targetPos, float speed, float damage, bool destroyOnhit, LayerMask mask, BaseCharacter owner)
    {
        GameObject projectileObj = GameObject.Instantiate(prefab, owner.AbilityPoint.transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.AddComponent<Projectile>();

        projectile.path = new Vector3[]
        {
            targetPos
        };
        projectile.speed = speed;
        projectile.damage = damage;
        projectile.DestroyOnCollision = destroyOnhit;
        projectile.layerMask = mask;
        projectile.owner = owner;
        projectile.OnHit = ProjectileUtility.DefaultOnHit;
    }

    public static void CreateProjectile(GameObject prefab, Vector3 targetPos, float speed, float damage, bool destroyOnhit, LayerMask mask, BaseCharacter owner, Action<BaseCharacter, BaseCharacter, float> onHit)
    {
        GameObject projectileObj = GameObject.Instantiate(prefab, owner.AbilityPoint.transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.AddComponent<Projectile>();
        projectile.path = new Vector3[]
        {
            targetPos
        };
        projectile.speed = speed;
        projectile.damage = damage;
        projectile.DestroyOnCollision = destroyOnhit;
        projectile.layerMask = mask;
        projectile.owner = owner;
        projectile.OnHit = onHit;
    }

    public static void CreateProjectile(GameObject prefab, Vector3[] path, float speed, float damage, bool destroyOnhit, LayerMask mask, BaseCharacter owner)
    {
        GameObject projectileObj = GameObject.Instantiate(prefab, owner.AbilityPoint.transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.AddComponent<Projectile>();
        projectile.path = path;
        projectile.speed = speed;
        projectile.damage = damage;
        projectile.DestroyOnCollision = destroyOnhit;
        projectile.layerMask = mask;
        projectile.owner = owner;
        projectile.OnHit = ProjectileUtility.DefaultOnHit;
    }

    public static void CreateProjectile(GameObject prefab, Vector3[] path, float speed, float damage, bool destroyOnhit, LayerMask mask, BaseCharacter owner, Action<BaseCharacter, BaseCharacter, float> onHit)
    {
        GameObject projectileObj = GameObject.Instantiate(prefab, owner.AbilityPoint.transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.AddComponent<Projectile>();
        projectile.path = path;
        projectile.speed = speed;
        projectile.damage = damage;
        projectile.DestroyOnCollision = destroyOnhit;
        projectile.layerMask = mask;
        projectile.owner = owner;
        projectile.OnHit = onHit;
    }
}

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    public Vector3[] path;
    public float speed;
    public float damage;
    public bool DestroyOnCollision;
    public LayerMask layerMask;
    public BaseCharacter owner;

    public Action<BaseCharacter, BaseCharacter, float> OnHit;

    int counter = 0;

    public void Start()
    {
        if (path.Length == 0)
        {
            Destroy(gameObject);
        }

        GetComponent<Collider>().isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Projectile");
    }


    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, path[counter], speed * Time.deltaTime);

        if(transform.position == path[counter])
        {
            counter++;
        }

        if(counter == path.Length)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != owner.gameObject && other.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            if (other.gameObject.TryGetComponent<BaseCharacter>(out var character))
            {
                OnHit(owner, character, damage);
            }

            if (DestroyOnCollision)
            {
                Destroy(gameObject);
            }
        }
    }
}
