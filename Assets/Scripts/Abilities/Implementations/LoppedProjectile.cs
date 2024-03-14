using Character.Abilities;
using Codice.Client.Commands;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/LoppedProjectile", fileName = "New Lopped Projectile")]
public class LoppedProjectile : AttackAbility
{
    [Header("Lopped Projectile")]
    [Tooltip("The prefab of the projectile")]
    public GameObject ProjectilePrefab;
    [Tooltip("The layer mask of the colliders the projectile can travel through")]
    public LayerMask ignoreMask;
    [Tooltip("The angle of the projectile")]
    public float Angle;
    [Tooltip("The travel time of the projectile")]
    public float TravelTime;
    [Tooltip("The ability that will be cast when the projectile hits the ground")]
    public BaseAbility OnHitAbility;

    Vector3 MousePoint;

    public override bool CanActivate(CasterInfo caster)
    {
        if(!base.CanActivate(caster))
        {
            return false;
        }

        MousePoint = TargetUtility.GetMousePoint(Camera.main);
        if (Vector3.Distance(caster.castPos, MousePoint) > Range)
        {
            return false;
        }
        return true;
    }

    public override void CastStarted(CasterInfo caster)
    {
        base.CastStarted(caster);
    }

    public override void CastEnded(CasterInfo caster)
    {
        base.CastEnded(caster);
        Debug.DrawRay(MousePoint, Vector3.up * 10, Color.red, 5);  
        ProjectileUtility.CreateCurvedProjectile(ProjectilePrefab, MousePoint, TravelTime, -Angle, ignoreMask, caster.owner, (projectileObject) =>
        {
            Cast(new CasterInfo { owner = caster.owner, castPos = projectileObject.transform.position }, OnHitAbility);
        });
    }

    public override void OnHold(CasterInfo caster)
    {
    }
}