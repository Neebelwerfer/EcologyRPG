using Character;
using System;
using UnityEngine;
using Character.Abilities;

public static class ProjectileUtility
{

    public static void CreateProjectile(GameObject prefab, Vector3 targetPos, float speed, float damage, DamageType damageType, bool destroyOnhit, LayerMask mask, BaseCharacter owner, Action<BaseCharacter> onHit)
    {
        GameObject projectileObj = GameObject.Instantiate(prefab, owner.AbilityPoint.transform.position, Quaternion.identity);
        ProjectileBehaviour projectile = projectileObj.AddComponent<ProjectileBehaviour>();
        projectile.path = new Vector3[]
        {
            targetPos
        };
        projectile.speed = speed;
        projectile.damage = damage;
        projectile.damageType = damageType;
        projectile.DestroyOnCollision = destroyOnhit;
        projectile.layerMask = mask;
        projectile.owner = owner;
        projectile.OnHit = onHit;
    }

    public static void CreateProjectile(GameObject prefab, Vector3[] path, float speed, float damage, DamageType damageType, bool destroyOnhit, LayerMask mask, BaseCharacter owner, Action<BaseCharacter> onHit)
    {
        GameObject projectileObj = GameObject.Instantiate(prefab, owner.AbilityPoint.transform.position, Quaternion.identity);
        ProjectileBehaviour projectile = projectileObj.AddComponent<ProjectileBehaviour>();
        projectile.path = path;
        projectile.speed = speed;
        projectile.damage = damage;
        projectile.damageType = damageType;
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

[RequireComponent(typeof(Collider))]
public class CurvedProjectileBehaviour : MonoBehaviour
{
    public Vector3 target;
    public float time;
    public float angle;
    public LayerMask IgnoreMask;
    public BaseCharacter owner;

    public Action<GameObject> OnGroundHit;

    Rigidbody rb;
    Collider col;
    private void Start()
    {
        if(TryGetComponent<Rigidbody>(out var body))
        {
            rb = body;
        }
        else
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.mass = 0;
        rb.drag = 0.5f;
        col = GetComponent<Collider>();
        col.isTrigger = false;
        col.excludeLayers = IgnoreMask;
        transform.rotation = Quaternion.LookRotation(target - transform.position);
        transform.Rotate(angle, 0, 0);
        rb.velocity = CalculateInitialVelocity(Vector3.Distance(owner.Position, target), time, angle) * transform.forward;

    }

    float CalculateInitialVelocity(float distance, float time, float angle)
    {
        return distance / (time * Mathf.Cos(angle));
    }


    private void OnCollisionEnter(Collision collision)
    {
        OnGroundHit?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
